using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    private Vector3 pos;
    private bool moving = false;

    void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        CheckInput();
        if (moving)
        {
            transform.position = pos;
            moving = false;
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            pos += Vector3.right;
            moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            pos -= Vector3.right;
            moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            pos += Vector3.up;
            moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            pos -= Vector3.up;
            moving = true;
        }
    }
}