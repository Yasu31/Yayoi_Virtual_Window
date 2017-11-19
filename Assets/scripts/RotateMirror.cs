using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMirror : MonoBehaviour {

	Camera cam = new Camera();

	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 p1 = transform.position - new Vector3 (0, transform.position.y, 0);
		Vector3 p2 = cam.transform.position - new Vector3 (0, cam.transform.position.y, 0);

		if (Vector3.Distance (p1, p2) < 80) {
			transform.Rotate (new Vector3 (0, 1, 0));

		}
	}
}
