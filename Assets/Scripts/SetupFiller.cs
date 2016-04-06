using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;

public class SetupFiller : MonoBehaviour {
	public float filling = 0.000001f; //filling rate for slider
	public Button doneButton;
	private GameObject[] sliders;
	private bool isFinished = false;
	private int activeString = 0;

	private readonly float[] nilFreq = {82.40f,110.0f,146.80f,196.0f,246.90f,329.6f}; // Frekuensi dasar tiap senar

	private const float freqDif = 6.00f; //karena beda tiap fret 6, jadi rentang setiap nada +- 3
	private readonly string[] relatedString = {"E","A","D","G","B","E2"};

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
		float minFreq = nilFreq [activeString] - (freqDif / 2.0f);
		float maxFreq = nilFreq [activeString] + (freqDif / 2.0f);
		if (minFreq <= freq && freq <= maxFreq) {
			fillSlider (relatedString[activeString]);
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
				if(aSlider.value >= 1.0){
					activeString++;
				}
				break;
			}
		}
		Thread.Sleep (50);
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
