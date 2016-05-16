using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System;

public class LevelController : MonoBehaviour {

	public string fileName;
	public float distanceX = 54.0f;
	public float distanceY = -33.0f;
	public float maxTime = 120.0f;
	public int sleep = 100;
	public int thisScreenId = 0;
	private const int STRING = 6;
	private bool isFinished = false;
	private bool toBeChanged = false;
	public List<int[]> fretNumbers = new List<int[]> ();
	public List<int[]> stringNumbers = new List<int[]>();

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
	private int activeTab = 0;
	private int[] fretNumInTab;

	private Button[] fretButtons; //generated
	public MinMaxFreq[] frequencys; //generated
	private GameObject sButton, fButton;

	// Use this for initialization
	void Start () {

		LoadFile ();

		MusicSingleton.Stop();
		//print (GameObject.FindGameObjectWithTag("FailedButton").name);
		sButton = GameObject.FindGameObjectWithTag("SuccessButton");
		fButton = GameObject.FindGameObjectWithTag("FailedButton");

		time.text = "Time : "+((int)now).ToString()+" s";

		fretButtons = GameObject.FindObjectsOfType<Button>().OrderBy( go => go.name ).ToArray();

		fretNumInTab = new int[fretNumbers.Count];
		for(int j = 0; j< fretNumbers.Count;j++){
			if (j == 0) {
				fretNumInTab [j] = fretNumbers [j].Length;
			} else {
				fretNumInTab [j] = fretNumbers [j].Length + fretNumInTab[j-1];
			}
		}

		frequencys = new MinMaxFreq[fretNumInTab[fretNumInTab.Length-1]];

		for(int j=0; j < fretNumbers.Count;j++){

			for(int i=0; i<fretNumbers[j].Length;i++){
				int padding = 0;
				if (j != 0) {
					padding = fretNumInTab [j-1];
				}
				fretButtons [padding+i].GetComponentInChildren<Text> ().text = fretNumbers[j][i].ToString ();
				//print ("position : "+fretButtons[padding+i].transform.position.x);

				// place fret to designed place

				fretButtons [padding + i].GetComponent<RectTransform> ().position = fretButtons [padding + i].GetComponent<RectTransform> ().position + /*new Vector3(54.0f,-32.0f);*/ new Vector3 ((distanceX * i), (distanceY * (stringNumbers [j] [i]-1)));
				fretButtons [padding + i].gameObject.SetActive (false);
				//toBeChanged = true;


				// Initialize all needed frequencys
				int tmp = fretNumbers [j][i];
				tmp = tmp + (STRING - stringNumbers [j][i]) * 5;
				if(stringNumbers[j][i] <= 2){
					tmp = tmp - 1;
				}

				//print (tmp);
				if (tmp == 0) {
					frequencys [padding+i].min = 79.0f;
					frequencys [padding+i].max = (teoFreqs [tmp] + teoFreqs [tmp+1]) / 2.0f;  
				} else {
					// TODO kalau tmp nya nilai string max
					//print (tmp);
					//print (tmp-1);
					//print (tmp-i);
					frequencys [padding+i] = new MinMaxFreq ();
					frequencys [padding+i].min = (teoFreqs [tmp] + teoFreqs [tmp - 1]) / 2.0f;
					frequencys [padding+i].max = (teoFreqs [tmp + 1] + teoFreqs [tmp]) / 2.0f;
				}
			}
		}

		int initial = fretNumbers.Count >= 2 ? 1 : 0;
		for(int i = 0;i < fretNumInTab[initial];i++){
			fretButtons [i].gameObject.SetActive (true);
		}



	}

