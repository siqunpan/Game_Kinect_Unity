using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	/*
	 * Attention: the gameobjects "Main Camera" and "GameManager" should both be at the position (0, 0, 0)
	 * 			  the axis-z presente the depth (the distance of the camera)
	 */

	private BoxCollider2D collider;
	private BirdsFactory birdsfactory;
	private GUIManager guiManager;
	private HandInputManager handInputManager;
	private AudioManager audioManager;

	private float f_durationProduceBird = 2f;
	private float f_speedAdd = 0f;
	private float f_timerGame = 0f;	
	private int i_score = 0;
	private int i_healthPoints = 0; 

	// Use this for initialization
	void Start () {
	
//		collider = GetComponent<BoxCollider2D> ();
//		collider.size = new Vector2 (Screen.width, Screen.height);

		birdsfactory = BirdsFactory.Instance;
		guiManager = GUIManager.Instance;
		handInputManager = HandInputManager.Instance;
		audioManager = AudioManager.Instance;

		birdsfactory.produceBirds(f_speedAdd, 1);

		handInputManager.handMotionDetected += new HandMotionDetectedEventHandler (onHandMotionDetected);

		i_healthPoints = GlobalVariables.I_MAX_HEALTH_POINTS;
	}
	
	// Update is called once per frame
	void Update () {
		i_healthPoints = guiManager.getHp ();
		i_score = guiManager.getScore ();
		if (i_healthPoints <= 0) return;
		if(f_speedAdd <= Mathf.Abs(GlobalVariables.F_MAX_SPEED_BIRD_AXIS_Z)*2)
		{
			f_speedAdd = i_score/10*3;
		}

		f_timerGame += Time.deltaTime;

		if(f_timerGame >= f_durationProduceBird)
		{
			int i_numBird = Random.Range(0,3);
			if(i_numBird <=1 ) i_numBird = 1;

			birdsfactory.produceBirds(0f-f_speedAdd, i_numBird);

			f_timerGame = 0f;
		}
	}

	public void onHandMotionDetected(object sender, HandMotionDetectedEventArgs e)
{
		if(birdsfactory.destroyDirdByHandMotion (e.motion) == true)
		{
			guiManager.addScore(1);
			audioManager.playAudioWeep();
		}
	}

	void OnTriggerEnter(Collider other) {
		birdsfactory.destroyBirdDirectly (other.transform.parent.gameObject);

		if(i_healthPoints > 0)
		{
			guiManager.reduceHealthPoints (1);
		}

		audioManager.playAudioHit ();

//		Color startCameraColor = Camera.main.backgroundColor;
//		StartCoroutine (changeCameraBackgroundColor(startCameraColor, Color.red));
	}

//	IEnumerator changeCameraBackgroundColor(Color _startColor, Color _newColor)
//	{
//		Camera.main.clearFlags = CameraClearFlags.SolidColor;
//		Camera.main.backgroundColor = _newColor;
//
//		yield return new WaitForSeconds (0.1f);
//
//		Camera.main.backgroundColor = _startColor;
//	}
}
