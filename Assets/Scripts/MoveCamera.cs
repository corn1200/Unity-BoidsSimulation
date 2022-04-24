using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, Input.GetAxis("Mouse X") * speed, 0f, Space.World);
        transform.Rotate(-Input.GetAxis("Mouse Y") * speed, 0f, 0f);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right;
        }
    }
}
