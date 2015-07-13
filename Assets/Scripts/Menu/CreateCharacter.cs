using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CreateCharacter : MonoBehaviour {

    public void StartCreateCharacter()
    {
        Application.LoadLevel("Game");
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(30, 30, 150, 30), "Start Game"))
        {
            StartCreateCharacter();
        }
    }

}
