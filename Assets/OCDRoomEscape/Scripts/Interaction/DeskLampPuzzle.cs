﻿using UnityEngine;
using System.Collections;

public class DeskLampPuzzle : Puzzle 
{
	private bool isLampOn = false;
	private bool hasLampBeenTurnedOn = false;
	private InteractableSwitch lampSwitch;

	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		lampSwitch = GetComponent<InteractableSwitch>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isLampOn != lampSwitch.turnedOn) {
			isLampOn = lampSwitch.turnedOn;

			if (!hasLampBeenTurnedOn) {
				base.CompletePuzzle();
			}
			hasLampBeenTurnedOn = true;
		}
	}

}
