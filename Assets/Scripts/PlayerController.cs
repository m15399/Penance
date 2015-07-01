using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;

	void Start () {
	
	}
	
	void Update () {
		// move player
		transform.localPosition += (Vector3)(OurInput.moveVector * moveSpeed * OurTime.dtGame);
	}
}
