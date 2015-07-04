using UnityEngine;
using System.Collections;

public class GenerateBase : MonoBehaviour{
    public int width;
    public int height;
    [Range(0, 100)]
    public int randomFillPercent;
    public int[,] map;
    public string seed;
    [HideInInspector]
    public Point startPoint;
    [HideInInspector]
    public Point endPoint;

    void Start()
    {
        GenerateLevel();
        GameState.Instance.Map = this.map;
    }

    public virtual void GenerateLevel() { }
}

/// <summary>
/// Abstract class to enforce derived classes to implement the abstract function GenerateLevel
/// </summary>
public abstract class Generate : GenerateBase {
    public abstract override void GenerateLevel();
}
