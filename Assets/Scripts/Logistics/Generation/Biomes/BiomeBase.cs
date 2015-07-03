using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiomeBase : MonoBehaviour {

    public GameObject groundSprite;
    public GameObject fillSprite;
    public List<GameObject> decals;
    public int height;
    public int width;

    private Vector2 SpawnPoint;

    public void Start()
    {
        SpawnPoint = transform.position;

        StartCoroutine(RenderPath());
    }

    public virtual void BuildLandPlot()
    {
        for(int i = 0; i < height; i++)
            for(int i2 = 0; i2 < width; i2++)
                SpawnBlock(new Vector3(i, i2, 0), groundSprite);
    }

    public virtual void FillLand()
    {
        for (int i = 0; i < height; i++)
            for (int i2 = 0; i2 < width; i2++)
                SpawnBlock(new Vector3(i, i2, 1), fillSprite);
    }

    public virtual IEnumerator RenderDecal()
    {
        return null;
    }

    public virtual IEnumerator RenderPath()
    {
        return null;
    }

    public GameObject SpawnBlock(Vector3 OffsetLocation, GameObject sprite)
    {
        GameObject groundTile = GameObject.Instantiate(sprite, OffsetLocation, Quaternion.identity) as GameObject;
        groundTile.transform.position = OffsetLocation;
        groundTile.transform.parent = transform.FindChild("Ground");

        return groundTile;
    }
}
