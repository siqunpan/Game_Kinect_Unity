using UnityEngine;
using System.Collections;

public class BirdManager : MonoBehaviour {

	private Vector3 v3_speed = Vector3.zero;

	// Use this for initialization
	void Start () {

	}

	public void initBird(Vector3 _v3_speedBird)
	{
		v3_speed = _v3_speedBird;
	}

	// Update is called once per frame
	void Update () {
		transform.position += v3_speed * Time.deltaTime;
	}
}
