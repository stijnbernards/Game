using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    private GameObject node
    {
        get
        {
            return Resources.Load("Prefabs/UI/SkillTree/Node") as GameObject;
        }
    }

    public void FirstSkills()
    {
        GameObject startNode;
        float iter = 1;

        foreach (KeyValuePair<string, Category> cat in GameState.Instance.CategoryRegistry)
        {
            startNode = Instantiate(node);
            startNode.transform.SetParent(gameObject.transform);
            startNode.transform.localPosition = new Vector2(100 * iter, 0);

            CreateSkillNode(startNode, cat.Value); 
            
            iter++;
        }
    }

    private void CreateSkillNode(GameObject parent, Category cat)
    {
        GameObject skillNode = Instantiate(node);
        skillNode.transform.SetParent(parent.transform);
        skillNode.transform.localPosition = new Vector2(0, -100);
        skillNode.AddComponent<Skill>().GenerateSkill(cat.Name);
    }
    
}