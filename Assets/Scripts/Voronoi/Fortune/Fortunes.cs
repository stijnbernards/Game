using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class Fortunes : MonoBehaviour
{
    private SweepLine sweepLine;
    private BeachLine beachLine;
    private List<Vector2> points;

    public GameObject wallSprite = Resources.Load("Stone") as GameObject;
    public GameObject groundSprite = Resources.Load("Ground") as GameObject;

    public void FortunesStart(Vector2[] points)
    {
        this.points = points.ToList();
        sweepLine = new SweepLine(ref this.points);
        beachLine = new BeachLine();
        sweepLine.OnIntersect += IntersectEvent;
        StartCoroutine("StartMovingLine");
    }

    IEnumerator StartMovingLine()
    {
        float y0 = -1, y1 = -1;

        points.ForEach(y =>
        {
            if (y0 == -1)
                y0 = y.y;
            else if (y.y < y0)
                y0 = y.x;

            if (y1 == -1)
                y1 = y.y;
            else if (y.y > y1)
                y1 = y.y;
        });

        while (sweepLine.Y < y1 - 1)
        {
            sweepLine.Move();
            beachLine.OnSweepMove(sweepLine.Y);
            yield return new WaitForSeconds(0.8F);
        }

        //draws an end visualization
        //beachLine.parabolas.ForEach(x =>
        //{
        //    x.ToList().ForEach(y =>
        //    {
        //        GameObject.Instantiate(wallSprite, y, Quaternion.identity);
        //    });
        //});
        //for (int x = -50; x < 50; x++)
        //{
        //    GameObject.Instantiate(wallSprite, new Vector2(x, sweepLine.Y), Quaternion.identity);
        //}

        //beachLine.endPoints.ForEach(x =>
        //{
        //    GameObject.Instantiate(wallSprite, x, Quaternion.identity);
        //});

        yield return 0;

    }

    private void IntersectEvent(Vector2 point)
    {
        beachLine.RegisterPoint(point);
    }

    private class SweepLine
    {
        public float Y { get { return pos; } }

        public delegate void IntersectEvent(Vector2 point);

        public event IntersectEvent OnIntersect;

        private float pos = 0;
        private List<Vector2> points;

        public SweepLine(ref List<Vector2> points)
        {
            this.points = points;
        }

        public void Move()
        {
            pos += 1F;
            CheckForIntersect();
        }

        private void CheckForIntersect()
        {
            points.FindAll(x => x.y == Mathf.Round(pos)).ForEach(x => OnIntersect(x));
        }
    }

    private class BeachLine
    {
        public GameObject groundSprite = Resources.Load("Ground") as GameObject;
        public GameObject wallSprite = Resources.Load("Stone") as GameObject;
        public GameObject endSprite = Resources.Load("EndPoint") as GameObject;

        private List<GameObject> p = new List<GameObject>();
        private List<Vector2> points = new List<Vector2>();
        public List<Vector2> endPoints = new List<Vector2>();
        public List<Vector2> intersections = new List<Vector2>();
        public List<Vector2[]> parabolas = new List<Vector2[]>();
        private List<Parabola> parabolaValues = new List<Parabola>();


        public void RegisterPoint(Vector2 point)
        {
            points.Add(point);
            points = points.Distinct().ToList();
        }

        public void OnSweepMove(float sweepY)
        {
            List<Vector2> parabola = new List<Vector2>();
            Vector2 p1, p2, p3;
            float denom, a, b, c, x1, y1, x2, y2, discriminant;

            parabolas = new List<Vector2[]>();
            parabolaValues = new List<Parabola>();
            intersections = new List<Vector2>();

            p.ForEach(x =>
            {
                Destroy(x);
            });
            p = new List<GameObject>();

            for (int x = 0; x < 50; x++)
            {
                p.Add(GameObject.Instantiate(wallSprite, new Vector2(x, sweepY), Quaternion.identity) as GameObject);
            }

            if (points.Count > 0)
            {
                //Debug.Log("s");
                points.ForEach(x =>
                {
                    p1 = new Vector2(x.x, x.y + (sweepY - x.y) / 2);
                    p2 = new Vector2(x.x + ((sweepY - x.y) * 2), x.y - ((sweepY - x.y) * 2));
                    p3 = new Vector2(x.x - ((sweepY - x.y) * 2), x.y - ((sweepY - x.y) * 2));

                    denom = (p1.x - p2.x) * (p1.x - p3.x) * (p2.x - p3.x);

                    if (denom != 0) 
                    { 
                        a = (p3.x * (p2.y - p1.y) + p2.x * (p1.y - p3.y) + p1.x * (p3.y - p2.y)) / denom;
                        b = (p3.x * p3.x * (p1.y - p2.y) + p2.x * p2.x * (p3.y - p1.y) + p1.x * p1.x * (p2.y - p3.y)) / denom;
                        c = (p2.x * p3.x * (p2.x - p3.x) * p1.y + p3.x * p1.x * (p3.x - p1.x) * p2.y + p1.x * p2.x * (p1.x - p2.x) * p3.y) / denom;

                        for (float px = x.x - 10; px < x.x + 10; px += 1F)
                        {
                            parabola.Add(new Vector2(px, a * (px * px) + b * px + c));
                        }

                        parabolas.Add(parabola.ToArray());
                        parabolaValues.Add(new Parabola()
                        {
                            a = a,
                            b = b,
                            c = c,
                            IntersectionL = new List<Vector2>(),
                            IntersectionR = new List<Vector2>()
                        });
                        parabola = new List<Vector2>();
                    }
                });

                parabolas.ForEach(x =>
                {
                    x.ToList().ForEach(y =>
                    {
                        p.Add(GameObject.Instantiate(endSprite, y, Quaternion.identity) as GameObject);
                    });
                });

                if (parabolaValues.Count > 0)
                {
                    parabolaValues.ForEach(x =>
                    {
                        parabolaValues.ForEach(y =>
                        {
                            a = y.a - x.a;
                            b = y.b - x.b;
                            c = y.c - x.c;
                            if (a == 0.0 && b == 0.0 && c == 0.0)
                            {

                            }
                            else
                            {
                                if (a == 0.0)
                                {
                                    if (b != 0.0)
                                    {
                                        x1 = -c / b;
                                        y1 = x.a * x1 * x1 + x.b * x1 + x.c;
                                        x.IntersectionL.Add(new Vector2(x1, y1));
                                    }
                                }
                                else
                                {
                                    discriminant = b * b - 4 * a * c;

                                    if (discriminant >= 0.0)
                                    {
                                        if (discriminant == 0.0)
                                        {
                                            x1 = -b / (2 * a);
                                            y1 = x.a * x1 * x1 + x.b * x1 + x.c;
                                            x.IntersectionR.Add(new Vector2(x1, y1));
                                        }
                                        else
                                        {
                                            x1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
                                            y1 = x.a * x1 * x1 + x.b * x1 + x.c;
                                            x2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
                                            y2 = x.a * x2 * x2 + x.b * x2 + x.c;
                                            x.IntersectionR.Add(new Vector2(x1, y1));
                                            x.IntersectionL.Add(new Vector2(x2, y2));
                                        }
                                    }
                                }
                            }
                        });
                    });

                    parabolaValues.ForEach(x =>
                    {
                        x.IntersectionL = x.IntersectionL.OrderByDescending(y => y.y).ToList();
                        
                        x.IntersectionR = x.IntersectionR.OrderByDescending(y => y.y).ToList();
                        
                        if(x.IntersectionR.Count > 0)
                            intersections.Add(x.IntersectionR[0]);

                        if (x.IntersectionL.Count > 0)
                            intersections.Add(x.IntersectionL[0]);
                    });

                    intersections.ForEach(x =>
                    {
                        p.Add(GameObject.Instantiate(groundSprite, x, Quaternion.identity) as GameObject);
                    });

                    var group = intersections.GroupBy(x => new Vector2((float)System.Math.Floor(x.x), (float)System.Math.Floor(x.y)));

                    foreach (var grp in group)
                    {
                        //Debug.Log(grp.Count());
                        if (grp.Count() > 2)
                        {
                            //Debug.Log("lol");
                            //conti = false;
                            if (!float.IsNaN(grp.Key.x) && !float.IsInfinity(grp.Key.x))
                            {
                                if (!float.IsNaN(grp.Key.y) && !float.IsInfinity(grp.Key.y))
                                {
                                    //Debug.Log(grp.Key);
                                    //p.ForEach(x =>
                                    //{
                                    //    Destroy(x);
                                    //});
                                    endPoints.Add(grp.Key);
                                    GameObject.Instantiate(wallSprite, grp.Key, Quaternion.identity);
                                    //throw new System.Exception();
                                }
                            }
                        }
                    }
                }
            }
        }

        public struct Parabola
        {
            public float a;
            public float b;
            public float c;
            public List<Vector2> IntersectionL;
            public List<Vector2> IntersectionR;
        }
    }
}