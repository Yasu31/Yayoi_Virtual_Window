using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMoveScript : MonoBehaviour {
    //http://thomasfredericks.github.io/UnityOSC/
    public OSC osc;
    public float fovDegrees=100;//horizontal fanning. Determining this accurately is important in correct measurements
    public float screenWidth=30;//in cm
    public float screenHeight = 17;//in cm
    public float numerator = 200;
    public float[] captureSize = { 640, 320 };

    private bool isReceiving = false;
    private float theta;  
    private float rawX, rawY, rawScale;
    private Vector3 facePos = new Vector3(0, 0, -40);

    private float xAid, yAid;   //will use this when calculating face position.
    //computes it here so it doesn't have to every time.
    

	// Use this for initialization
	void Start () {
        osc.SetAddressHandler("/pose/position", OnReceiveFace);
        osc.SetAddressHandler("/pose/scale", onReceiveScale);

        theta = fovDegrees * (3.14f / 180);//horizontal fanning of camera in radians 

        //will use these values later to compute face position.
        //it's calculated here to save the effort of having to compute every frame
        xAid = Mathf.Tan(theta / 2) / captureSize[0] *2;
        yAid = Mathf.Tan((theta / 2)*captureSize[1]/captureSize[0]) / captureSize[1] *2;
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

        //make this code smarter
        //to determine if it's actually been receiving data for a while
        isReceiving = true;
    }
    void UpdateFacePos()
    {
        if (!isReceiving)
        {
            print("is not receiving face data from faceOSC");
            return;
        }
        float distance = numerator / rawScale;
        float phiX = -Mathf.Atan((rawX - captureSize[0] / 2) * xAid);
        float phiY = -Mathf.Atan((rawY - captureSize[1] / 2) * yAid);
        facePos.x = distance * Mathf.Cos(phiY) * Mathf.Sin(phiX);
        facePos.y = distance * Mathf.Cos(phiX) * Mathf.Sin(phiY) + screenHeight / 2 +4;//eye is higher than face position
        facePos.z = -distance * Mathf.Cos(phiX)*Mathf.Cos(phiY);

        print(facePos);
    }

}
