using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMoveScript : MonoBehaviour {
    //http://thomasfredericks.github.io/UnityOSC/
    public OSC osc;
    
    private float rawX, rawY, rawScale = 0;
    private Vector3 facePos = new Vector3(0, 0, -40);

	// Use this for initialization
	void Start () {
        osc.SetAddressHandler("/pose/position", OnReceiveFace);
        osc.SetAddressHandler("/pose/scale", onReceiveScale);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateFacePos();
        transform.position = facePos;		
	}

    void OnReceiveFace(OscMessage message)
    {
        rawX = message.GetFloat(0);
        //0~640
        rawY = message.GetFloat(1);
        //0~360
    }
    void onReceiveScale(OscMessage message)
    {
        rawScale = message.GetFloat(0);
    }
    void UpdateFacePos()
    {
        facePos.x= (rawX-320)/10;
        facePos.y = (rawY-180) / 10;
        print(facePos);
    }
}
