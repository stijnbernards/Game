using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainIsland : Generate
{
    public GameObject groundSprite = Resources.Load("Ground") as GameObject;
    public GameObject wallSprite = Resources.Load("Stone") as GameObject;

    private Island island;

    public override void BeginPoint() { }
    public override void EndPoint() { }

    public override void GenerateLevel()
    {
        List<Vector2> points = new List<Vector2>(GenHelpers.GenrateAmountBetween(new Vector2(-125, -125), new Vector2(125, 125), 100));

        Voronoi v = Delaunay.DeriveVoronoi(points, Delaunay.Triangulate(points));

        Voronoi v2 = LloydRelaxation.Relax(v, new Vector2(-150, -150), new Vector2(150, 150), 50);
        v2.AssignPerlin();

        island = Island.VoronoiToIsland(v2, new Vector2(-100, -100), new Vector2(100, 100));

        //HashSet<Vector2> DrawPoints = new HashSet<Vector2>();

        foreach (VoronoiEdge edge in v2.Edges.OrderBy(x => x.Midpoint.Point.x).ThenBy(x => x.Midpoint.Point.y))
        {
            bool tex = true;
            GameObject curS = groundSprite;

            foreach (IslandPolygon poly in island.Polygons)
            {
                if (poly.Poly.MidPoint.Point == edge.Midpoint.Point)
                {
                    if (Mathf.Abs(poly.Poly.PerlinNoise - Mathf.Abs(poly.Poly.MidPoint.Point.x) - Mathf.Abs(poly.Poly.MidPoint.Point.y)) < 300F)
                    {
                        curS = wallSprite;
                        curS.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, Mathf.Round(poly.Elevation) / 255F, 1f);
                        tex = false;
                        break;
                    }
                }
            }

            foreach (Vector2 vect in GenHelpers.PlotLine((int)edge.site1.x, (int)edge.site1.y, (int)edge.site2.x, (int)edge.site2.y, false))
            {
                if (tex)
                {
                    GameObject.Instantiate(curS, vect, Quaternion.identity);
                }
                else
                {
                    GameObject.Instantiate(curS, vect, Quaternion.identity);
                }
            }
        }
    }

    public class IslandPolygon
    {
        public Polygon Poly;
        public bool IsWater = false;
        public float Elevation = 0.0F;

        public IslandPolygon(Polygon poly, bool isWater)
        {
            Poly = poly;
            IsWater = isWater;
        }
    }

    public class IslandTile
    {
        public Tile tile;
        public float Elevation = 0.0F;
        public bool IsWater = false;
        public Vector2 Point;
    }

    //Split into classes, todo smooth elevation etc...
    public class Island
    {
        public GameObject wallSprite = Resources.Load("Stone") as GameObject;
        public List<IslandPolygon> Polygons = new List<IslandPolygon>();
        public IslandTile[,] islandTiles;

        private Vector2 BottomLeft;
        private Vector2 TopRight;
        private int width;
        private int height;

        public Island(List<IslandPolygon> polygons, Vector2 bottomLeft, Vector2 topRight)
        {
            Polygons = polygons;
            BottomLeft = bottomLeft;
            TopRight = topRight;
            
            height = (int)Mathf.Abs(bottomLeft.y - topRight.y);
            width = (int)Mathf.Abs(bottomLeft.x - topRight.x);

            islandTiles = new IslandTile[width, height];

            RenderSquare(new Vector2(50, 50), new Vector2(100, 100));
            RenderSquare(new Vector2(50, 100), new Vector2(100, 150));
            RenderSquare(new Vector2(100, 100), new Vector2(150, 150));
            RenderSquare(new Vector2(150, 150), new Vector2(200, 200));
        }

        //Relative to island bounds so no fucking - numbers
        public void RenderSquare(Vector2 pointx, Vector2 pointy)
        {
            SetElevation(pointx, pointy);
            Tile[,] tiles = new Tile[(int)Mathf.Abs(pointx.x - pointy.x), (int)Mathf.Abs(pointx.y - pointy.y)];

            for (int x = (int)pointx.x; x < (int)pointy.x; x++)
            {
                for (int y = (int)pointx.y; y < (int)pointy.y; y++)
                {
                    tiles[x - (int)pointx.x, y - (int)pointx.y] = new Tile()
                    {
                        TileNumber = (int)islandTiles[x, y].Elevation
                    };
                }
            }

            GameState.Instance.MapRenderer.DispatchMap(tiles);
        }

        private void SetElevation(Vector2 pointx, Vector2 pointy)
        {
            Vector2 islandCenter = new Vector2((BottomLeft.x + TopRight.x) / 2, (BottomLeft.y + TopRight.y) / 2);
            bool isWater = false;
            Vector2 midPoint = new Vector2(0, 0);

            for (int x = (int)Mathf.Abs(pointx.x); x < (int)Mathf.Abs(pointy.x); x++)
            {
                for (int y = (int)Mathf.Abs(pointx.y); y < pointy.y; y++)
                {
                    isWater = false;

                    foreach(IslandPolygon poly in Polygons){
                        if(Polygon.IsPointInPolygon(new Vector2(x + BottomLeft.x, y + BottomLeft.y), poly.Poly.Points.ToArray()))
                        {
                            isWater = poly.IsWater;
                            midPoint = poly.Poly.MidPoint.Point;
                            break;
                        }
                    }

                    islandTiles[x, y] = new IslandTile()
                    {
                        Elevation = Mathf.Sqrt
                        (
                            Mathf.Pow(Mathf.Abs(islandCenter.x - (midPoint.x + BottomLeft.x)), 2) +
                            Mathf.Pow(Mathf.Abs(islandCenter.y - (midPoint.y + BottomLeft.y)), 2)
                        ),
                        IsWater = isWater 
                    };
                }
            }
        }

        public static Island VoronoiToIsland(Voronoi v, Vector2 bottomLeft, Vector2 topRight)
        {
            List<IslandPolygon> islandPoly = new List<IslandPolygon>();

            bool isWater = true;

            foreach (VoronoiEdge edge in v.Edges.OrderBy(x => x.Midpoint.Point.x).ThenBy(x => x.Midpoint.Point.y))
            {

                foreach (Polygon poly in v.Polygons.Values)
                {
                    isWater = true;
                    if (poly.MidPoint.Point == edge.Midpoint.Point)
                    {
                        if (Mathf.Abs(poly.PerlinNoise - Mathf.Abs(poly.MidPoint.Point.x) - Mathf.Abs(poly.MidPoint.Point.y)) < 25F)
                        {
                            isWater = false;
                            islandPoly.Add(new IslandPolygon(poly, isWater));
                            break;
                        }
                        else
                        {
                            islandPoly.Add(new IslandPolygon(poly, isWater));
                            break;
                        }
                    }
                }
            }

            return new Island(islandPoly, bottomLeft, topRight);
        }
    }
}