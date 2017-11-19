using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spotLightOnOff : MonoBehaviour {

	//Camera cam = new Camera();
    public GameObject cameraParent;

	// Use this for initialization
	void Start () {
		print ("start");
		//cam = Camera.main;

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 p1 = this.transform.position - new Vector3 (0, this.transform.position.y, 0);
        //Vector3 p2 = cam.transform.position - new Vector3 (0, cam.transform.position.y, 0);
        Vector3 p2 = cameraParent.transform.position - new Vector3(0, cameraParent.transform.position.y, 0);

        if (Vector3.Distance(p1, p2) < 80) {
			this.GetComponent<Light> ().range = 50;
			print ("on");
		} else {
			this.GetComponent<Light> ().range = 0;
			print ("off");
		}
	}
}
