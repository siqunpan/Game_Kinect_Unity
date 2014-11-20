using System;
using UnityEngine;
using System.Collections;

public enum RightHandPosition {
	NON,
	UP,
	MID,
	DOWN
}

public enum HandMotion{
	NON,
	RIGHT_HAND_WAVE_OUT,
	LEFT_HAND_WAVE_OUT,
	RIGHT_HAND_RISE,
	LEFT_HAND_RISE,
	DOUBLE_HAND_RISE
}

// A delegate type for hooking up right hand position detected event notifications.
public delegate void RightHandPositionDetectedEventHandler(object sender, RightHandPositionDetectedEventArgs e);

// A delegate type for hooking up hand motion detected event notifications.
public delegate void HandMotionDetectedEventHandler(object sender, HandMotionDetectedEventArgs e);

public class HandInputManager : MonoBehaviour {

	public event RightHandPositionDetectedEventHandler rightHandPositionDetected;
	public event HandMotionDetectedEventHandler handMotionDetected;

	private SkeletonWrapper sw;
	private int PlayerId = 0;

	public float detectPositionDuration = 1f; // the duration of time for detection of fixed position of right hand
	public float rightHandMarginX = 0.3f;
	private bool firstPos = true;
	private Vector3 rightHandPos; // right hand position
	private Vector3 leftHandPos; // left hand position
	private Vector3 rightHandPosPre = Vector3.zero; // previous right hand position
	private Vector3 leftHandPosPre = Vector3.zero; // previous left hand position
	private Vector3 rightHandRelativePos; // relative position to spine
	private RightHandPosition currentRightHandPos = RightHandPosition.NON;
	private float posStartTime = 0;// record the right hand position start time

	private Vector3 spinePos;
	private Vector3 headPos;

	public float detectMotionDuration = 0.5f; // the duration of time of hand motion
	public float detectMotionDistance = 0.3f; // the distance of hand motion for dectction. use this with detectMotionDuration to restrict the average speed 
	private Vector3 rightHandVelo; // velocity of the right hand (no real velocity, because only the signs are used to identify the motion direction)
	private Vector3 leftHandVelo; // velocity of the left hand
	private Vector3 motionStartRightHandPos;
	private Vector3 motionStartLeftHandPos;
	private float motionRightHandStartTime = 0; // record the hand motion start time
	private float motionLeftHandStartTime = 0;
	private HandMotion currentRightHandMotion = HandMotion.NON;
	private HandMotion currentLeftHandMotion = HandMotion.NON;

	public float accuracy = 0.01f;

	private static HandInputManager _instance;
	
	public static HandInputManager Instance
	{
		get
		{
			_instance = FindObjectOfType(typeof(HandInputManager)) as HandInputManager;
			if(_instance == null)
			{
				_instance = GameObject.Instantiate(GlobalVariables.GO_HAND_INPUT_MANAGER) as HandInputManager;
			}
			
			return _instance;
		}
	}

