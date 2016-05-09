using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoviePlayer : MonoBehaviour {

	private AudioSource aSource;
	public AudioClip aClip;
	// Use this for initialization
	void Start () {
		((MovieTexture)GetComponentInParent<RawImage> ().texture).Play();
		aSource = GetComponentInParent<AudioSource> ();
		aSource.clip = aClip;
		aSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
