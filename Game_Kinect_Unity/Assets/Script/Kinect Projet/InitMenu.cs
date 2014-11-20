using UnityEngine;
using System.Collections;

public class InitMenu : MonoBehaviour {

	public HandInputManager him = null;

	private float f_weightButtonGUI = 0f;
	private float f_heightButtonGUI = 0f;
	private Color color_buttonHeadTracking = Color.black;
	private Color color_buttonStartGame = Color.black;
	private Color color_quitGame = Color.black;
	private AudioManager audioManager;

	// Use this for initialization
	void Start () {

		f_weightButtonGUI = Screen.width / 3f;
		f_heightButtonGUI = Screen.height * 0.125f;

		// listen right hand position detected event
		him.rightHandPositionDetected += new RightHandPositionDetectedEventHandler(onRightHandPositionDetected);

		audioManager = AudioManager.Instance;
	}

	void onRightHandPositionDetected (object sender, RightHandPositionDetectedEventArgs e){
		switch(e.pos){
			case RightHandPosition.UP: 
				color_buttonHeadTracking = Color.green;
				Application.LoadLevel("HeadTracking");
				//Debug.Log("UP detected");
			break;
			case RightHandPosition.MID: 
				color_buttonStartGame = Color.green;
				Application.LoadLevel("Game");
				//Debug.Log("MID detected");
			break;
			case RightHandPosition.DOWN: 
				color_quitGame = Color.green;
				Application.Quit();
				//Debug.Log("DOWN detected");
			break;
			default: 
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
//		GUI.Box (new Rect (Screen.width / 4f, Screen.height / 8f, Screen.width / 2f, Screen.height / 2f), "Kinect Projet");

		/*
		if(GUI.Button(new Rect(Screen.width * 0.5f- f_weightButtonGUI * 0.5f, Screen.height * 0.25f - f_heightButtonGUI * 0.5f, f_weightButtonGUI, f_heightButtonGUI), "HEAD TRACKING"))
		{
			Debug.Log("HEAD TRACKING");
		}

		if(GUI.Button(new Rect(Screen.width * 0.5f- f_weightButtonGUI * 0.5f, Screen.height * 0.5f - f_heightButtonGUI * 0.5f, f_weightButtonGUI, f_heightButtonGUI), "START GAME"))
		{
			Debug.Log ("game");
		}

		if(GUI.Button(new Rect(Screen.width * 0.5f- f_weightButtonGUI * 0.5f, Screen.height * 0.75f - f_heightButtonGUI * 0.5f, f_weightButtonGUI, f_heightButtonGUI), "QUIT GAME"))
		{
			//Application.Quit();
			Debug.Log ("quit");
		}
		*/

		GUI.backgroundColor = color_buttonHeadTracking;
		if(GUI.Button (new Rect (Screen.width * 0.5f - f_weightButtonGUI * 0.5f, Screen.height * 0.25f - f_heightButtonGUI * 0.5f, f_weightButtonGUI, f_heightButtonGUI), "HEAD TRACKING"))
		{
			Application.LoadLevel("HeadTracking");
		}
		GUI.backgroundColor = color_buttonStartGame;
		if(GUI.Button (new Rect (Screen.width * 0.5f - f_weightButtonGUI * 0.5f, Screen.height * 0.5f - f_heightButtonGUI * 0.5f, f_weightButtonGUI, f_heightButtonGUI), "START GAME"))
		{
			Application.LoadLevel("Game");
		}
		GUI.backgroundColor = color_quitGame;
		if(GUI.Button (new Rect (Screen.width * 0.5f - f_weightButtonGUI * 0.5f, Screen.height * 0.75f - f_heightButtonGUI * 0.5f, f_weightButtonGUI, f_heightButtonGUI), "QUIT GAME"))
		{
			Application.Quit();
		}
	}
}
