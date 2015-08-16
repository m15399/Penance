using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;

	void Start () {
	
	}
	
	void Update () {
		// move player
		transform.localPosition += (Vector3)(OurInput.moveVector * moveSpeed * OurTime.dtGame);

		// quick code to reverse visual when going left
		Transform visual = transform.GetChild(0);
		if(OurInput.moveVector.x > 0)
			visual.localScale = new Vector3(1, 1, 1);
		else if (OurInput.moveVector.x < 0)
			visual.localScale = new Vector3(-1, 1, 1);
	}
}
