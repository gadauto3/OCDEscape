using UnityEngine;
using System.Collections;

public class CoffeePotPuzzle : Puzzle
{
    private bool didSolvePuzzle = false;
	private bool isCoffeePotInMaker = false;
	private bool hasCoffeePotGoneHome = false;
	
	// Use this for initialization
	void Start () {
		
	}

    public void SetPuzzleIsSolved()
    {
        didSolvePuzzle = true;
    }
	
	void Update () {
		bool isCoffeePotNowInMaker = didSolvePuzzle;
		if (isCoffeePotInMaker != isCoffeePotNowInMaker) {
			isCoffeePotInMaker = isCoffeePotNowInMaker;
			
			if (!hasCoffeePotGoneHome) {
				base.CompletePuzzle();
			}
			hasCoffeePotGoneHome = true;
		}
	}
}
