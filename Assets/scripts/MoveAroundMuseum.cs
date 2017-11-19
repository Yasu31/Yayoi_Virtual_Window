using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundMuseum : MonoBehaviour {
    private Vector3 origin;
    private float speed = 0.5f;
	// Use this for initialization
	void Start () {
        origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //矢印キー
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //origin += this.transform.forward;
            transform.Translate(this.transform.forward * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(-1*this.transform.forward * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(this.transform.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-1*this.transform.right * speed * Time.deltaTime);
        }

        //マウス
        float mouseWheelScroll = Input.GetAxis("Mouse ScrollWheel");
        origin += new Vector3(0, 0, -5 * mouseWheelScroll);
        if (Input.GetMouseButton(1))
        {
            transform.Translate(this.transform.right * speed * Time.deltaTime);
        }
        else if (Input.GetMouseButton(0))
        {
            transform.Translate(-1*this.transform.right * speed * Time.deltaTime);
        }
    }
}
