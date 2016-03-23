using UnityEngine;
using System.Collections;

public class SceneSelector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void ScreenSelect(int screenIndex){
		UnityEngine.SceneManagement.SceneManager.LoadScene (screenIndex);
	}
}
