using UnityEngine;
using System.Collections;


public static class OurTime {

	static float _dtGame;
	/// <summary>
	/// Time passed in the last frame for gameplay things 
	/// </summary>
	public static float dtGame {
		get {
			Update ();
			return _dtGame;
		}
	}

	static float _gameTime;
	/// <summary>
	/// Total time passed for gameplay things
	/// </summary>
	public static float gameTime {
		get {
			Update ();
			return _gameTime;
		}
	}

	public static bool gameplayPaused = false;
	public static float gameplaySpeed = 1f;

	static float _dtTactical;
	/// <summary>
	/// Time passed in the last frame for tactical mode 
	/// </summary>
	public static float dtTactical {
		get {
			Update ();
			return _dtTactical;
		}
	}

	static float _tacticalTime;
	/// <summary>
	/// Total time passed for tactical mode things
	/// </summary>
	public static float tacticalTime {
		get {
			Update ();
			return _tacticalTime;
		}
	}

	public static bool tacticalPaused = false;


	static float _dt;
	/// <summary>
	/// Time passed in the last frame for non-gameplay & non-tactical things (e.g. menus) 
	/// </summary>
	public static float dt {
		get {
			Update ();
			return _dt;
		}
	}

	static float _time = -1;
	/// <summary>
	/// Total time passed for non-gameplay & non-tactical mode things (e.g. menus)
	/// </summary>
	public static float time {
		get {
			Update ();
			return _time;
		}
	}

	/// <summary>
	/// Updates all the private variables if any time has passed
	/// </summary>
	static void Update(){
		if(_time != Time.unscaledTime){

			_time = Time.unscaledTime;
			_dt = Time.unscaledDeltaTime;


			if(_time == -1){
				// first frame
				_gameTime = Time.unscaledTime;
				_tacticalTime = Time.unscaledTime;
				_dtGame = _dt;
				_dtTactical = _dt;

			} else {
				if(!gameplayPaused){
					_dtGame = _dt * gameplaySpeed;
					_gameTime += _dtGame;

				}
				if(!tacticalPaused){
					_dtTactical = _dt;
					_tacticalTime += _dtTactical;
				}
			}


		}
	}

}
