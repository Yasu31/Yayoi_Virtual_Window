using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show_and_hide : MonoBehaviour {

    private bool wasClosed = false;
    private float closeStartTime;
    public OSC osc;

    private int count = 0;
    private float sum = 0;
    private float average;

    private bool mode = false;

    public Material skyboxNow;
    public Material skyboxPast;

    public GameObject everyObject;

    // Use this for initialization
    void Start () {
        osc.SetAddressHandler("/gesture/eye/left", OnReceiveEye);
        RenderSettings.skybox = skyboxNow;
        everyObject.SetActive(false);
    }
	private void OnReceiveEye(OscMessage msg)
    {
        if(Time.time < 1.5)
        {
            //calibration should not start immediately
            return;
        }
        if (count < 50)
        {
            count += +1;
            sum += msg.GetFloat(0);
        }
        else if (count == 50)
        {
            average = sum / count;
            print("initialized");
            print(average);
            count += 1;
        }
        else
        {
            if (msg.GetFloat(0) < average-0.2)
            {
                if (!wasClosed)
                {
                    //print("detect close");
                    wasClosed = true;
                    closeStartTime = Time.time;
                }
                if (Time.time - closeStartTime > 1)
                {
                    closeStartTime = Time.time+2;
                    print("change mode");
                    mode =! mode;
                    everyObject.SetActive(mode);
                    if (mode)
                    {
                        RenderSettings.skybox = skyboxPast;
                    }
                    else
                    {
                        RenderSettings.skybox = skyboxNow;
                    }
                    DynamicGI.UpdateEnvironment();
                }
            }
            else
            {
                wasClosed = false;
            }
        }
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(2))
        {
            mode = !mode;
            everyObject.SetActive(mode);
            if (mode)
            {
                RenderSettings.skybox = skyboxPast;
            }
            else
            {
                RenderSettings.skybox = skyboxNow;
            }
            DynamicGI.UpdateEnvironment();
        }

    }
}
