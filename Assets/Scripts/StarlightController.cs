﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class StarlightController : MonoBehaviour {

	public float maxTime = 120.0f;
	private bool isFinished = false;
	public int[] fretNumbers = {11,12,11,11,12,14,13,12,11,14,11,12,13,11,12,12,13,12,11,14};
	public int[] stringNumbers = {1,2,3,1,1,2,3,1,1,2,1,2,3,1,1,2,3,1,1,2};

	public Text time;
	private float now = 0.0f;
	private int activeFret = 0; //index array dari frequencys yang mau dicocokin

	private readonly float[] nilFreq = {329.60f,246.90f,196.00f,146.80f,110.00f,82.40f}; // Frekuensi dasar tiap senar
	private const float freqDif = 6.00f; //karena beda tiap fret 6, jadi rentang setiap nada +- 3

	private Button[] fretButtons; //generated
	private float[] frequencys; //generated
	private int thisScreenId = 3;

	// Use this for initialization
	void Start () {
		//frets = GameObject.FindGameObjectsWithTag ("Fret");
		GameObject.FindGameObjectWithTag("SuccessButton").SetActive(false);
		GameObject.FindGameObjectWithTag("FailedButton").SetActive(false);
		time.text = "Time : "+((int)now).ToString()+" s";

		fretButtons = GameObject.FindObjectsOfType<Button>().OrderBy( go => go.name ).ToArray();
		//print (fretButtons.Length == fretNumbers.Length);
		//print (fretButtons.Length == stringNumbers.Length);
		frequencys = new float[fretNumbers.Length];

		for(int i=0; i<fretNumbers.Length;i++){
			fretButtons [i].GetComponentInChildren<Text> ().text = fretNumbers[i].ToString ();
			frequencys [i] = nilFreq [stringNumbers [i] - 1] + freqDif * (float)fretNumbers [i];
		}
	}

	// Update is called once per frame
	void Update () {

		now = now + (Time.deltaTime);
		float xnow = now;
		if(PlayerPrefs.GetInt("PlayMode") == 1){
			xnow = maxTime - now;
		}
		time.text = "Time : "+((int)xnow).ToString()+ " s";

		if (!isFinished) {
			checkInput (GetComponent<SensorReader>().currentFrequency);
		} else {
			print ("Game Selesai");
			ScreenSelect (1);
		}
		checkFinished ();
	}

	void checkInput(float freq){
		float minFreq = frequencys [activeFret] - (freqDif / 2.0f);
		float maxFreq = frequencys [activeFret] + (freqDif / 2.0f);
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
			if(PlayerPrefs.GetInt("PlayMode")==1){
				int activeLevel = PlayerPrefs.GetInt ("ActiveLevel");
				if (PlayerPrefs.HasKey ("Level" + activeLevel)) {
					int btime = PlayerPrefs.GetInt ("Level" + activeLevel);
					int nowTime = (int)(maxTime - now);
					if (nowTime > btime) {
						PlayerPrefs.SetInt ("Level" + activeLevel, nowTime);
					} else {
						// Do Nothing
					}
				} else {
					int nowTime = (int)(maxTime - now);
					PlayerPrefs.SetInt ("Level"+activeLevel,(int) nowTime);

				}

			}
			GameObject.FindGameObjectWithTag("SuccessButton").SetActive(true);
		}
		else if((now >= maxTime) && (PlayerPrefs.GetInt("PlayMode")==1)){
			isFinished = true;
			GameObject.FindGameObjectWithTag("FailedButton").SetActive(true);
		}
	}

	public void ScreenSelect(int screenIndex){
		StartCoroutine (DelayStop(screenIndex));
	}

	IEnumerator DelayStop(int screenIndex){
		yield return new WaitForSeconds (1);
		Debug.Log ("ScreenSelect("+screenIndex+")");
		UnityEngine.SceneManagement.SceneManager.LoadScene (screenIndex);
		UnityEngine.SceneManagement.SceneManager.UnloadScene (thisScreenId);
		Debug.Log ("ScreenCount = "+UnityEngine.SceneManagement.SceneManager.sceneCount);
	}

}
