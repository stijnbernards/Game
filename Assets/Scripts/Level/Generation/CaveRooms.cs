using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CaveRooms : Generate
{
    public GameObject groundSprite = Resources.Load("Ground") as GameObject;
    public GameObject wallSprite = Resources.Load("Stone") as GameObject;

    private List<Vector2[]> rooms = new List<Vector2[]>();

    public override void BeginPoint()
    {
        int x, y;

        if (this.Level != 0)
        {
            FindRandomEmpty(out x, out y);
            map[x, y].TileNumber = 8;
            map[x, y].Name = "starttile";
            map[x, y].Action = new Tile.TileAction(() =>
            {
                GameState.Instance.Character.Behaviour.SetMoving(false);
                GameState.Instance.GetLevel<Caves>(Hardness, (int)Level - 1, this.ID, true);
            });
            this.startPoint = new Point(x, y);
        }
        else
        {
            FindRandomEmpty(out x, out y);

            map[x, y].TileNumber = 8;
            map[x, y].Name = "starttile";
            map[x, y].Action = new Tile.TileAction(() =>
            {
                GameState.Instance.Character.Behaviour.SetMoving(false);
                GameState.Instance.GetLevel<Caves>(Hardness, 0, "DEBUG_LEVEL", true);
            });

            this.startPoint = new Point(x, y);
        }
    }

    public override void EndPoint()
    {
        int x, y;

        if (this.Level + 1 < GameState.Instance.LevelRegistry.LevelCount(this.ID))
        {
            FindRandomEmpty(out x, out y);

            map[x, y].Action = new Tile.TileAction(() =>
            {
                GameState.Instance.Character.Behaviour.SetMoving(false);
                GameState.Instance.GetLevel<Caves>(Hardness, (int)Level + 1, this.ID, false);
            });

            map[x, y].TileNumber = 9;
            map[x, y].Name = "endtile";

            this.endPoint = new Point(x, y);
        }
    }

    public override void GenerateLevel()
    {
        int randX,
            randY,
            iter1 = 0,
            iter2 = 0;

        map = new Tile[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = new Tile();

        for (int x = 0; x < Mathf.RoundToInt(Random.Range(9, 9)); x++)
        {
            rooms.Add(GenRoom(width / 4));
        }

        rooms.ForEach(x => {
            randX = Mathf.FloorToInt(iter1 / 3) % 2 > 0 ? width - (iter1 % 3 * (width / 3) + (width / 3)) : iter1 % 3 * (width / 3);
            randY = Mathf.FloorToInt(iter1 / 3) * (width / 3); 
            x.ToList()
                .ForEach(y => { 
                    rooms[iter1][iter2] = new Vector2(y.x + randX, y.y + randY); 
                    iter2++; 
                });
            iter2 = 0;
            iter1++; 
        });

        rooms.Add(BuildRoads());
        rooms.ForEach(x =>
        {
            x.ToList().ForEach(y =>
            {
                try
                {
                    map[(int)y.x, (int)y.y].TileNumber = 0;
                }
                catch (System.Exception e) { };
            });
        });
    }

    /// <summary>
    /// Generates a room with variable walls.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="complexity"></param>
    /// <returns></returns>
    private Vector2[] GenRoom(int size, int complexity = 8)
    {
        Vector2 placeholder;
        List<Vector2> returnPoints = new List<Vector2>();
        List<Vector2> points = new List<Vector2>();
        List<Vector2> points1 = new List<Vector2>();
        List<Vector2> points2 = new List<Vector2>();
        List<Vector2> points3 = new List<Vector2>();
        List<Vector2> points4 = new List<Vector2>();

        int iter = 0;
        int calc = Mathf.RoundToInt(size / 5);

        while (iter < complexity)
        {
            if (iter < complexity / 4)
            {
                placeholder = GenHelpers.GenerateBetween(new Vector2(calc, 0), new Vector2(calc * 4, calc));
                points1.Add(placeholder);
            }
            else if (iter < complexity / 2)
            {
                placeholder = GenHelpers.GenerateBetween(new Vector2(calc * 4, calc), new Vector2(calc * 5, calc * 4));
                points2.Add(placeholder);
            }
            else if (iter < complexity / 1.33333333333333333333333333)
            {
                placeholder = GenHelpers.GenerateBetween(new Vector2(calc, calc * 4), new Vector2(calc * 4, calc * 5));
                points3.Add(placeholder);
            }
            else
            {
                placeholder = GenHelpers.GenerateBetween(new Vector2(0, calc), new Vector2(calc, calc * 4));
                points4.Add(placeholder);
            }

            iter++;
        }

        points.AddRange(points1.OrderBy(x => x.x));
        points.AddRange(points2.OrderBy(y => y.y));
        points.AddRange(points3.OrderByDescending(x => x.x));
        points.AddRange(points4.OrderByDescending(x => x.y));

        for (int x = 0; x < points.Count - 1; x++)
        {
            GenHelpers.PlotLine(points[x].x, points[x].y, points[x + 1].x, points[x + 1].y).ToList<Vector2>().ForEach(y => { returnPoints.Add(y); });
        }

        GenHelpers.PlotLine(points[points.Count - 1].x, points[points.Count - 1].y, points[0].x, points[0].y).ToList<Vector2>().ForEach(y => { returnPoints.Add(y); });


        FloodFillRoom(ref returnPoints, width / 4);

        return returnPoints.ToArray();
    }

    /// <summary>
    /// function to insert a value at a point with rules.
    /// </summary>
    /// <param name="list">the list to check</param>
    /// <param name="value">true x or false y</param>
    /// <returns></returns>
    private List<Vector2> InsertList(List<Vector2> list, Vector2 insert, bool value, bool reverse)
    {
        list.Add(insert);

        if (reverse)
            if(value)
                list = list.OrderBy(x => x.x).ToList();
            else
                list = list.OrderBy(x => x.y).ToList();
        else
            if (value)
                list = list.OrderByDescending(x => x.x).ToList();
            else
                list = list.OrderByDescending(x => x.y).ToList();

        return list;
    }

    /// <summary>
    /// Builds static roads between the rooms
    /// </summary>
    /// <returns></returns>
    private Vector2[] BuildRoads()
    {
        int random;

        List<Vector2> roads = new List<Vector2>();

        List<int> roomsDone = new List<int>()
        {
            0
        };

        for (int x = 0; x < rooms.Count - 1; x++)
        {
            //random = Mathf.RoundToInt(Random.Range(0, rooms.Count - 1));

            random = x + 1;

            //while(roomsDone.Contains(random))
                //random = Mathf.RoundToInt(Random.Range(0, rooms.Count - 1));
            try
            {
                roads.AddRange(
                    BuildRoadBetween(
                        rooms[roomsDone.Last()][Mathf.RoundToInt(Random.Range(0, rooms[roomsDone.Last()].Length - 1))],
                        rooms[random][Mathf.RoundToInt(Random.Range(0, rooms[roomsDone.Last()].Length - 1))]
                    )
                );

                roomsDone.Add(random);
            }
            catch (System.Exception e)
            {

            }
        }

        return roads.ToArray();
    }

    private Vector2[] BuildRoadBetween(Vector2 p0, Vector2 p1)
    {
        List<Vector2> road = new List<Vector2>();

        GenHelpers.PlotLine(p0.x, p0.y, p1.x, p1.y).ToList<Vector2>().ForEach(y => { road.Add(y); });

        return road.ToArray();
    }

    public override void BuildLevel()
    {
        //GameObject parent = new GameObject("Map");
        //GameObject tile = null;
        //Vector3 offset = new Vector3();

        //if (map != null)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            offset = new Vector3(x, y, 0f);

        //            if (map[x, y].TileNumber == 1)
        //                tile = GameObject.Instantiate(wallSprite, offset, Quaternion.identity) as GameObject;
        //            else if (map[x, y].TileNumber == 0)
        //                tile = GameObject.Instantiate(groundSprite, offset, Quaternion.identity) as GameObject;
        //            else if (map[x, y].TileNumber == 2)
        //                tile = GameObject.Instantiate(startSprite, offset, Quaternion.identity) as GameObject;
        //            else
        //                tile = GameObject.Instantiate(endSprite, offset, Quaternion.identity) as GameObject;

        //            tile.transform.position = offset;
        //            tile.transform.parent = parent.transform;
        //        }
        //    }
        //}

        GameState.Instance.MapRenderer.DispatchMap(map);
    }

    private void FloodFillRoom(ref List<Vector2> roomBounds, int height)
    {
        int startpoint = Mathf.FloorToInt(height / 2);

        FloodFill(ref roomBounds, new Vector2(startpoint, startpoint));
    }

    private void FloodFill(ref List<Vector2> roomBounds, Vector2 pos)
    {
        if (!roomBounds.Contains(pos))
        {
            roomBounds.Add(pos);
            FloodFill(ref roomBounds, new Vector2(pos.x + 1F, pos.y));
            FloodFill(ref roomBounds, new Vector2(pos.x - 1F, pos.y));
            FloodFill(ref roomBounds, new Vector2(pos.x, pos.y + 1F));
            FloodFill(ref roomBounds, new Vector2(pos.x, pos.y - 1F));
        }
    }

}
