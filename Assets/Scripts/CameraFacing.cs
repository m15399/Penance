
using UnityEngine;
using System.Collections;

/// <summary>
/// Automatically rotates the object to face the camera
/// </summary>
[ExecuteInEditMode]
public class CameraFacing : MonoBehaviour
{
	public bool reverseFace = false; 

	public bool updateInEditMode = true;

	void  Update ()
	{
		if(!Application.isPlaying && !updateInEditMode)
			return;

		// rotates the object relative to the camera
		Vector3 targetPos = transform.position + Camera.main.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
		Vector3 targetOrientation = Camera.main.transform.rotation * Vector3.up;
		transform.LookAt (targetPos, targetOrientation);

	}
}
