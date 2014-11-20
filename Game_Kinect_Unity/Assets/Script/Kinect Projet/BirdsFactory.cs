using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdsFactory : MonoBehaviour {
	
	private float f_sizeWorldTriggerColliderProduceBird;

	private float f_distanceCameraProduceBird = -1f; 
	private float f_leftBorderCameraProduceBird = -1f;
	private float f_rightBorderCameraProduceBird = -1f;
	private float f_downBorderCameraProduceBird = -1f;
	private float f_upBorderCameraProduceBird = -1f;

	private float f_distanceCameraProduceBirdNearPlane = -1f; 
	private float f_leftBorderCameraProduceBirdNearPlane = -1f;
	private float f_rightBorderCameraProduceBirdNearPlane = -1f;
	private float f_downBorderCameraProduceBirdNearPlane = -1f;
	private float f_upBorderCameraProduceBirdNearPlane = -1f;

	private Vector3 v3_posProduceBird = Vector3.zero;
	private Vector3 v3_posDestinationBird = Vector3.zero;

	private float f_maxSpeedBirdAxisZ = -1f;
	private Vector3 v3_speedBird = Vector3.zero;

	private List<GameObject> list_leftSidebirds = new List<GameObject>();
	private List<GameObject> list_rightSidebirds = new List<GameObject>();
	private List<GameObject> list_upSidebirds = new List<GameObject>();

	private static BirdsFactory _instance;

	public static BirdsFactory Instance
	{
		get
		{
			_instance = FindObjectOfType(typeof(BirdsFactory)) as BirdsFactory;
			if(_instance == null)
			{
				_instance = GameObject.Instantiate(GlobalVariables.GO_BIRDS_FACTORY) as BirdsFactory;
			}

			return _instance;
		}
	}


	// Use this for initialization
	void Start () {
		//The collider here is a sphere, so localScale.x, .y and .z is the same; 
		f_sizeWorldTriggerColliderProduceBird = GlobalVariables.GO_BIRD.transform.localScale.x;

		f_distanceCameraProduceBird = GlobalVariables.I_DEPTH_PRODUCE_BIRD;
		f_leftBorderCameraProduceBird = Camera.main.ViewportToWorldPoint (new Vector3 (0f, 0f, f_distanceCameraProduceBird)).x;
		f_rightBorderCameraProduceBird = Camera.main.ViewportToWorldPoint (new Vector3 (1f, 0f, f_distanceCameraProduceBird)).x;
		f_downBorderCameraProduceBird = Camera.main.ViewportToWorldPoint (new Vector3 (0f, 0f, f_distanceCameraProduceBird)).y;
		f_upBorderCameraProduceBird = Camera.main.ViewportToWorldPoint (new Vector3 (0f, 1f, f_distanceCameraProduceBird)).y;

		//The collider here is a sphere
		f_leftBorderCameraProduceBird += f_sizeWorldTriggerColliderProduceBird * 0.7f;
		f_rightBorderCameraProduceBird -= f_sizeWorldTriggerColliderProduceBird * 0.7f;
		f_downBorderCameraProduceBird += f_sizeWorldTriggerColliderProduceBird * 0.7f;
		f_upBorderCameraProduceBird -= f_sizeWorldTriggerColliderProduceBird * 0.7f;

		f_distanceCameraProduceBirdNearPlane = Camera.main.nearClipPlane;
		f_leftBorderCameraProduceBirdNearPlane = Camera.main.ViewportToWorldPoint (new Vector3 (0f, 0f, f_distanceCameraProduceBirdNearPlane)).x;
		f_rightBorderCameraProduceBirdNearPlane = Camera.main.ViewportToWorldPoint (new Vector3 (1f, 0f, f_distanceCameraProduceBirdNearPlane)).x;
		f_downBorderCameraProduceBirdNearPlane = Camera.main.ViewportToWorldPoint (new Vector3 (0f, 0f, f_distanceCameraProduceBirdNearPlane)).y;
		f_upBorderCameraProduceBirdNearPlane = Camera.main.ViewportToWorldPoint (new Vector3 (0f, 1f, f_distanceCameraProduceBirdNearPlane)).y;
		
		//The collider here is a sphere
		f_leftBorderCameraProduceBirdNearPlane += f_sizeWorldTriggerColliderProduceBird * 0.7f;
		f_rightBorderCameraProduceBirdNearPlane -= f_sizeWorldTriggerColliderProduceBird * 0.7f;
		f_downBorderCameraProduceBirdNearPlane += f_sizeWorldTriggerColliderProduceBird * 0.7f;
		f_upBorderCameraProduceBirdNearPlane -= f_sizeWorldTriggerColliderProduceBird * 0.7f;


		f_maxSpeedBirdAxisZ = GlobalVariables.F_MAX_SPEED_BIRD_AXIS_Z;
	}
	
	public void produceBirds(float _f_speedAdd, int i_numBirds)
	{
		int i_posProduceBirdMode = -1; 
		float f_duringBird = -1f;
		GameObject go_bird = null;
		int i_positionBirdBefore = -1;

		for(int i = 0; i < i_numBirds; ++i)
		{
			while(i_posProduceBirdMode == i_positionBirdBefore)
			{
				i_posProduceBirdMode = Random.Range (0, 3);
			}
				
			i_positionBirdBefore = i_posProduceBirdMode;

			if(i_posProduceBirdMode == 0)
			{
				v3_posProduceBird.x = f_leftBorderCameraProduceBird;
				v3_posProduceBird.y = (f_downBorderCameraProduceBird + f_upBorderCameraProduceBird) / 2f;

				v3_posDestinationBird.x = f_leftBorderCameraProduceBirdNearPlane;
				v3_posDestinationBird.y = (f_downBorderCameraProduceBirdNearPlane + f_upBorderCameraProduceBirdNearPlane) / 2f;
			}
			else if(i_posProduceBirdMode == 1)
			{
				v3_posProduceBird.x = (f_leftBorderCameraProduceBird + f_rightBorderCameraProduceBird) / 2f;
				v3_posProduceBird.y = f_upBorderCameraProduceBird;

				v3_posDestinationBird.x = (f_leftBorderCameraProduceBirdNearPlane + f_rightBorderCameraProduceBirdNearPlane) / 2f;
				v3_posDestinationBird.y = f_upBorderCameraProduceBirdNearPlane;
			}
			else if(i_posProduceBirdMode == 2)
			{
				v3_posProduceBird.x = f_rightBorderCameraProduceBird;
				v3_posProduceBird.y = (f_downBorderCameraProduceBird + f_upBorderCameraProduceBird) / 2f;

				v3_posDestinationBird.x = f_rightBorderCameraProduceBirdNearPlane;
				v3_posDestinationBird.y = (f_downBorderCameraProduceBirdNearPlane + f_upBorderCameraProduceBirdNearPlane) / 2f;
			}

			v3_posProduceBird.z = f_distanceCameraProduceBird;

			v3_posDestinationBird.z = f_distanceCameraProduceBirdNearPlane;

			v3_speedBird.z = f_maxSpeedBirdAxisZ + _f_speedAdd;
			f_duringBird =  Mathf.Abs ((v3_posDestinationBird.z - v3_posProduceBird.z) /(v3_speedBird.z));
			v3_speedBird.x = (v3_posDestinationBird.x - v3_posProduceBird.x) / f_duringBird;
			v3_speedBird.y = (v3_posDestinationBird.y - v3_posProduceBird.y) / f_duringBird;

			go_bird = GameObject.Instantiate (GlobalVariables.GO_BIRD, v3_posProduceBird, Quaternion.identity) as GameObject;
			go_bird.GetComponentInChildren<BirdManager>().initBird(v3_speedBird);
			if(i_posProduceBirdMode == 0)
			{
				list_leftSidebirds.Add(go_bird);
			}
			else if(i_posProduceBirdMode == 1)
			{
				list_upSidebirds.Add(go_bird);
			}
			else if(i_posProduceBirdMode == 2)
			{
				list_rightSidebirds.Add (go_bird);
			}

			go_bird.transform.LookAt (Camera.main.transform);
		}
	}

	public void destroyBirdDirectly(GameObject _go_bird)
	{
		if(list_leftSidebirds.Contains(_go_bird))
		{
			list_leftSidebirds.Remove(_go_bird);
		}
		else if(list_upSidebirds.Contains(_go_bird))
		{
			list_upSidebirds.Remove(_go_bird);
		}
		else if(list_rightSidebirds.Contains(_go_bird))
		{
			list_rightSidebirds.Remove(_go_bird);
		}

		GameObject.Destroy (_go_bird);
	}

	public bool destroyDirdByHandMotion(HandMotion _enum_handMotion)
	{
		bool b_destroyed = false;

		if(_enum_handMotion == HandMotion.LEFT_HAND_WAVE_OUT)
		{
			if(list_leftSidebirds.Count != 0 && list_leftSidebirds[0] != null)
			{
				GameObject go_bird = list_leftSidebirds[0];
				list_leftSidebirds.RemoveAt(0);
				GameObject.Destroy(go_bird);
				b_destroyed = true;
				//Debug.Log("Left bird has been destroyed!!!!!");
			}
		}
		else if(_enum_handMotion == HandMotion.DOUBLE_HAND_RISE)
		{
			if(list_upSidebirds.Count != 0 && list_upSidebirds[0] != null)
			{
				GameObject go_bird = list_upSidebirds[0];
				list_upSidebirds.RemoveAt(0);
				GameObject.Destroy(go_bird);
				b_destroyed = true;
				//Debug.Log("Up bird has been destroyed");

			}
		}
		else if(_enum_handMotion == HandMotion.RIGHT_HAND_WAVE_OUT)
		{
			if(list_rightSidebirds.Count != 0 && list_rightSidebirds[0] != null)
			{
				GameObject go_bird = list_rightSidebirds[0];
				list_rightSidebirds.RemoveAt(0);
				GameObject.Destroy(go_bird);
				b_destroyed = true;
				//Debug.Log("Right bird has been destroyed");
			}
		}

		return b_destroyed;
	}
}
