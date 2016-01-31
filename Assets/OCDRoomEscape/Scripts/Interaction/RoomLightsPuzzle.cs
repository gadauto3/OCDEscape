using UnityEngine;
using System.Collections;

public class RoomLightsPuzzle : Puzzle 
{
	public GameObject roomLights;

	private bool isRoomLightOn = false;
	private bool hasLightBeenTurnedOn = false;


	// Use this for initialization
	void Start () 
	{
	
	}

	void Update () {
		bool isLightTurnedOn = (roomLights && roomLights.activeSelf);
		if (isRoomLightOn != isLightTurnedOn) {
			Debug.Log("Room lights activated");
			isRoomLightOn = isLightTurnedOn;
			
			if (!hasLightBeenTurnedOn) {
				base.CompletePuzzle();
			}
			hasLightBeenTurnedOn = true;
		}
	}
}
