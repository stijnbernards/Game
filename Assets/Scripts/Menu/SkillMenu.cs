using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class SkillMenu : MonoBehaviour {

    Text SkillPoints;
    Text Strength;
    Button BtnStr;

    void Start()
    {
        SkillPoints = GameObject.Find("SkillPoints").GetComponent<Text>();
        Strength = GameObject.Find("StrTxt").GetComponent<Text>();
        BtnStr = GameObject.Find("Str").GetComponent<Button>();

        BtnStr.onClick.AddListener(() => { GameState.Instance.Character.Strength++; UpdateGUI(); });

        if (GameState.Instance.Character.SkillPoints <= 0)
        {
            DisableAll();
        }

        UpdateGUI();
    }

    void UpdateGUI()
    {
        Strength.text = GameState.Instance.Character.Strength.ToString();
        SkillPoints.text = GameState.Instance.Character.SkillPoints.ToString() + " Skillpoint(s) left.";

        if (GameState.Instance.Character.SkillPoints > 0)
            EnableAll();
        else
            DisableAll();
    }

    void DisableAll()
    {
        BtnStr.interactable = false;
    }

    void EnableAll()
    {
        BtnStr.interactable = true;
    }
}
