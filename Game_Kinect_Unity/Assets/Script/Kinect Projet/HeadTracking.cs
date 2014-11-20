using UnityEngine;
using System.Collections;

public class HeadTracking : MonoBehaviour {

	public Vector3 v3_speed = Vector3.zero;
	public HandInputManager him = null;

	private Color color_buttonBack = Color.black;

	// Use this for initialization
	void Start () {
		him.rightHandPositionDetected += new RightHandPositionDetectedEventHandler(onRightHandPositionDetected);
	}

	public void onRightHandPositionDetected(object sender, RightHandPositionDetectedEventArgs e)
	{
		switch(e.pos){
		case RightHandPosition.UP: 
			color_buttonBack = Color.green;
			Application.LoadLevel("Menu");
			//Debug.Log("UP detected");
			break;
		case RightHandPosition.MID: 
			//Debug.Log("MID detected");
			break;
		case RightHandPosition.DOWN: 
			//Debug.Log("DOWN detected");
			break;
		default: 
			break;
		}
	}
	
//	// Update is called once per frame
//	void Update () {
//
//		if (Input.GetKeyDown (KeyCode.UpArrow)) {
//			Camera.main.transform.Translate (0f, v3_speed.y, 0f);
//		}
//		else if(Input.GetKeyDown(KeyCode.DownArrow))
//		{
//			Camera.main.transform.Translate(0f,-v3_speed.y, 0f);
//		}
//		else if(Input.GetKeyDown(KeyCode.LeftArrow))
//		{
//			Camera.main.transform.Translate(-v3_speed.x,0f, 0f);
//		}
//		else if(Input.GetKeyDown(KeyCode.RightArrow))
//		{
//			Camera.main.transform.Translate(+v3_speed.x,0f, 0f);
//		}
//
//	}

	void OnGUI()
	{
		GUI.backgroundColor = color_buttonBack;
		if(GUI.Button (new Rect(Screen.width - 200, 50,150, 30), "Back"))
		{
			Application.LoadLevel("Menu");
		}
	}
}
















