using UnityEngine;
using System.Collections;

public class RoomLightsPuzzle : Puzzle 
{
	private bool isRoomLightOn = false;
	private bool hasLightBeenTurnedOn = false;

	// Use this for initialization
	void Start () {
	
	}

	void Update () {
		bool isLightTurnedOn = false;
		if (isRoomLightOn != isLightTurnedOn) {
			isRoomLightOn = isLightTurnedOn;
			
			if (!hasLightBeenTurnedOn) {
				base.CompletePuzzle();
			}
			hasLightBeenTurnedOn = true;
		}
	}
}
