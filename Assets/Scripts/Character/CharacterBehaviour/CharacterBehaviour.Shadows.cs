using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterBehaviour : MonoBehaviour
{
    #region ShadowCasting

    public void CheckTilesVisible()
    {
        float los = GameState.Instance.Character.LOS;

        List<Vector2[]> lines1 = new List<Vector2[]>();
        List<Vector2[]> lines2 = new List<Vector2[]>();

        List<Vector2> linesFinal = new List<Vector2>();
        List<Vector2> lineWithCollision = new List<Vector2>();

        Vector2[] orderedLine;
        List<int> obstacles = GameState.Instance.Map.Obstacles;
        bool collision = false;

        lines1.AddRange(CheckSideVisible(1, los));
        lines1.AddRange(CheckSideVisible(2, los));
        lines1.AddRange(CheckSideVisible(3, los));
        lines1.AddRange(CheckSideVisible(4, los));
        lines2.AddRange(CheckSideVisible(5, los));
        lines2.AddRange(CheckSideVisible(6, los));
        lines2.AddRange(CheckSideVisible(7, los));
        lines2.AddRange(CheckSideVisible(8, los));

        foreach (Vector2[] line in lines1)
        {
            lineWithCollision = new List<Vector2>();
            collision = false;

            orderedLine = line.OrderBy(x => x.x).ThenBy(x => x.y).ToArray<Vector2>();

            foreach (Vector2 v in orderedLine)
            {
                if (obstacles.Contains(GameState.Instance.Map.GetTileSafe(v.x, v.y).TileNumber))
                {
                    break;
                }

                GameState.Instance.Map.GetTileSafe(v.x, v.y).Seen = true;

                if (!collision)
                {
                    lineWithCollision.Add(v);
                }
            }

            linesFinal.AddRange(lineWithCollision.ToArray());
        }

        foreach (Vector2[] line in lines2)
        {
            lineWithCollision = new List<Vector2>();
            collision = false;

            orderedLine = line.OrderByDescending(x => x.x).ThenByDescending(x => x.y).ToArray<Vector2>();

            foreach (Vector2 v in orderedLine)
            {
                if (obstacles.Contains(GameState.Instance.Map.GetTileSafe(v.x, v.y).TileNumber))
                {
                    break;
                }

                GameState.Instance.Map.GetTileSafe(v.x, v.y).Seen = true;

                if (!collision)
                {
                    lineWithCollision.Add(v);
                }
            }

            linesFinal.AddRange(lineWithCollision.ToArray());
        }

        GameState.Instance.MapRenderer.DispatchShadows(linesFinal);
    }

    public List<Vector2[]> CheckSideVisible(int side, float los)
    {
        List<Vector2[]> lines = new List<Vector2[]>();

        switch (side)
        {
            case 1:
                for (int i = 0; i <= los; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x + i,
                        GameState.Instance.Character.Behaviour.transform.position.y + los,
                        false
                    ));
                }
                break;
            case 2:
                for (int i = 0; i <= los; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x + i,
                        GameState.Instance.Character.Behaviour.transform.position.y - los,
                        false
                    ));
                }
                break;
            case 3:
                for (int i = 0; i <= los; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x + los,
                        GameState.Instance.Character.Behaviour.transform.position.y + i,
                        false
                    ));
                }
                break;
            case 4:
                for (int i = 0; i <= los; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x + los,
                        GameState.Instance.Character.Behaviour.transform.position.y - i,
                        false
                    ));
                }
                break;
            case 5:
                for (int i = 0; i <= los; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x - i,
                        GameState.Instance.Character.Behaviour.transform.position.y - los,
                        false
                    ));
                }
                break;
            case 6:
                for (int i = 0; i <= los; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x - i,
                        GameState.Instance.Character.Behaviour.transform.position.y + los,
                        false
                    ));
                }
                break;
            case 7:
                for (int i = 0; i <= los; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x - los,
                        GameState.Instance.Character.Behaviour.transform.position.y + i,
                        false
                    ));
                }
                break;
            case 8:
                for (int i = 0; i <= los; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x - los,
                        GameState.Instance.Character.Behaviour.transform.position.y - i,
                        false
                    ));
                }
                break;
        }

        return lines;
    }

    #endregion
}