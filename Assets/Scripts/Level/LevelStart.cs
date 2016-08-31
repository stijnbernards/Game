using UnityEngine;
using System.Collections;

public class LevelStart : MonoBehaviour {

	void Start () 
    {
        GameState.Instance.StartState();
        //new Caves().StartGen(1);
        //new CaveRooms().StartGen(1);
        //new MainIsland().StartGen(1);
        //GameState.Instance.GetLevel<DebugLevel>(1, 1, "DEBUG_LEVEL", false);
        GameState.Instance.GetLevel<Dungeon>(1, 1, "DUNGEON", false);
    }
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
