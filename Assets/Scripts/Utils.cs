using UnityEngine;
using System.Collections;

public static class Utils {

	public static void DebugRect(Vector3 center, Vector3 size){
		size = new Vector3(size.x, size.y, 0);
		Vector3 ul = center - size/2;
		Vector3 ur = ul + new Vector3(size.x, 0, 0);
		Vector3 ll = ul + new Vector3(0, size.y, 0);
		Vector3 lr = ul + size;
		Debug.DrawLine(ul, ur);
		Debug.DrawLine(ur, lr);
		Debug.DrawLine(lr, ll);
		Debug.DrawLine(ll, ul);
	}

}
