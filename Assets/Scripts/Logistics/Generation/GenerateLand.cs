using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateLand : MonoBehaviour {

    public GameObject groundSprite;
    public int width;
    public int height;

    private ArrayList GroundBlocksArray = new ArrayList();
    private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

	void Start () {
        List<Vector2> PointList = new List<Vector2>();

        GroundBlocksArray.Add(gameObject);
        Vector2 SpawnPoint = transform.position;
        Vector2 EndPoint = GenerateEnd();
        PointList.Add(SpawnPoint);
        PointList.Add(EndPoint);

        SpawnBlock(EndPoint);
        BuildRoad(PointList);
	}

    void BuildRoad(List<Vector2> points)
    {
        for (int iter = 0; iter < points.Count - 1; iter++)
        {
            int x = (int)points[iter].x;
            int y = (int)points[iter].y;
            int x2 = (int)points[iter + 1].x;
            int y2 = (int)points[iter + 1].y;

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Mathf.Abs(w);
            int shortest = Mathf.Abs(h);
            if (!(longest > shortest))
            {
                longest = Mathf.Abs(h);
                shortest = Mathf.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;

            for (int i = 0; i <= longest; i++)
            {
                SpawnBlock(new Vector2(x, y));
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }
    }

    Vector2 GenerateEnd(){
        float X;
        float Y;
        int rand = Random.Range(1, 3);

        X = (rand == 1) ? Random.Range(1, width + 1) : width;
        Y = (rand == 2) ? Random.Range(1, height + 1) : height;
        return new Vector2(X, Y);
    }

    public bool SpawnBlock(Vector2 OffsetLocation)
    {
        GameObject groundTile = GameObject.Instantiate(groundSprite, OffsetLocation, Quaternion.identity) as GameObject;
        GroundBlocksArray.Add(groundTile);
        groundTile.transform.position = OffsetLocation;
        groundTile.transform.parent = transform.FindChild("Ground");
        return true;
    }
}