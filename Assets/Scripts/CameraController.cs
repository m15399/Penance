using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	const float SMOOTH_MODE_SNAPPINESS = 6f;
	
	public enum FollowMode {
		/// <summary>
		/// Rigidly follows the target
		/// </summary>
		LOCKED, 
		/// <summary>
		/// 'Tweens' to the target over a short time
		/// </summary>
		SMOOTH
	}

	FollowMode _followMode = FollowMode.LOCKED;
	public FollowMode followMode {
		get {
			return _followMode;
		}
		set {
			_followMode = value;
		}
	}

	// Targets
	// Camera follows the top target on the stack. 
	// Targets can be pushed to the stack to make the camera follow them temporarily.
	public Transform defaultFollowTarget;
	Stack<Transform> followTargets;
	
	void Start () {
		followTargets = new Stack<Transform>();
		followTargets.Push(defaultFollowTarget);
	}

	/// <summary>
	/// Push a new target to be followed temporarily
	/// </summary>
	public void PushTarget(Transform target){
		followTargets.Push(target);
	}

	/// <summary>
	/// Pop the last target pushed
	/// </summary>
	public void PopTarget(){
		followTargets.Pop();
	}
	
	void LateUpdate () {
		Vector3 followTarget = followTargets.Peek().position;

		switch(followMode){
		case FollowMode.LOCKED:
			transform.localPosition = followTarget;
			break;
		case FollowMode.SMOOTH:
			transform.localPosition += (followTarget - transform.localPosition) * SMOOTH_MODE_SNAPPINESS * OurTime.dtTactical;
			break;
		}
	}
}
