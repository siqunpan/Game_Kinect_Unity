using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	private int i_score = 0;
	private int i_healthPoints;
	private int i_maxHealthPoints; // must be even number
	private HandInputManager him;
	public GUITexture heartGUI;
	public Texture fullHeartTex;
	public Texture halfHeartTex;
	public Texture emptyHeartTex;
	private float heartSpacingX = 30;
	private float heartMarginX = 0;
	private float heartMarginY = 0;
	private float heartSize = 30;
	private GameObject[] hearts;
	public GameObject go_gameover;

	GUIStyle style = new GUIStyle();

	private static GUIManager _instance;
	
	public static GUIManager Instance
	{
		get
		{
			_instance = FindObjectOfType(typeof(GUIManager)) as GUIManager;
			if(_instance == null)
			{
				_instance = GameObject.Instantiate(GlobalVariables.GO_GUI_MANAGER) as GUIManager;
			}
			
			return _instance;
		}
	}
	
	// Use this for initialization
	void Start () {

		him = HandInputManager.Instance;
		him.rightHandPositionDetected += new RightHandPositionDetectedEventHandler(onRightHandPositionDetected);

		style.fontSize = 30;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.red;

		i_maxHealthPoints = GlobalVariables.I_MAX_HEALTH_POINTS;
		i_healthPoints = i_maxHealthPoints;
		hearts = new GameObject[i_maxHealthPoints/2];
		initHearts ();
	}
	
	void initHearts(){
		int fullHeartNum = this.i_maxHealthPoints / 2;
		for (int i=0; i<fullHeartNum; i++) {
			GameObject go_newHeart = (GameObject)Instantiate (heartGUI.gameObject);
			go_newHeart.guiTexture.pixelInset = new Rect(heartMarginX+heartSpacingX*i, heartMarginY, heartSize, heartSize); 
			hearts[i] = go_newHeart;
		}
	}
	
	void updateHearts(){
		int fullHeartNum = this.i_healthPoints / 2;
		int halfHeartNum = this.i_healthPoints % 2;
		int emptyHeartNum = this.i_maxHealthPoints / 2 - fullHeartNum - halfHeartNum;
		int totalHeartNum = hearts.Length;
		for(int i=0;i<fullHeartNum;i++){
			hearts[i].guiTexture.texture = fullHeartTex;
		}
		if (halfHeartNum == 1) {
			hearts[fullHeartNum].guiTexture.texture = halfHeartTex;
		}
		if (fullHeartNum + halfHeartNum < totalHeartNum) {
			for(int i=fullHeartNum + halfHeartNum; i<totalHeartNum;i++){
				hearts[i].guiTexture.texture = emptyHeartTex;
			}
		}
	}

	public int getScore(){
		return i_score;
	}

	public int getHp(){
		return i_healthPoints;
	}

	public void addScore(int _i_score)
	{
		i_score += _i_score;
	}
	
	public void reduceScore(int _i_score)
	{
		i_score -= _i_score;
	}
	
	public void reduceHealthPoints(int _i_points)
	{
		i_healthPoints -= _i_points;
	}
	
	public void addHealthPoints(int _i_points)
	{
		i_healthPoints += _i_points;
	}
	
	public void reduceMaxHealthPoints(int _i_points)
	{
		i_maxHealthPoints -= _i_points;
	}
	
	public void addMaxHealthPoints(int _i_points)
	{
		i_maxHealthPoints += _i_points;
	}
	
	void OnGUI () {
		// render score
		GUILayout.Label ("SCORE: "+ this.i_score,style);
		if (i_healthPoints < 0) {
			i_healthPoints = 0;
		}
		// render hp
		updateHearts ();
		if (i_healthPoints <= 0) {
			go_gameover.SetActive(true);
			if(GUI.Button (new Rect(Screen.width - 200, Screen.height - 50, 150, 30), "Restart"))
			{
				go_gameover.SetActive(false);
				i_healthPoints = i_maxHealthPoints;
				i_score = 0;
			}
			if(GUI.Button (new Rect(Screen.width - 200, 50,150, 30), "Back"))
			{
				him.rightHandPositionDetected -= new RightHandPositionDetectedEventHandler(onRightHandPositionDetected);
				Application.LoadLevel("Menu");
			}
		}
	}

	void Update(){
	}
	
	public void onRightHandPositionDetected(object sender, RightHandPositionDetectedEventArgs e)
	{
		if (i_healthPoints > 0) return;

		if (e.pos == RightHandPosition.UP) {
			him.rightHandPositionDetected -= new RightHandPositionDetectedEventHandler(onRightHandPositionDetected);
			Application.LoadLevel("Menu");
		}
		else if(e.pos == RightHandPosition.DOWN){
			// restart game
			go_gameover.SetActive(false);
			i_healthPoints = i_maxHealthPoints;
			i_score = 0;
		}
	}
}
