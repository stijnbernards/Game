using UnityEngine;
using System.Collections;

public class Caves : Generate{

    public GameObject groundSprite;
    public GameObject wallSprite;
    public int smoothing;

    //Speaks for itself...
    public override void GenerateLevel()
    {
        while (!Generate());
        BuildLevel();
        
    }

    /// <summary>
    /// Generates the level
    /// </summary>
    /// <returns>succeeded flooding percentage check</returns>
    bool Generate()
    {
        map = new int[width, height];
        RandomFillLevel();
        for (int i = 0; i < smoothing; i++)
        {
            SmoothLevel();
        }

        return FloodFillCheck();
    }

    /// <summary>
    /// Randomly fills grid with wall or empty
    /// </summary>
    void RandomFillLevel()
    {
        seed = System.Guid.NewGuid().ToString();

        //rng based on a random generated GUID hash thing
        System.Random rng = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    map[x, y] = 1;
                else
                    map[x, y] = (rng.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    /// <summary>
    /// Smoothes the walls based on nearly walls + ground n stuff
    /// </summary>
    void SmoothLevel()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = WallPlaceLogic(x, y);
            }
        }
    }

    /// <summary>
    /// Checks if tile should be set to wall or ground
    /// </summary>
    /// <param name="x">x coord</param>
    /// <param name="y">y coord</param>
    /// <returns>wall or ground</returns>
    int WallPlaceLogic(int x, int y)
    {
        int nWallTiles = GetSurroundingWalls(x, y);
        if (map[x, y] == 1)
        {
            if (nWallTiles >= 4)
                return 1;
            else if(nWallTiles < 2)
                return 0;
        }
        else{
            if(nWallTiles >= 5)
                return 1;
        }
        return 0;
    }

    /// <summary>
    /// Gets the surrounding walls of a tile
    /// </summary>
    /// <param name="x">x coord</param>
    /// <param name="y">y coord</param>
    /// <returns>Number of surrrounding walls</returns>
    int GetSurroundingWalls(int x, int y)
    {
        int wallCount = 0;
        for (int nX = x - 1; nX <= x + 1; nX++)
        {
            for (int nY = y - 1; nY <= y + 1; nY++)
            {
                if (nX >= 0 && nX < width && nY >= 0 && nY < height)
                {
                    if (nX != x || nY != y)
                        wallCount += map[nX, nY];
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    /// <summary>
    /// Chooses a random spot on the map to fill with walls,
    /// if filled spot is greater than 40% this function returns true and accepts the map.
    /// Also fills all dead spots with walls
    /// </summary>
    /// <returns>Boolean map ok or no ok</returns>
    bool FloodFillCheck()
    {
        bool generated = true;
        int startX = 0;
        int startY = 0;
        int percentage = 0;

        while(generated){
            startX = Random.Range(0, width);
            startY = Random.Range(0, height);
            if(map[startX, startY] != 1)
                generated = false;
        }

        CheckTiles(new Vector2(startX, startY));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0)
                    map[x, y] = 1;
                else if (map[x, y] == 3)
                {
                    map[x, y] = 0;
                    percentage++;
                }
            }
        }

        percentage = percentage / ((width * height) / 100);

        if (percentage > 40)
            return true;

        return false;
    }

    /// <summary>
    /// Checks neighbouring tiles
    /// </summary>
    /// <param name="pos">The position</param>
    void CheckTiles(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        if (map[x, y] == 3)
            return;
        else if (map[x, y] == 1)
            return;
        else
            map[x, y] = 3;

        CheckTiles(new Vector2(x + 1, y));
        CheckTiles(new Vector2(x, y + 1));
        CheckTiles(new Vector2(x - 1, y));
        CheckTiles(new Vector2(x, y - 1));
    }

    /// <summary>
    /// Visualizes the int[,] map
    /// </summary>
    void BuildLevel()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 offset = new Vector3(x, y, 1f);
                    GameObject tile;

                    if (map[x, y] == 1)
                        tile = GameObject.Instantiate(wallSprite, offset, Quaternion.identity) as GameObject;
                    else
                        tile = GameObject.Instantiate(groundSprite, offset, Quaternion.identity) as GameObject;

                    tile.transform.position = offset;
                }
            }
        }
    }
}
