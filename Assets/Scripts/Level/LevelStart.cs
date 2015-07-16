using UnityEngine;
using System.Collections;

public class LevelStart : MonoBehaviour {

	void Start () 
    {
        GameState.Instance.StartState();
        new Caves().StartGen(1);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
