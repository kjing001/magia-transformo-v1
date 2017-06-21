using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellResult : MonoBehaviour {
	private Manager manager;
	//private Text resultText;

	// Use this for initialization
	void Start () {
		manager = GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ();
		//resultText = GameObject.FindGameObjectWithTag ("Result").GetComponent<Text> ();
		Debug.Log ("You cast:\n" + manager.GetSpell ());
	}

	public void Reset() {
		manager.OnReset ();
	}
}
