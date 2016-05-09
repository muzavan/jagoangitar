using UnityEngine;
using System.Collections;

public class OnClickSound : MonoBehaviour {

	// Use this for initialization
	public AudioSource aSource;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Clicked(){
		aSource.Play ();
	}
}
