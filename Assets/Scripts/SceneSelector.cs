using UnityEngine;
using System.Collections;

public class SceneSelector : MonoBehaviour {

	public int thisScreenId = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ScreenSelect(int screenIndex){
		Debug.Log ("ScreenSelect("+screenIndex+")");
		UnityEngine.SceneManagement.SceneManager.LoadScene (screenIndex);
		UnityEngine.SceneManagement.SceneManager.UnloadScene (thisScreenId);
		Debug.Log ("ScreenCount = "+UnityEngine.SceneManagement.SceneManager.sceneCount);
	}
}
