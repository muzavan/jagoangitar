using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

public class BestTimeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int mode = PlayerPrefs.GetInt ("PlayMode",0);
		//Button[] bestTimes = (Button[])GameObject.FindGameObjectsWithTag ("BestTime").OrderBy (go => go.name).ToArray ();
		GameObject[] bests = GameObject.FindGameObjectsWithTag("BestTime").OrderBy (go => go.name).ToArray ();
		Button[] bestTimes = new Button[bests.Length];
		for(int i=0; i<bests.Length;i++){
			bestTimes [i] = bests [i].GetComponent<Button> ();
			//print ("Name : "+bestTimes[i].name);
		}
		if (mode == 0) {
			for (int i = 0; i < bestTimes.Length; i++) {
				bestTimes [i].gameObject.SetActive (false);
			}
		} else {
			for (int i = 0; i < bestTimes.Length; i++) {
				//Set each level to associated value
				if(PlayerPrefs.HasKey("Level"+i)){
					bestTimes [i].GetComponentInChildren<Text> ().text = PlayerPrefs.GetInt ("Level"+i) + " s";
					bestTimes [i].GetComponent<Image>().color = new Color (12.0f / 255.0f, 234.0f / 255.0f, 77.0f / 255.0f, 157.0f / 255.0f);
				}

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
