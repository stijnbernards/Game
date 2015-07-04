using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CreateCharacter : MonoBehaviour {

    public void StartCreateCharacter()
    {
        DontDestroyOnLoad(GameState.Instance);
        GameState.Instance.StartState();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(30, 30, 150, 30), "Start Game"))
        {
            StartCreateCharacter();
        }
    }

}
