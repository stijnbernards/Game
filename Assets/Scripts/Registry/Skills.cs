using UnityEngine;
using System;
using System.Collections.Generic;

public class Skills
{
    public const string SKILL_TEST = "TEST_SKILL_ONE";

    public static void RegisterSkills()
    {
        GameObject testSkill = SkillItem.Default;
        testSkill.GetComponent<SkillItem>().SetAction(new Action(() => { Debug.Log("hey"); }));
        testSkill.transform.SetParent(GameObject.Find("SkillSlot1").transform);

        GameState.Instance.SkillRegistry.RegisterSkill(SKILL_TEST, testSkill);
    }
}