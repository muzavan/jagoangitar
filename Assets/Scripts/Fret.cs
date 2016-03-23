using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fret : MonoBehaviour {

	public enum State
	{
		NotActive,
		Incorrect,
		Correct
	};
	public int fretNumber = 0;
	public int stringNumber = 1;
	private int yLoc = 0;
	private double frequency;
	private State state = State.NotActive;
	private readonly double[] nilFreq = {82.40 , 110.00 , 146.80, 196.00, 246.90,329.60}; // Frekuensi dasar tiap senar
	private const double freqDif = 6.00; //karena beda tiap fret 6, jadi rentang setiap nada +- 3

	// Use this for initialization
	void Start () {
		//yLoc = stringNumber * (-15);
		this.gameObject.transform.position.Set (this.gameObject.transform.position.x,((float)stringNumber*15),this.gameObject.transform.position.z);
		frequency = nilFreq [stringNumber - 1] + (double)(fretNumber) * (freqDif);
		Text fretText = gameObject.GetComponentInChildren<Text> ();
		fretText.text = fretNumber.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.Correct){
			//ubah warna jadi hijau
			Image button = this.GetComponent<Image> ();
			button.color = Color.green;
		}
			
	}

	void checkFret(double freq){
		if (Mathf.Abs (Mathf.RoundToInt((float)(freq - frequency))) <= (freqDif / 2)) {
			state = State.Correct;
		}
	}


}
