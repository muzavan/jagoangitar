using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoviePlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print ("Play");
		((MovieTexture)GetComponentInParent<RawImage> ().texture).Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
