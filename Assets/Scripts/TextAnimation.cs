using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour {

	private Text text;
	private int inc = 1;
	// Use this for initialization
	void Start () {
		text = GetComponentInParent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(text.fontSize >= 45){
			inc = -1;
		}
		else if(text.fontSize <=30){
			inc = 1;
		}
		text.fontSize += inc;
	}
}
