using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	private float minX = -162;
	private float marginX = 25;
	private float marginY = -15;
	private float maxY = 93;
	public float maxTime = 120.0f;
	public int thisScreenId = 0;
	private bool isFinished = false;
	public int[] fretNumbers = {11,12,11,11,12,14,13,12,11,14,11,12,13,11,12,12,13,12,11,14};
	public int[] stringNumbers = {1,2,3,1,1,2,3,1,1,2,1,2,3,1,1,2,3,1,1,2};
	//private float[] somethings = {110.21,55.18,109.58,27.71,55.18,174.04,55.02,300.48,99.64,98.12,302.85,291.38,74.11,73.12,298.16,519.76,180.57,295.86,155.72,57.58,117.98,110.52,55.26,388.51,55.26,111.48,222.32,110.52,284.9,93.35,272.78,270.86,293.6,280.74,27.73,55.42,427.36,110.84,55.34};

	public Text time;
	private float now = 0.0f;
	private List<float> lists = new List<float> ();
	private int activeFret = 0; //index array dari frequencys yang mau dicocokin

	private readonly float[] nilFreq = {329.60f,246.90f,196.00f,146.80f,110.00f,82.40f}; // Frekuensi dasar tiap senar
	public float freqDif = 6.00f; //karena beda tiap fret 6, jadi rentang setiap nada +- 3

	private Button[] fretButtons; //generated
	public float[] frequencys; //generated
	private float lastFreq=0;

	// Use this for initialization
	void Start () {
		//frets = GameObject.FindGameObjectsWithTag ("Fret");
		time.text = "Time : "+((int)now).ToString()+" s";

		fretButtons = GameObject.FindObjectsOfType<Button>().OrderBy( go => go.name ).ToArray();
		//print (fretButtons.Length == fretNumbers.Length);
		//print (fretButtons.Length == stringNumbers.Length);
		//frequencys = new float[fretNumbers.Length];

		for(int i=0; i<fretNumbers.Length;i++){
			fretButtons [i].GetComponentInChildren<Text> ().text = fretNumbers[i].ToString ();
			//frequencys [i] = nilFreq [stringNumbers [i] - 1] + freqDif * (float)fretNumbers [i];
		}
	}

	// Update is called once per frame
	void Update () {

		now = now + (Time.deltaTime);
		time.text = "Time : "+((int)now).ToString()+ " s";

		if (!isFinished) {
			//print (GetComponent<SensorReader>().currentFrequency);
			checkInput (GetComponent<SensorReader>().currentFrequency);
		} else {
			print ("Game Selesai");
			UnityEngine.SceneManagement.SceneManager.LoadScene (1);
			UnityEngine.SceneManagement.SceneManager.UnloadScene (thisScreenId);
		}
	}

	void checkInput(float freq){
		if ((int)freq != (int)lastFreq && freq != 0) {
			lists.Add (freq);
			lastFreq = freq;
		}

		string printed = "{";
		foreach(float item in lists){
			printed = printed + item.ToString() + ",";
		}
		printed = printed + "}";
		print (printed);
		float minFreq = frequencys [activeFret] - (freqDif / 2.0f);
		float maxFreq = frequencys [activeFret] + (freqDif / 2.0f);
		print ("Butuh Rentang "+minFreq.ToString()+" -- "+maxFreq.ToString()+" Hz");
		print ("Adanya "+freq.ToString()+" Hz");
		if (minFreq <= freq && freq <= maxFreq) {
			fretButtons [activeFret].GetComponent<Image> ().color = Color.green;
			activeFret = activeFret + 1;
		} else {
			// to do kalau salah?
			// sementara do nothing
		}
	}

	void checkFinished(){
		if (activeFret + 1 == fretNumbers.Length) {
			isFinished = true;
		}
		else if(now >= 100.0f){
			isFinished = true;
		}
	}

}
