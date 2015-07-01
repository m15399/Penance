using UnityEngine;
using System.Collections;

/// <summary>
/// Used for physics. GameObjects with this script will automatically check for collisions
/// and be moved when pushed, based on their mass. The object should have either a CircleCollider2D 
/// or a BoxCollider2D on it. 
/// </summary>
public class OurCollider : MonoBehaviour {

	/// <summary>
	/// Push objects a little extra to avoid them getting stuck to each other
	/// </summary>
	const float EXTRA_PUSH = .001f;

	public static Vector3 TrueCenter (Collider2D collider){
		Vector3 pos = collider.gameObject.transform.position;
		Vector3 scale = collider.gameObject.transform.lossyScale; // use the FINAL scale, not local

		float xOffset = collider.offset.x * scale.x;
		float yOffset = collider.offset.y * scale.y;

		return pos + new Vector3(xOffset, yOffset, 0);
	}
	
	public static Vector3 TrueSize (BoxCollider2D collider){
		return new Vector3(collider.size.x * collider.transform.lossyScale.x, 
		                   collider.size.y * collider.transform.lossyScale.y, 1);
	}

	public static Vector3 TrueSize (CircleCollider2D collider){
		float d = BoundingRadius(collider) * 2;
		return new Vector3(d, d, 1);
	}

	public static float BoundingRadius (BoxCollider2D collider){
		return TrueSize(collider).magnitude / 2;
	}

	public static float BoundingRadius (CircleCollider2D collider){
		return collider.radius * collider.transform.lossyScale.x;
	}


	static float lastTime = -1;
	static OurCollider[] currColliders = null;
	/// <summary>
	/// Gets all instances of OurCollider that are active on the current frame
	/// </summary>
	static OurCollider[] collidersForFrame {
		get {
			if(Time.time != lastTime){
				currColliders = FindObjectsOfType<OurCollider>();
				lastTime = Time.time;
			}
			return currColliders;
		}
	}


	Collider2D _collider = null;
	new public Collider2D collider {
		get {
			if(_collider == null)
				_collider = GetComponent<Collider2D>();
			return _collider;
		}
	}

	/// <summary>
	/// Gets the circle collider, or null if the collider isn't a CircleCollider2D.
	/// </summary>
	public CircleCollider2D circleCollider {
		get {
			return collider as CircleCollider2D;
		}
	}

	/// <summary>
	/// Gets the box collider, or null if the collider isn't a BoxCollider2D.
	/// </summary>
	public BoxCollider2D boxCollider {
		get {
			return collider as BoxCollider2D;
		}
	}

	public Vector3 center {
		get {
			return TrueCenter(collider);
		}
	}

	public float boundingRadius {
		get {
			if(circleCollider != null)
				return BoundingRadius(circleCollider);
			else
				return BoundingRadius(boxCollider);
		}
	}

	public Bounds bounds {
		get {
			return new Bounds(center, size);
		}
	}

	public Vector3 size {
		get {
			if(circleCollider != null)
				return TrueSize(circleCollider);
			else
				return TrueSize(boxCollider);
		}
	}

	[Tooltip("How heavy the object is. Heavier objects are harder to push")]
	public float mass = 1;

	[Tooltip("Immoveable objects cannot be pushed. Used for walls, buildings, etc")]
	public bool immoveable = false;

	bool IsCollidingWith (OurCollider other){

		float r1 = boundingRadius, r2 = other.boundingRadius;

		Vector2 relation = other.center - center;
		float distance = relation.magnitude;
		float desiredDistance = r1 + r2;

		// bounding radius intersects && bounding box intersects = collision
		return distance < desiredDistance && bounds.Intersects(other.bounds);
	}

	void ResolveBoxVsBox (OurCollider box, OurCollider box2, ref float pushAmt, ref Vector3 pushDir){

		Bounds b1 = box.bounds;
		Bounds b2 = box2.bounds;

		Vector2 relation = b2.center - b1.center;
		Vector2 desired = b1.extents + b2.extents;

		float overlapHorizontal = Mathf.Abs(relation.x) - desired.x;
		float overlapVertical = Mathf.Abs(relation.y) - desired.y;

		if(overlapHorizontal < overlapVertical){
			// vertical collision
			pushAmt = overlapVertical;
			if(relation.y < 0)
				pushDir = new Vector3(0, -1, 0);
			else
				pushDir = new Vector3(0, 1, 0);
		} else {
			// horizontal collision
			pushAmt = overlapHorizontal;
			if(relation.x < 0)
				pushDir = new Vector3(-1, 0, 0);
			else
				pushDir = new Vector3(1, 0, 0);
		}
	}

	void ResolveCircleVsBox (OurCollider circle, OurCollider box, ref float pushAmt, ref Vector3 pushDir){
		Vector3 relation = box.bounds.ClosestPoint(circle.center) - circle.center;

		pushDir = relation.normalized;
		pushAmt = circle.boundingRadius - relation.magnitude;

	}

	void ResolveCircleVsCircle (OurCollider circle1, OurCollider circle2, ref float pushAmt, ref Vector3 pushDir){
		Vector2 relation = circle2.center - circle1.center;
		float distance = relation.magnitude;
		float desiredDistance = circle1.boundingRadius + circle2.boundingRadius;

		pushAmt = desiredDistance - distance;
		pushDir = -(Vector3)relation.normalized;
	}


	void ResolveCollision (OurCollider other){ 

		if(IsCollidingWith(other)){

			// amount to push objects apart
			float pushAmt = 0;
			Vector3 pushDir = new Vector3();

			// igure out the collision type and resolve it

			if(boxCollider != null){ // we are a box
			
				if(other.boxCollider != null){ // both boxes
					ResolveBoxVsBox(this, other, ref pushAmt, ref pushDir);
				
				} else { // we are a box, they are a circle
					ResolveCircleVsBox(other, this, ref pushAmt, ref pushDir);
				}
			} else {

				if(other.boxCollider != null){ // we are a circle, they are a box
					ResolveCircleVsBox(this, other, ref pushAmt, ref pushDir);
					pushAmt *= -1; // since we're the second object, we have to reverse the push
				
				} else { // both circles
					ResolveCircleVsCircle(this, other, ref pushAmt, ref pushDir);
				}
			}


			// figure out how much to push each object
			// object with less mass will get pushed harder
			float ourAmt = other.mass / (mass + other.mass);
			float otherAmt = 1 - ourAmt;

			// check for immoveable objects
			if(immoveable){
				ourAmt = 0;
				otherAmt = 1;
				if(other.immoveable){ // both immoveable?
					otherAmt = 0;
				}
			} else if (other.immoveable){
				otherAmt = 0;
				ourAmt = 1;
			}

			// push both objects
			pushAmt += EXTRA_PUSH;
			transform.position += pushDir * pushAmt * ourAmt;
			other.transform.position -= pushDir * pushAmt * otherAmt; 

		}
	}

	void LateUpdate(){

		// Check collisions against all other colliders
		OurCollider[] colliders = collidersForFrame;
		int l = colliders.Length;

		for(int i = 0; i < l; i++){
			// skip ourself
			if(colliders[i] != this){
				ResolveCollision(colliders[i]);
			}
		}
	}
}
