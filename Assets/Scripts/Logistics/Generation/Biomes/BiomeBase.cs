using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiomeBase : MonoBehaviour {

    public GameObject groundSprite;
    public List<GameObject> decals;
    public int height;
    public int width;

    private Vector2 SpawnPoint;

    public void Start()
    {
        SpawnPoint = transform.position;

        this.BuildLandPlot();
        StartCoroutine(RenderRoad());
    }

    public virtual void BuildLandPlot()
    {
        for(int i = 0; i < height; i++)
            for(int i2 = 0; i2 < width; i2++)
                SpawnBlock(new Vector2(i, i2));
    }

    public virtual IEnumerator RenderDecal()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

        }
    }

    public virtual IEnumerator RenderRoad()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
        }
    }

    public bool SpawnBlock(Vector2 OffsetLocation)
    {
        GameObject groundTile = GameObject.Instantiate(groundSprite, OffsetLocation, Quaternion.identity) as GameObject;
        groundTile.transform.position = OffsetLocation;
        groundTile.transform.parent = transform.FindChild("Ground");
        return true;
    }
}
