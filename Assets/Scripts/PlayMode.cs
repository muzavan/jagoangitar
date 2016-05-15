using UnityEngine;
using System.Collections;

public class PlayMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setPlayMode(int mode){
		PlayerPrefs.SetInt ("PlayMode",mode); // Mode {0 : training, 1 : play}
	}

	public void setActiveLevel(int activeLevel){
		PlayerPrefs.SetInt ("ActiveLevel",activeLevel);
	}
}
