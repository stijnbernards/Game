using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {

    public GameObject target;

    Vector3 offset;

	void Start () {
        offset = new Vector3(0f, 0f, -10f);
        transform.position = target.transform.position + offset;
	}
	
	void LateUpdate () {
        transform.position = target.transform.position + offset;
	}
}
