using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

[Obsolete]
public class SkillMenu : MonoBehaviour {

    Text SkillPoints, Strength, Constitution, Dexterity;
    Button BtnStr, BtnConst, BtnDex;

    void Start()
    {
        SkillPoints = GameObject.Find("SkillPoints").GetComponent<Text>();
        Strength = GameObject.Find("StrTxt").GetComponent<Text>();
        Constitution = GameObject.Find("ConstTxt").GetComponent<Text>();
        Dexterity = GameObject.Find("DexTxt").GetComponent<Text>();
        BtnStr = GameObject.Find("Str").GetComponent<Button>();
        BtnConst = GameObject.Find("Const").GetComponent<Button>();
        BtnDex = GameObject.Find("Dex").GetComponent<Button>();

        BtnStr.onClick.AddListener(() => { GameState.Instance.Character.Strength++; UpdateGUI(); });
        BtnConst.onClick.AddListener(() => { GameState.Instance.Character.Constitution++; UpdateGUI(); });
        BtnDex.onClick.AddListener(() => { GameState.Instance.Character.Dexterity++; UpdateGUI(); });

        if (GameState.Instance.Character.SkillPoints <= 0)
        {
            DisableAll();
        }

        UpdateGUI();
    }

    void UpdateGUI()
    {
        Strength.text = GameState.Instance.Character.Strength.ToString();
        Constitution.text = GameState.Instance.Character.Constitution.ToString();
        Dexterity.text = GameState.Instance.Character.Dexterity.ToString();
        SkillPoints.text = GameState.Instance.Character.SkillPoints.ToString() + " Skillpoint(s) left.";

        if (GameState.Instance.Character.SkillPoints > 0)
            EnableAll();
        else
            DisableAll();
    }

    void DisableAll()
    {
        BtnStr.interactable = false;
        BtnConst.interactable = false;
        BtnDex.interactable = false;
    }

    void EnableAll()
    {
        BtnStr.interactable = true;
        BtnConst.interactable = true;
        BtnDex.interactable = true;
    }
}
