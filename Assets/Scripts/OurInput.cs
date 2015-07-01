using UnityEngine;
using System.Collections;


public static class OurInput {

	/// <summary>
	/// Gets the direction the player is trying to go (e.g. the position of the stick)
	/// </summary>
	public static Vector2 moveVector {
		get {
			Vector2 stick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			// cap magnitude at 1, e.g. if player is holding down and left
			float mag = Mathf.Min(stick.magnitude, 1);
			return stick.normalized * mag;
		}
	}

}