	// Use this for initialization
	void Start () {
		sw = SkeletonWrapper.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		if(sw.pollSkeleton()){
			float currentTime = Time.time;
			// get right hand position (index = 11)
			this.rightHandPos = sw.bonePos[PlayerId,11];
			// get left hand position (index = 7)
			this.leftHandPos = sw.bonePos[PlayerId,7];
			// get spine position (index = 1)
			this.spinePos = sw.bonePos[PlayerId,1];
			// get head position (index = 3)
			this.headPos = sw.bonePos[PlayerId,3];
			// calculate the relative position of right hand
			this.rightHandRelativePos = this.rightHandPos - this.spinePos;
			float yHandRelativePos = this.rightHandPos.y - this.spinePos.y;
			float xHandRelativePos = this.rightHandPos.x - this.spinePos.x;
			/*
			Debug.Log("=======");
			Debug.Log("right hand: "+rightHandPos);
			Debug.Log("spine: "+spinePos);
			Debug.Log ("Relative pos:" + xHandRelativePos+"# "+yHandRelativePos);
			Debug.Log ("height: " + (headPos.y - spinePos.y));
			Debug.Log("=======");
			*/
			if(xHandRelativePos >= rightHandMarginX && yHandRelativePos <= 0.1){ // right hand position: down
				if(currentRightHandPos != RightHandPosition.DOWN){
					currentRightHandPos = RightHandPosition.DOWN;
					posStartTime = currentTime;
				}
			}
			else if(xHandRelativePos >= rightHandMarginX && yHandRelativePos > 0.2 && yHandRelativePos <=0.4){ // right hand position: mid
				if(currentRightHandPos != RightHandPosition.MID){
					currentRightHandPos = RightHandPosition.MID;
					posStartTime = currentTime;
				}
			}
			else if(xHandRelativePos >= rightHandMarginX && yHandRelativePos > 0.45){ // right hand position: up
				if(currentRightHandPos != RightHandPosition.UP){
					currentRightHandPos = RightHandPosition.UP;
					posStartTime = currentTime;
				}
			}
			else{
				currentRightHandPos = RightHandPosition.NON;
			}

			// validation of right hand position
			if(currentRightHandPos!=RightHandPosition.NON && currentTime - posStartTime >= detectPositionDuration){
				// rise right hand position detected event
				OnRightHandPositionDetected(currentRightHandPos);
				// reset the timer
				posStartTime = currentTime;
			}

			if(firstPos){
				firstPos = false;
			} else{
				// get right hand velocity
				this.rightHandVelo = rightHandPos - rightHandPosPre;
				// get left hand velocity
				this.leftHandVelo = leftHandPos - leftHandPosPre;
				// right hand motion detection
				if(this.rightHandVelo.x > accuracy && Math.Abs(this.rightHandVelo.x) > Math.Abs(this.rightHandVelo.y)){
					if(currentRightHandMotion != HandMotion.RIGHT_HAND_WAVE_OUT){
						currentRightHandMotion = HandMotion.RIGHT_HAND_WAVE_OUT;
						motionRightHandStartTime = currentTime;
						motionStartRightHandPos = rightHandPos;
					}
				}
				else if(this.rightHandVelo.y > accuracy && Math.Abs(this.rightHandVelo.x) < Math.Abs(this.rightHandVelo.y)){
					if(currentRightHandMotion != HandMotion.RIGHT_HAND_RISE){
						currentRightHandMotion = HandMotion.RIGHT_HAND_RISE;
						motionRightHandStartTime = currentTime;
						motionStartRightHandPos = rightHandPos;
					}
				}
				else{
					currentRightHandMotion = HandMotion.NON;
				}

				// left hand motion detection
				if(this.leftHandVelo.x < -accuracy && Math.Abs(this.leftHandVelo.x) > Math.Abs(this.leftHandVelo.y)){
					if(currentLeftHandMotion != HandMotion.LEFT_HAND_WAVE_OUT){
						currentLeftHandMotion = HandMotion.LEFT_HAND_WAVE_OUT;
						motionLeftHandStartTime = currentTime;
						motionStartLeftHandPos = leftHandPos;
					}
				}
				else if(this.leftHandVelo.y > accuracy && Math.Abs(this.leftHandVelo.x) < Math.Abs(this.leftHandVelo.y)){
					if(currentLeftHandMotion != HandMotion.LEFT_HAND_RISE){
						currentLeftHandMotion = HandMotion.LEFT_HAND_RISE;
						motionLeftHandStartTime = currentTime;
						motionStartLeftHandPos = leftHandPos;
					}
				}
				else{
					currentLeftHandMotion = HandMotion.NON;
				}

				// validation of hand motion
				bool rightHandRiseValide = false;
				bool leftHandRiseValide = false;
				// right
				if(currentRightHandMotion == HandMotion.RIGHT_HAND_WAVE_OUT 
				   && currentTime - motionRightHandStartTime <= detectMotionDuration
				   && rightHandPos.x - motionStartRightHandPos.x >= detectMotionDistance){
					// right hand wave out detected
					// rise motion detected event
					OnHandMotionDetected(HandMotion.RIGHT_HAND_WAVE_OUT);
					// reset timer for new detection
					motionRightHandStartTime = currentTime;
					motionStartRightHandPos = rightHandPos;
				}
				else if(currentRightHandMotion == HandMotion.RIGHT_HAND_RISE 
				        && currentTime - motionRightHandStartTime <= detectMotionDuration
				        && rightHandPos.y - motionStartRightHandPos.y >= detectMotionDistance){
					rightHandRiseValide = true;
				}
				// left
				if(currentLeftHandMotion == HandMotion.LEFT_HAND_WAVE_OUT 
				   && currentTime - motionLeftHandStartTime <= detectMotionDuration
				   && motionStartLeftHandPos.x - leftHandPos.x >= detectMotionDistance){
					// left hand wave out detected
					// rise motion detected event
					OnHandMotionDetected(HandMotion.LEFT_HAND_WAVE_OUT);
					// reset timer for new detection
					motionLeftHandStartTime = currentTime;
					motionStartLeftHandPos = leftHandPos;
				}
				else if(currentLeftHandMotion == HandMotion.LEFT_HAND_RISE 
				        && currentTime - motionLeftHandStartTime <= detectMotionDuration
				        && leftHandPos.y - motionStartLeftHandPos.y >= detectMotionDistance){
					leftHandRiseValide = true;
				}
				// double hand
				if(rightHandRiseValide&&leftHandRiseValide){
					// double hands rise detected
					// rise motion detected event
					OnHandMotionDetected(HandMotion.DOUBLE_HAND_RISE);
					motionRightHandStartTime = currentTime;
					motionLeftHandStartTime = currentTime;
					motionStartRightHandPos = rightHandPos;
					motionStartLeftHandPos = leftHandPos;
				}
			}
			rightHandPosPre = rightHandPos;
			leftHandPosPre = leftHandPos;
		}
	}

	// trigger the event handlers
	protected virtual void OnRightHandPositionDetected(RightHandPosition pos) {
		if (rightHandPositionDetected != null) {
			RightHandPositionDetectedEventArgs e = new RightHandPositionDetectedEventArgs();
			e.pos = pos;
			rightHandPositionDetected (this, e);
		}
	}

	// trigger the event handlers
	protected virtual void OnHandMotionDetected(HandMotion motion) {
		if (handMotionDetected != null) {
			HandMotionDetectedEventArgs e = new HandMotionDetectedEventArgs();
			e.motion = motion;
			handMotionDetected (this, e);
		}
	}
}


// Event arguments
public class RightHandPositionDetectedEventArgs : EventArgs
{
	public RightHandPosition pos = RightHandPosition.NON;
}
public class HandMotionDetectedEventArgs : EventArgs
{
	public HandMotion motion = HandMotion.NON;
}
