using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {

	public static SkeletonWrapper GO_KINECT_PREFAB;
	public SkeletonWrapper go_KinectPrefab;

	public static GUIManager GO_GUI_MANAGER;
	public GUIManager go_guiManager;

	public static HandInputManager GO_HAND_INPUT_MANAGER;
	public HandInputManager go_handInputManager;

	public static AudioManager GO_AUDIO_MANAGER;
	public AudioManager go_audioManager;

	public static GameObject GO_BIRD;
	public GameObject go_bird;

	public static BirdsFactory GO_BIRDS_FACTORY;
	public BirdsFactory go_birdsFactory;

	public static int I_DEPTH_PRODUCE_BIRD;
	public int i_depthProduceBird;

	public static float F_MAX_SPEED_BIRD_AXIS_Z;
	public float f_maxSpeedBirdAxisZ;

	public static int I_MAX_HEALTH_POINTS;
	public int i_maxHealthPoints;

	void Awake()
	{
		GO_KINECT_PREFAB = go_KinectPrefab;

		GO_GUI_MANAGER = go_guiManager;

		GO_HAND_INPUT_MANAGER = go_handInputManager;

		GO_AUDIO_MANAGER = go_audioManager;

		GO_BIRD = go_bird;

		GO_BIRDS_FACTORY = go_birdsFactory;

		I_DEPTH_PRODUCE_BIRD = i_depthProduceBird;

		F_MAX_SPEED_BIRD_AXIS_Z = f_maxSpeedBirdAxisZ;

		I_MAX_HEALTH_POINTS = i_maxHealthPoints;
	}
	// Use this for initialization
	void Start () {
	}
}
