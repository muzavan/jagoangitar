using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class StarlightController : MonoBehaviour {

	public float minX = -162;
	public float marginX = 25;
	public float marginY = -15;
	public float maxY = 93;
	private bool isFinished = false;
	private int[] fretNumbers = {11,12,11,11,12,14,13,12,11,14,11,12,13,11,12,12,13,12,11,14};
	private int[] stringNumbers = {1,2,3,1,1,2,3,1,1,2,1,2,3,1,1,2,3,1,1,2};

	public Text time;
	private float now = 0.0f;
	private int activeFret = 0; //index array dari frequencys yang mau dicocokin

	private readonly float[] nilFreq = {329.60f,246.90f,196.00f,146.80f,110.00f,82.40f}; // Frekuensi dasar tiap senar
	private const float freqDif = 6.00f; //karena beda tiap fret 6, jadi rentang setiap nada +- 3
	
	private Button[] fretButtons; //generated
	private float[] frequencys; //generated

	// Use this for initialization
	void Start () {
		//frets = GameObject.FindGameObjectsWithTag ("Fret");
		time.text = "Time : "+((int)now).ToString()+" s";

		fretButtons = GameObject.FindObjectsOfType<Button>().OrderBy( go => go.name ).ToArray();
		//print (fretButtons.Length == fretNumbers.Length);
		//print (fretButtons.Length == stringNumbers.Length);
		frequencys = new float[fretButtons.Length];

		for(int i=0; i<fretButtons.Length;i++){
			fretButtons [i].GetComponentInChildren<Text> ().text = fretNumbers[i].ToString ();
			frequencys [i] = nilFreq [stringNumbers [i] - 1] + freqDif * (float)fretNumbers [i];
		}
	}
	
	// Update is called once per frame
	void Update () {

		now = now + (Time.deltaTime);
		time.text = "Time : "+((int)now).ToString()+ " s";

		if (!isFinished) {
			// to do baca input yang beneran
			if(Input.GetKeyDown("space")){
				checkInput (frequencys [activeFret]);
			}

		} else {

		}
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
		if (activeFret + 1 == fretButtons.Length) {
			print ("Game Selesai");
		}
	}
		
}
