using System.Collections.Generic;
using UnityEngine;

public class MapRenderer
{
    public Texture2D tileSheet;

    private int tileResolution = 16;

    private Grid map;

    public MapRenderer()
    {
        tileSheet = Resources.Load("download", typeof(Texture2D)) as Texture2D;

        map = new GameObject("map").AddComponent<Grid>();
        map.transform.position = new Vector3(-0.5F, -0.5F, 0);
        map.GetComponent<MeshRenderer>().materials = new Material[] { Resources.Load("SpriteSheet", typeof(Material)) as Material };
    }

    Color[][] ChopUpTiles()
    {
        int numTilesPerRow = tileSheet.width / tileResolution;
        int numRows = tileSheet.height / tileResolution;

        Color[][] tiles = new Color[numTilesPerRow * numRows][];

        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numTilesPerRow; x++)
            {
                tiles[y * numTilesPerRow + x] = tileSheet.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
            }
        }

        return tiles;
    }

    public void DispatchMap(Tile[,] tileMap)
    {
        map.XSize = tileMap.GetLength(0);
        map.YSize = tileMap.GetLength(1);
        map.Generate();
        
        int texWidth = map.XSize * tileResolution;
        int texHeight = map.YSize * tileResolution;
        
        Texture2D texture = new Texture2D(texWidth, texHeight);

        Color[][] tiles = ChopUpTiles();

        if (tileMap != null)
        {
            for (int x = 0; x < tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < tileMap.GetLength(1); y++)
                {
                    Color[] p;

                    if (tileMap[x, y].TileNumber < tiles.Length - 1)
                    {
                        p = tiles[tileMap[x, y].TileNumber + 1];
                    }
                    else
                    {
                        int tNumber = (tileMap[x, y].TileNumber + 1) / tiles.Length;
                        p = tiles[tNumber];
                    }

                    texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
                }
            }

            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();

            Material tileTexture = new Material(Shader.Find("Unlit/Texture"));
            tileTexture.mainTexture = texture;

            map.GetComponent<MeshRenderer>().materials = new Material[] { tileTexture };
        }
    }
}

