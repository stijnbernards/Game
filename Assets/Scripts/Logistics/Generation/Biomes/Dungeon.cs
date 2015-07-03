using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dungeon : BiomeBase{

    public int cells;

    private List<GameObject> ListCells = new List<GameObject>();

    /// <summary>
    /// Plot based render system for Taiga biome,
    /// Tree's 'n stuff
    /// </summary>
    /// <returns></returns>
    public override IEnumerator RenderPath()
    {
        //Cell creating loop
        for(int i = 0; i < cells; i++){
            yield return new WaitForSeconds(0.1f);
            CreateCell();
        }

        //Cell intersection loop.
    }

    public void CreateCell()
    {
        List<GameObject> cellGameObjs = new List<GameObject>();
        Vector2 startPosition = new Vector2(Random.Range(0, cells / 2), Random.Range(0, cells / 2));

        int cellHeight = Random.Range(4, 9);
        int cellWidth = Random.Range(4, 9);
        for (int i = 0; i < cellHeight; i++)
            for(int i2 = 0; i2 < cellWidth; i2++)
            {
                cellGameObjs.Add(base.SpawnBlock(new Vector3(startPosition.x + i, startPosition.y + i2, 0), groundSprite));
            }

        GameObject cell = cellGameObjs[0];
        foreach (GameObject obj in cellGameObjs)
        {
            obj.transform.parent = cell.transform;
        }
        ListCells.Add(cell);
    }
}
