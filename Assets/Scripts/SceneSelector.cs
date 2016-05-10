using UnityEngine;
using System.Collections;
using System.Threading;

public class SceneSelector : MonoBehaviour {

	public int thisScreenId = 0;
	public AudioSource aSource;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ScreenSelect(int screenIndex){
		if(aSource != null){
			aSource.Play ();
		}
		StartCoroutine (DelayStop(screenIndex));
	}

	IEnumerator DelayStop(int screenIndex){
		yield return new WaitForSeconds (1);
		Debug.Log ("ScreenSelect("+screenIndex+")");
		UnityEngine.SceneManagement.SceneManager.LoadScene (screenIndex);
		UnityEngine.SceneManagement.SceneManager.UnloadScene (thisScreenId);
		Debug.Log ("ScreenCount = "+UnityEngine.SceneManagement.SceneManager.sceneCount);
	}
		
}
