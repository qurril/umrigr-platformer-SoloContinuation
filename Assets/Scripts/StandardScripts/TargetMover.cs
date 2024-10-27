using UnityEngine;
using System.Collections;

public class TargetMover : MonoBehaviour {

	// define the possible states through an enumeration
	public enum motionDirections {Horizontal, Vertical};
	
	// store the state
	public motionDirections motionState = motionDirections.Horizontal;

	// motion parameters
	public float moveSpeed = 180.0f;
	public float motionMagnitude = 0.1f;

	// Update is called once per frame
	void Update () {

		// do the appropriate motion based on the motionState
		switch(motionState) {
			case motionDirections.Horizontal:
				//Move the object horizontally along the x axis taking in the account the motionMagnitude and the speed, if the object comes to the end of the screen it will move in the opposite direction
				
				//gameObject.transform.Translate(Vector3.right * Mathf.Cos(Time.timeSinceLevelLoad) * motionMagnitude);
				break;
			case motionDirections.Vertical:
				gameObject.transform.Translate(Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad) * motionMagnitude);
				break;
		}
	}
}
