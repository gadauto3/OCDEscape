using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
	public Text instructionsText;

	public void SetInstructionsForTime (string instructions, float timeToDisplay)
	{
		SetInstructions (instructions);
		Invoke ("ClearInstructions", timeToDisplay);
	}

	public void SetInstructions (string instructions)
	{
		instructionsText.text = instructions;
	}

	public void ClearInstructions ()
	{
		instructionsText.text = "";
	}

	// Use this for initialization
	void Start ()
	{
		SetInstructionsForTime ("Welcome to OCD Escape!", 3);
	}
}
