using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMain {

    private static GameObject gameLogText = Resources.Load("Prefabs/GameLogText") as GameObject;
    private static GameObject content = GameObject.Find("GLContent");
    private static GameObject popup = Resources.Load("Prefabs/Popup") as GameObject;
    private static GameObject popupsPanel = GameObject.Find("Popups");

    
    //TODO:: Replace find!!
    public static void SetHealth(){
        GameObject.Find("Health").GetComponent<Text>().text = "Health: " + GameState.Instance.Character.Health.ToString("N0") + " / " + GameState.Instance.Character.MaxHealth.ToString("N0");
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

    public static void SetChat(string text)
    {
        //Game
        GameLog(text);
    }

    public static void Popup(string text, Action action = null)
    {
        GameObject dialog = GameObject.Instantiate(popup);

        dialog.transform.SetParent(popupsPanel.transform, false);
        dialog.GetComponent<Text>().text = text;

        GameState.Instance.Character.Behaviour.AddKeyBind(KeyCode.Space, new CharacterBehaviour.KeydownAction(() =>
            {
                GameState.Instance.DestroyObject(dialog);

                GameState.Instance.Character.Behaviour.RemoveKeyBind(KeyCode.Space);

                if (action != null) 
                {
                    action.Invoke();
                }
            }));
    }

    public static void YesNoDialog(string text, Action action = null)
    {
        GameObject dialog = GameObject.Instantiate(popup);

        dialog.transform.SetParent(popupsPanel.transform, false);
        dialog.GetComponent<Text>().text = text;

        GameState.Instance.Character.Behaviour.AddKeyBind(KeyCode.Return, new CharacterBehaviour.KeydownAction(() =>
        {
            GameState.Instance.DestroyObject(dialog);

            GameState.Instance.Character.Behaviour.RemoveKeyBind(KeyCode.Return);
            GameState.Instance.Character.Behaviour.RemoveKeyBind(KeyCode.Escape);

            if (action != null)
            {
                action.Invoke();
            }
        }));

        GameState.Instance.Character.Behaviour.AddKeyBind(KeyCode.Escape, new CharacterBehaviour.KeydownAction(() =>
        {
            GameState.Instance.DestroyObject(dialog);

            GameState.Instance.Character.Behaviour.RemoveKeyBind(KeyCode.Escape);
            GameState.Instance.Character.Behaviour.RemoveKeyBind(KeyCode.Return);
        }));
    }

    //TODO::Add formatting player did blah etc...
    public static void GameLog(string text)
    {
        GameObject logText = GameObject.Instantiate(gameLogText);
        logText.transform.SetParent(content.transform, false);
        logText.GetComponent<Text>().text = text;
    }
}
