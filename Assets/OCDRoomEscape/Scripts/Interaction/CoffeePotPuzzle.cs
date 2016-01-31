using UnityEngine;
using System.Collections;

public class CoffeePotPuzzle : Puzzle 
{
	private bool isCoffeePotInMaker = false;
	private bool hasCoffeePotGoneHome = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
		bool isCoffeePotNowInMaker = false;
		if (isCoffeePotInMaker != isCoffeePotNowInMaker) {
			isCoffeePotInMaker = isCoffeePotNowInMaker;
			
			if (!hasCoffeePotGoneHome) {
				base.CompletePuzzle();
			}
			hasCoffeePotGoneHome = true;
		}
	}
}
