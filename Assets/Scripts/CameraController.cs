using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(Input.GetKey(KeyCode.DownArrow)) {
            Vector2 transf = parent.transform.position;
            parent.transform.position = new(transf.x, transf.y - 1f, -1);
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            Vector2 transf = parent.transform.position;
            parent.transform.position = new(transf.x, transf.y + 1f, -1);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            Vector2 transf = parent.transform.position;
            parent.transform.position = new(transf.x - 1f, transf.y, -1);
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            Vector2 transf = parent.transform.position;
            parent.transform.position = new(transf.x + 1f, transf.y, -1);
        }
    }
}
