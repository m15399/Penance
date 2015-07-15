using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	protected GameManager(){}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate(){

		// Check collisions between colliders
		OurCollider[] colliders = OurCollider.collidersForFrame;
		int l = colliders.Length;
		List<OurCollider> didCollide = new List<OurCollider>();
		
		for(int i = 0; i < l; i++){
			for(int j = 0; j < l; j++){
				// skip collider vs self
				if(i != j){
					bool collided = colliders[i].ResolveCollision(colliders[j]);
					if(collided){
						didCollide.Add(colliders[i]);
						didCollide.Add(colliders[j]);
					}
				}
			}
		}

		// check again on the colliders that hit something
		// greatly improves fidelity
		int l2 = didCollide.Count;
		for(int i = 0; i < l2; i++){
			for(int j = 0; j < l2; j++){
				if(i != j){
					didCollide[i].ResolveCollision(didCollide[j]);
				}
			}
		}
	}
}
