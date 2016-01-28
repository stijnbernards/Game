using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Forest : Generate
{
    public GameObject groundSprite = Resources.Load("Ground") as GameObject;

    public override void GenerateLevel()
    {
        //List<Vector2> v = new List<Vector2>()
        //{
        //    new Vector2(5,10),
        //    new Vector2(15,20),
        //    new Vector2(22,35),
        //    new Vector2(13,77),
        //    new Vector2(20,69),
        //    new Vector2(16,43),
        //    new Vector2(38,56),
        //};

        List<Vector2> v = new List<Vector2>()
        {
            new Vector2(10,5),
            new Vector2(5, 15),
            new Vector2(15, 15),
            new Vector2(10,25),
            new Vector2(5, 35),
            new Vector2(15, 35),
            new Vector2(20,25),
            new Vector2(30,5),
            new Vector2(25, 15),
            new Vector2(35, 15),
            new Vector2(30,25),
            new Vector2(25, 35),
            new Vector2(35, 35),
            new Vector2(30, 45),
        };

        //for (int x = 0; x < 50; x++)
        //{
        //    v.Add(new Vector2(Random.Range(0, 50), Random.Range(0, 90)));
        //    GameObject.Instantiate(groundSprite, v[x], Quaternion.identity);
        //}

        v.ForEach(x =>
        {
            GameObject.Instantiate(groundSprite, x, Quaternion.identity);
        });

        GameObject.Find("LevelStarter").AddComponent<Fortunes>();
        GameObject.Find("LevelStarter").GetComponent<Fortunes>().FortunesStart(v.ToArray());
    }

}

//10,0,5,10,15,10,10,20,5,30,15,30,20,20,30,0,25,10,35,10,30,20,25,30
