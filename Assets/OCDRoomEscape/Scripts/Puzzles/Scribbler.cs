using UnityEngine;
using System.Collections;

public class Scribbler : MonoBehaviour 
{
	public bool isPen;

	ScribblerSortingPuzzle puzzle;

	// Use this for initialization
	void Start () 
	{
		puzzle = GetComponentInParent<ScribblerSortingPuzzle>();
		// puzzle.AddScribbler(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
