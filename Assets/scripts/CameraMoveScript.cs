using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMoveScript : MonoBehaviour {
    //http://thomasfredericks.github.io/UnityOSC/
    public OSC osc;
	private float fovDegrees=100;//horizontal fanning. Determining this accurately is important in correct measurements
	private float screenWidth=30;//in cm
	private float screenHeight = 17;//in cm
	private float numerator = 200;
    private int numOfScenes = 3;
	private float[] captureSize = { 640, 320 };
	public enum Device{
		Macbook_Pro_13in, MBP13_withLens, 学科PC, Macbook_Air, snoopy_kali, TV47
    }
	private Device device=Device.Macbook_Pro_13in;
    public enum Webcam
    {
        学科PC, MBP, MBP_with_lens, Buffalo
    }
    private Webcam webcam=Webcam.MBP;

	//Camera cam;

    private bool isReceiving = false;
	Vector3 bottomLeft;
	Vector3 bottomRight;
	Vector3 topLeft;
    private float theta;
    private float rawX, rawY, rawScale;
    private Vector3 facePos = new Vector3(-15, -7, -40);

    private float xAid, yAid;   //will use this when calculating face position.
    //computes it here so it doesn't have to every time.

    private bool wasOpen = false; //to check if in previous data, mouth was open.
    private float openStartTime;

    string messagePopup = "";
    private float messagePopupStart;

    GUIStyle style = new GUIStyle(GUIStyle.none);


    // Use this for initialization
    void Start () {
		switch (device) {
		case Device.Macbook_Pro_13in:
			screenWidth = 30;
			screenHeight = 17;
			break;
		case Device.MBP13_withLens:
			screenWidth = 30;
			screenHeight = 17;
			break;
		case Device.Macbook_Air:
			screenWidth = 30;
			screenHeight = 17;
			break;
		case Device.学科PC:
			screenWidth = 30;
			screenHeight = 17;
			break;
        case Device.snoopy_kali:
            screenWidth = 150;
            screenHeight = 75;
            break;
        case Device.TV47:
            screenWidth = 104;
            screenHeight = 59;
            break;
        }
        switch (webcam)
        {
            case Webcam.MBP:
                fovDegrees = 90;
                numerator = 200;
                captureSize = new float[] { 640, 320 };
                break;
            case Webcam.MBP_with_lens:
                fovDegrees = 90;
                numerator = 130;
                captureSize = new float[] { 640, 320 };
                break;
            case Webcam.学科PC:
                fovDegrees = 60;
                numerator = 200;
                captureSize = new float[] { 640, 480 };
                break;
            case Webcam.Buffalo:
                fovDegrees = 120;
                numerator = 200;
                captureSize = new float[] { 640, 480 };
                break;
        }

        osc.SetAddressHandler("/pose/position", OnReceiveFace);
        osc.SetAddressHandler("/pose/scale", onReceiveScale);
        osc.SetAddressHandler("/gesture/mouth/height", onReceiveJaw);

        theta = fovDegrees * (3.14f / 180);//horizontal fanning of camera in radians

        //will use these values later to compute face position.
        //it's calculated here to save the effort of having to compute every frame
        xAid = Mathf.Tan(theta / 2) / captureSize[0] *2;
        yAid = Mathf.Tan((theta / 2)*captureSize[1]/captureSize[0]) / captureSize[1] *2;


		bottomLeft = new Vector3 (-screenWidth / 2, -screenHeight / 2, 0);
		bottomRight = new Vector3 (screenWidth / 2, -screenHeight / 2, 0);
		topLeft = new Vector3 (-screenWidth / 2, screenHeight / 2, 0);
        //cam = GetComponent<Camera> ();

        switch (SceneManager.GetActiveScene().name)
        {
            case "forest":
                setMessage("太古の弥生時代の発掘現場です。");
                break;
            default:
                setMessage("loaded");
                break;
        }

        style.fontSize = 50;
        style.wordWrap = true;

    }

    private void setMessage(string msg)
    {
        messagePopup = msg;
        messagePopupStart = Time.time;
    }
    private void OnGUI()
    {
        if (Time.time - messagePopupStart < 4)
        {
            GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 40, 400, 80), messagePopup, style);
        }
    }


    // Update is called once per frame
    void LateUpdate () {
        UpdateFacePos();
		Camera cam = Camera.main;
		Matrix4x4 pm=GeneralizedPerspectiveProjection(bottomLeft, bottomRight, topLeft, facePos, cam.nearClipPlane, cam.farClipPlane);
		transform.position=facePos;
		cam.projectionMatrix = pm;

        if (Input.GetMouseButtonDown(0))
            changeScene();
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
    void onReceiveJaw(OscMessage message)
    {
        if (message.GetFloat(0) > 5)
        {
            if (!wasOpen)
            {
                print("detect open");
                wasOpen = true;
                openStartTime = Time.time;
            }
            if(Time.time-openStartTime > 2.0)
            {
                wasOpen = false;
                changeScene();
            }
        }
        else
        {
            wasOpen = false;
        }
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
        facePos.z = -distance;// * Mathf.Cos(phiX)*Mathf.Cos(phiY);

        //print(facePos);
    }

    void changeScene()
    {
        print("changing scene..");
        setMessage("次のシーンをロード中...");
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % numOfScenes);
    }
	public static Matrix4x4 GeneralizedPerspectiveProjection(Vector3 pa, Vector3 pb, Vector3 pc, Vector3 pe, float near, float far){
		Vector3 va, vb, vc;
		Vector3 vr, vu, vn;

		float left, right, bottom, top, eyedistance;

		Matrix4x4 transformMatrix;
		Matrix4x4 projectionM;
		Matrix4x4 eyeTranslateM;
		Matrix4x4 finalProjection;
		vr = pb - pa;
		vr.Normalize ();
		vu = pc - pa;
		vu.Normalize ();
		vn = Vector3.Cross (vr, vu);
		vn.Normalize ();

		va = pa - pe;
		vb = pb - pe;
		vc = pc - pe;
		eyedistance = (Vector3.Dot (va, vn));

		left = (Vector3.Dot (vr, va) * near) / eyedistance;
		right = (Vector3.Dot (vr, vb) * near) / eyedistance;
		bottom = (Vector3.Dot (vu, va) * near) / eyedistance;
		top = (Vector3.Dot (vu, vc) * near) / eyedistance;
		projectionM = PerspectiveOffCenter (left, right, bottom, top, near, far);

		transformMatrix = new Matrix4x4 ();
		transformMatrix [0, 0] = vr.x;
		transformMatrix [0, 1] = vr.y;
		transformMatrix [0, 2] = vr.z;
		transformMatrix [0, 3] = 0;
		transformMatrix [1, 0] = vu.x;
		transformMatrix [1, 1] = vu.y;
		transformMatrix [1, 2] = vu.z;
		transformMatrix [1, 3] = 0;
		transformMatrix [2, 0] = vn.x;
		transformMatrix [2, 1] = vn.y;
		transformMatrix [2, 2] = vn.z;
		transformMatrix [2, 3] = 0;
		transformMatrix [3, 0] = 0;
		transformMatrix [3, 1] = 0;
		transformMatrix [3, 2] = 0;
		transformMatrix [3, 3] = 1f;

		eyeTranslateM = new Matrix4x4 ();
		eyeTranslateM [0, 0] = 1;
		eyeTranslateM [0, 1] = 0;
		eyeTranslateM [0, 2] = 0;
		eyeTranslateM [0, 3] = -pe.x;
		eyeTranslateM [1, 0] = 0;
		eyeTranslateM [1, 1] = 1;
		eyeTranslateM [1, 2] = 0;
		eyeTranslateM [1, 3] = -pe.y;
		eyeTranslateM [2, 0] = 0;
		eyeTranslateM [2, 1] = 0;
		eyeTranslateM [2, 2] = 1;
		eyeTranslateM [2, 3] = -pe.z;
		eyeTranslateM [3, 0] = 0;
		eyeTranslateM [3, 1] = 0;
		eyeTranslateM [3, 2] = 0;
		eyeTranslateM [3, 3] = 1f;

		finalProjection = new Matrix4x4 ();
		finalProjection = Matrix4x4.identity * projectionM * transformMatrix;//* eyeTranslateM;

		//return finalProjection;
		return finalProjection;

	}

	static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
	{
		float x = 2.0F * near / (right - left);
		float y = 2.0F * near / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0F * far * near) / (far - near);
		float e = -1.0F;
		Matrix4x4 m = new Matrix4x4();
		m[0, 0] = x;
		m[0, 1] = 0;
		m[0, 2] = a;
		m[0, 3] = 0;
		m[1, 0] = 0;
		m[1, 1] = y;
		m[1, 2] = b;
		m[1, 3] = 0;
		m[2, 0] = 0;
		m[2, 1] = 0;
		m[2, 2] = c;
		m[2, 3] = d;
		m[3, 0] = 0;
		m[3, 1] = 0;
		m[3, 2] = e;
		m[3, 3] = 0;
		return m;
	}

    //tried to have the window settings to be user-definable, but I don't have time now.
    //    private Rect windowrect = new Rect(0, 0, 120, 120);
    //   private int fov;
    //    private int captureWidth;
    //    private int captureHeight;
    //    private int screenWidth2;
    //    private int screenHeight2;
    //    private int numerator2;
    //    void OnGUI()
    //    {
    //       windowrect = GUI.Window(0, windowrect, DoMyWindow, "configuration");
    //
    //    }

    //    void DoMyWindow()
    //    {
    //        GUI.Label(new Rect(0, 0, 60, 20), "fov");
    //        GUI.TextField(new Rect(60,0,60,20), )
    //    }



}
