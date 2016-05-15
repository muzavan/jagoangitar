using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public float maxTime = 120.0f;
	public int thisScreenId = 0;
	private const int STRING = 6;
	private bool isFinished = false;
	public int[] fretNumbers = {11,12,11,11,12,14,13,12,11,14,11,12,13,11,12,12,13,12,11,14};
	public int[] stringNumbers = {1,2,3,1,1,2,3,1,1,2,1,2,3,1,1,2,3,1,1,2};
	
	// theoretically, needed frequencys for each tune
	private readonly float[] teoFreqs = {
		82.41f,87.31f,92.50f,98.00f,103.83f, //string-6 fret 0-4
		110.00f,116.54f,123.47f,130.81f,138.59f, //string-5 fret 0-4
		146.83f,155.56f,164.81f,174.61f,185.00f, //string-4 fret 0-4
		196.00f,207.65f,220.00f,233.08f, //string-3 fret 0-3
		246.94f,261.63f,277.18f,293.66f,311.13f, //string-2 fret 0-4
		329.63f,349.23f,369.99f,392.00f,415.30f, //string-1 fret 0-4
		440.00f,466.16f,493.88f,523.25f,554.37f, //string-1 fret 5-9
		587.33f,622.25f,659.25f,698.46f,739.99f, //string-1 fret 10-14
		783.99f,830.61f,880.00f,932.33f,987.77f, //string-1 fret 15-19
	};

	public Text time;
	private float now = 0.0f;
	private int activeFret = 0; //index array dari frequencys yang mau dicocokin

	private Button[] fretButtons; //generated
	private MinMaxFreq[] frequencys; //generated

	// Use this for initialization
	void Start () {
		//frets = GameObject.FindGameObjectsWithTag ("Fret");
		time.text = "Time : "+((int)now).ToString()+" s";

		fretButtons = GameObject.FindObjectsOfType<Button>().OrderBy( go => go.name ).ToArray();
		frequencys = new MinMaxFreq[fretNumbers.Length];

		for(int i=0; i<fretNumbers.Length;i++){
			
			fretButtons [i].GetComponentInChildren<Text> ().text = fretNumbers[i].ToString ();
			//print (fretButtons[i].name.ToString() + " " +fretButtons [i].GetComponentInChildren<Text> ().text);

			// Initialize all needed frequencys
			int tmp = fretNumbers [i];
			tmp = tmp + (STRING - stringNumbers [i]) * 5;
			if(stringNumbers[i] <= 2){
				tmp = tmp - 1;
			}

			//print (tmp);
			if (tmp == 0) {
				frequencys [i].min = 79.0f;
				frequencys [i].max = (teoFreqs [tmp] + teoFreqs [tmp+1]) / 2.0f;  
			} else {
				// TODO kalau tmp nya nilai string max
				//print (tmp);
				//print (tmp-1);
				//print (tmp-i);
				frequencys [i] = new MinMaxFreq ();
				frequencys [i].min = (teoFreqs [tmp] + teoFreqs [tmp - 1]) / 2.0f;
				frequencys [i].max = (teoFreqs [tmp + 1] + teoFreqs [tmp]) / 2.0f;
			}
		}
	}

	// Update is called once per frame
	void Update () {

		now = now + (Time.deltaTime);
		time.text = "Time : "+((int)now).ToString()+ " s";

		if (!isFinished) {
			print (GetComponent<SensorReader>().currentFrequency);
			checkInput (GetComponent<SensorReader>().currentFrequency);
		} else {
			print ("Game Selesai");
			UnityEngine.SceneManagement.SceneManager.LoadScene (1);
			UnityEngine.SceneManagement.SceneManager.UnloadScene (thisScreenId);
		}
		checkFinished ();

	}

	void checkInput(float freq){
		float minFreq = frequencys [activeFret].min;
		float maxFreq = frequencys [activeFret].max;
		//print ("Butuh Rentang "+minFreq.ToString()+" -- "+maxFreq.ToString()+" Hz");
		if (minFreq <= freq && freq <= maxFreq) {
			fretButtons [activeFret].GetComponent<Image> ().color = Color.green;
			activeFret = activeFret + 1;
		} else {
			// to do kalau salah?
			// sementara do nothing
		}
	}

	void checkFinished(){
		if (activeFret + 1 > fretNumbers.Length) {
			isFinished = true;
			if(PlayerPrefs.GetInt("PlayMode") == 1){
				int activeLevel = PlayerPrefs.GetInt ("ActiveLevel");
				PlayerPrefs.SetInt ("Level"+activeLevel,(int)now);
			}

		}
		else if((now >= maxTime) && (PlayerPrefs.GetInt("PlayMode") == 1)){
			isFinished = true;
		}
	}

	public class MinMaxFreq {
		public float min;
		public float max;

		public MinMaxFreq(){
			min = 0.0f;
			max = 0.0f;
		}
	}

}
