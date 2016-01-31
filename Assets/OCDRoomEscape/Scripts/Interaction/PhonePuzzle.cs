using UnityEngine;
using System.Collections;

public class PhonePuzzle : Puzzle 
{	
	private bool isPhoneOnHook = false;
	private bool hasPhoneBeenPutOnHook = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
		bool isPhonePutOnHook = false;
		if (isPhoneOnHook != isPhonePutOnHook) {
			isPhoneOnHook = isPhonePutOnHook;
			
			if (!hasPhoneBeenPutOnHook) {
				base.CompletePuzzle();
			}
			hasPhoneBeenPutOnHook = true;
		}
	}
}
