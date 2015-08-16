using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour {

	public Sprite[] frames;
	public int fps = 12;


	int currFrame = 0;
	float frameStartTime;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		frameStartTime = OurTime.gameTime;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		float timePassed = OurTime.gameTime - frameStartTime;
		float frameTime = 1.0f/fps;
		if(timePassed > frameTime){
			frameStartTime += frameTime;
			NextFrame();
		}
	}

	void NextFrame(){
		currFrame++;
		if(currFrame >= frames.Length)
			currFrame = 0;

		spriteRenderer.sprite = frames[currFrame];
	}
}
