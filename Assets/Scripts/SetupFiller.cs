using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

public class SetupFiller : MonoBehaviour {
	public float filling = 0.1f; //filling rate for slider
	public Button doneButton;
	private GameObject[] sliders;
	private bool isFinished = false;
	// Use this for initialization
	void Start () {
		sliders = GameObject.FindGameObjectsWithTag ("String");
		doneButton.interactable = false;
		//print (sliders.Length);
	}
	
	// Update is called once per frame
	void Update () {
		handleInput ();	
		if(checkIsFinished()){
			Finishing ();
		}
	}

	void handleInput(){
		if (Input.GetKeyDown ("e")) {
			//print (sliders.Length);
			fillSlider ("E");
		} else if(Input.GetKeyDown ("a")){
			fillSlider ("A");
		} else if(Input.GetKeyDown ("d")){
			fillSlider ("D");
		} else if(Input.GetKeyDown ("g")){
			fillSlider ("G");
		} else if(Input.GetKeyDown ("b")){
			fillSlider ("B");
		} else if(Input.GetKeyDown ("space")){
			fillSlider ("E2");
		}
	}

	void fillSlider(string sliderName){
		//Possible Input : E,A,D,G,B,E2
		foreach (GameObject slider in sliders) {
			//print (slider.name);
			if(slider.name == sliderName){
				Slider aSlider = slider.GetComponentInChildren<Slider> ();
				//print (aSlider.name);
				aSlider.value += filling;
				//print (aSlider.value);
				break;
			}
		}
	}

	bool checkIsFinished(){
		foreach (GameObject slider in sliders) {
			Slider aSlider = slider.GetComponentInChildren<Slider> ();
			if(aSlider.value < aSlider.maxValue){
				return false;
			}
		}
		return true;
	}

	void Finishing(){
		//Button tmpButton = Instantiate (doneButton);
		if(!isFinished){
			doneButton.interactable = true;
		}

	}
}
