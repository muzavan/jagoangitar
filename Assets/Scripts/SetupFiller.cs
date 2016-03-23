using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetupFiller : MonoBehaviour {
	public float filling = 0.1f; //filling rate for slider
	public Button doneButton;
	private GameObject[] sliders;
	private bool isFinished = false;

	private readonly float[] nilFreq = {329.60f,246.90f,196.00f,146.80f,110.00f,82.40f}; // Frekuensi dasar tiap senar
	private const float freqDif = 6.00f; //karena beda tiap fret 6, jadi rentang setiap nada +- 3
	private readonly string[] relatedString = {"E2","B","G","D","A","E"};

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
		float freq = GetComponent<SensorReader> ().currentFrequency;
		for(int i = 0; i<nilFreq.Length;i++){
			float minFreq = nilFreq [i] - (freqDif / 2.0f);
			float maxFreq = nilFreq [i] + (freqDif / 2.0f);
			if (minFreq <= freq && freq <= maxFreq) {
				fillSlider (relatedString[i]);
				break;
			} 
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
