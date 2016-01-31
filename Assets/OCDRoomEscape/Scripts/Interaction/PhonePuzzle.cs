using UnityEngine;
using System.Collections;

public class PhonePuzzle : Puzzle 
{	
	private bool isPhoneOnHook = false;
	private bool hasPhoneBeenPutOnHook = false;

    private bool didSolvePuzzle = false;


    // Use this for initialization
    void Start () {
		
	}

    public void SetPuzzleIsSolved()
    {
        didSolvePuzzle = true;
    }

    void Update () {
		bool isPhonePutOnHook = didSolvePuzzle;
		if (isPhoneOnHook != isPhonePutOnHook) {
			isPhoneOnHook = isPhonePutOnHook;
			
			if (!hasPhoneBeenPutOnHook) {
				base.CompletePuzzle();
			}
			hasPhoneBeenPutOnHook = true;
		}
	}
}