	// Update is called once per frame
	void Update () {

		//print ("Active : " + activeFret);
		//print ("Position : " + fretButtons[activeFret].gameObject.GetComponent<RectTransform>().position);
		now = now + (Time.deltaTime);
		float xnow = now;
		if(PlayerPrefs.GetInt("PlayMode") == 1){
			xnow = maxTime - now;
			if(xnow < 0){
				xnow = 0;
			}
		}
		time.text = "Time : "+((int)xnow).ToString()+ " s";

		//cek what fret should be shown
		if(activeTab + 1 < fretNumInTab.Length && toBeChanged){

			print ("Hide : " + activeFret);
			for(int i = 0 ;i < activeFret;i++){
				fretButtons [i].gameObject.SetActive (false);
			}

			print (" Shown : " + activeTab.ToString() + " " + (activeTab+1).ToString());
			for(int i = fretNumInTab[activeTab];i < fretNumInTab[activeTab+1];i++){
				fretButtons [i].gameObject.SetActive (true);
			}
			toBeChanged = false;
		}

		if (!isFinished) {
			////print (GetComponent<SensorReader>().currentFrequency);
			checkInput (GetComponent<SensorReader>().currentFrequency);

			/*if(Input.GetKeyDown("space")){
				checkInput(frequencys [activeFret].min + 0.001f);
			}*/

		} else {
			//print ("Game Selesai");
			ScreenSelect(1);
		}
	}

	void checkInput(float freq){
		float minFreq = frequencys [activeFret].min;
		float maxFreq = frequencys [activeFret].max;
		//print ("Butuh Rentang "+minFreq.ToString()+" -- "+maxFreq.ToString()+" Hz");
		if (minFreq <= freq && freq <= maxFreq) {
			fretButtons [activeFret].GetComponent<Image> ().color = Color.green;
			Thread.Sleep (sleep);
			activeFret = activeFret + 1;
			if(activeFret == fretNumInTab[activeTab]){
				activeTab++;
				print ("Active Tab : " + activeTab);
				toBeChanged = true;
				checkFinished ();
			}
		} else {
			// to do kalau salah?
			// sementara do nothing
		}
	}

	void checkFinished(){
		//print ("Finished : " + activeFret + " " + fretNumInTab[fretNumInTab.Length-1]);
		if (activeFret >= fretNumInTab[fretNumInTab.Length-1]) {
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

			sButton.SetActive(true);
			print ("GameSelesai");
		}
		else if((now >= maxTime) && (PlayerPrefs.GetInt("PlayMode")==1)){
			isFinished = true;
			fButton.SetActive(true);
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

	void LoadFile(){
		try
        {   // Open the text file using a stream reader.
			int counter = 0;
			string line;

			// Read the file and display it line by line.
			StreamReader file = new StreamReader("Assets/Levels/"+fileName);

			// Baca Fret Numbers
			while((line = file.ReadLine()) != "###")
			{
				counter++;
				int[] frets;
				string length = line.Split(' ')[0]; //get fret number next line
				int fretnum = 0;
				if(Int32.TryParse(length,out fretnum)){
					// do nothing
				}
				else{
					print("Error on Line : #" + counter);
					break;
				}
				frets = new int[fretnum];
				line = file.ReadLine();
				string[] digits = line.Split(' ');

				// Parse Input to Fret Numbers
				for(int i=0;i<frets.Length;i++){
					int x = 0;
					if(Int32.TryParse(digits[i],out x)){
						// do nothing
					}
					frets[i] = x;
				}

				print("Line "+counter.ToString()+" : "+digits.Length.ToString());
				fretNumbers.Add(frets);
			}

			// Baca String Numbers
			while((line = file.ReadLine()) != "###")
			{
				counter++;
				int[] strings;
				string length = line.Split(' ')[0]; //get fret number next line
				int stringnum = 0;
				if(Int32.TryParse(length, out stringnum)){
					// do nothing
				}
				else{
					print("Error on Line : #" + counter);
					break;
				}
				strings = new int[stringnum];
				line = file.ReadLine();
				string[] digits = line.Split(' ');

				// Parse Input to Fret Numbers
				for(int i=0;i<strings.Length;i++){
					int x = 0;
					if(Int32.TryParse(digits[i], out x)){
						// do nothing
					}
					strings[i] = x;
				}

				print("Line "+counter.ToString()+" : "+digits.Length.ToString());
				stringNumbers.Add(strings);
			}

			file.Close();

			// Suspend the screen.
        }
        catch (Exception e)
        {
            print("The file could not be read:");
            print(e.Message);
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
