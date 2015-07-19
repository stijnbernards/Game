using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMain {

    public static void SetHealth(){
        GameObject.Find("Health").GetComponent<Text>().text = "Health: " + GameState.Instance.Character.Health.ToString("N0");
    }

    public static void SetLevel()
    {
        GameObject.Find("Level").GetComponent<Text>().text = "Level: " + GameState.Instance.Level;
    }

    public static void SetExp()
    {
        GameObject.Find("Exp").GetComponent<Text>().text = "Experience: " + Mathf.Round(GameState.Instance.Character.Exp);
    }

    public static void SetCharLevel()
    {
        GameObject.Find("CharacterLevel").GetComponent<Text>().text = "Level: " + GameState.Instance.Character.Level;
    }
}
