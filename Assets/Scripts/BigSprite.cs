using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// Takes an array of 'tile' images and constructs a large image from the tiles.
/// </summary>

[ExecuteInEditMode]
public class BigSprite : MonoBehaviour {

	[Tooltip("Click this to reconstruct the BigSprite")]
	public bool refresh = true;

	[Tooltip("Number of images wide")]
	public int width;
	[Tooltip("Number of images high")]
	public int height;

	public string sortingLayerName;
	public int orderInLayer;

	[Tooltip("Array of sprites to use as the tiles. Will be sorted by name automatically")]
	public Sprite[] sprites;
	
	void Update () {
	
		// when refresh is clicked...
		if(refresh){

			// sort the sprites by name, in case they got scrambled when dragged on
			Array.Sort(sprites, delegate(Sprite s1, Sprite s2) {
				return s1.name.CompareTo(s2.name);
			});

			// destroy old tiles
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in transform) {
				children.Add(child.gameObject);
			}
			foreach (GameObject child in children) {
				GameObject.DestroyImmediate(child);
			}

			// Create the tiles

			float spriteWidth = 0, spriteHeight = 0;

			for(int i = 0; i < width * height; i++){
				GameObject sprite = new GameObject();
				sprite.transform.parent = transform;

				SpriteRenderer sr = sprite.AddComponent<SpriteRenderer>();
				sr.sprite = sprites[i];
				if(sortingLayerName.Length > 0){
					sr.sortingLayerName = sortingLayerName;
				}
				sr.sortingOrder = orderInLayer;

				// need to grab the width and height after creating the first sprite
				if(i == 0){
					Renderer renderer = sprite.GetComponent<Renderer>();
					spriteWidth = renderer.bounds.size.x;
					spriteHeight = renderer.bounds.size.y;
				}
					
				float x = spriteWidth * (i % width);
				float y = -spriteHeight * (i / width);
				sprite.transform.localPosition = new Vector3(x, y, 0);
			}

			refresh = false;
		}

	}
}
