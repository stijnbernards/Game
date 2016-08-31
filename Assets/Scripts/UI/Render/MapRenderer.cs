using System.Collections.Generic;
using UnityEngine;

public class MapRenderer
{
    public Texture2D tileSheetTiles;
    public Texture2D tileSheetShadows;

    private int tileResolution = 16;

    private Grid map;
    private Grid shadows;

    public MapRenderer()
    {
        tileSheetTiles = Resources.Load("download", typeof(Texture2D)) as Texture2D;
        tileSheetShadows = Resources.Load("Shadows", typeof(Texture2D)) as Texture2D;

        map = new GameObject("map").AddComponent<Grid>();
        map.transform.position = new Vector3(-0.5F, -0.5F, 0);
        map.GetComponent<MeshRenderer>().materials = new Material[] { Resources.Load("SpriteSheet", typeof(Material)) as Material };

        shadows = new GameObject("shadows").AddComponent<Grid>();
        shadows.gameObject.layer = LayerMask.NameToLayer("Shadows");
        shadows.transform.position = new Vector3(-0.5F, -0.5F, -0.5F);
        shadows.GetComponent<MeshRenderer>().materials = new Material[] { Resources.Load("ShadowsMaterial", typeof(Material)) as Material };
    }

    Color[][] ChopUpTiles(Texture2D tileSheet)
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

    /// <summary>
    /// Should change the shadows size to resizeable....
    /// </summary>
    /// <param name="losTiles"></param>
    public void DispatchShadows(List<Vector2> losTiles)
    {
        int texWidth = shadows.XSize * tileResolution;
        int texHeight = shadows.YSize * tileResolution;
        
        Texture2D texture = new Texture2D(texWidth, texHeight);

        Color[][] tiles = ChopUpTiles(tileSheetShadows);

        for (int x = (int)(GameState.Instance.Character.Behaviour.transform.position.x - 14); x < (int)(GameState.Instance.Character.Behaviour.transform.position.x + 14); x++)
        {
            for (int y = (int)(GameState.Instance.Character.Behaviour.transform.position.y - 10); y < (int)(GameState.Instance.Character.Behaviour.transform.position.y + 10); y++)
            {
                Color[] p;

                if (GameState.Instance.Map.GetTileSafe(x, y).Seen == false)
                {
                    p = tiles[1];
                }
                else
                {
                    p = tiles[0];
                }

                if (losTiles.Contains(new Vector2(x, y)))
                {
                    p = tiles[2];
                }

                texture.SetPixels((x - (int)(GameState.Instance.Character.Behaviour.transform.position.x - 14)) * tileResolution, (y - (int)(GameState.Instance.Character.Behaviour.transform.position.y - 10)) * tileResolution, tileResolution, tileResolution, p);
            }
        }

        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        Material tileTexture = new Material(Shader.Find("Standard"));

        tileTexture.mainTexture = texture;
        tileTexture.SetFloat("_Mode", 2.0F);
        tileTexture.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        tileTexture.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        tileTexture.SetInt("_ZWrite", 0);
        tileTexture.DisableKeyword("_ALPHATEST_ON");
        tileTexture.EnableKeyword("_ALPHABLEND_ON");
        tileTexture.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        tileTexture.renderQueue = 3000;

        shadows.GetComponent<MeshRenderer>().materials = new Material[] { tileTexture };

        shadows.transform.position = new Vector2(GameState.Instance.Character.Behaviour.transform.position.x - 14.5F, GameState.Instance.Character.Behaviour.transform.position.y - 10.5F);
    }

    public void DispatchMap(Tile[,] tileMap)
    {
        map.XSize = tileMap.GetLength(0);
        map.YSize = tileMap.GetLength(1);
        map.Generate();

        shadows.XSize = 28;
        shadows.YSize = 20;
        shadows.Generate();
        
        int texWidth = map.XSize * tileResolution;
        int texHeight = map.YSize * tileResolution;
        
        Texture2D texture = new Texture2D(texWidth, texHeight);

        Color[][] tiles = ChopUpTiles(tileSheetTiles);

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

