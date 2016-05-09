using UnityEngine;
using System.Collections;

public class MusicSingleton : MonoBehaviour {

	private static MusicSingleton instance;
	public AudioSource aSource;
	public static MusicSingleton Instance {
		get {return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			if(!instance.aSource.isPlaying){
				instance.aSource.Play ();
			}
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
			if(!instance.aSource.isPlaying){
				instance.aSource.Play ();
			}
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
