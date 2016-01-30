using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScribblerHolder : MonoBehaviour 
{
	List<Scribbler> scribblers;
	
	public void AddScribbler(Scribbler scribbler)
	{
		scribblers.Add(scribbler);
	}
	
	public void RemoveScribbler(Scribbler scribbler)
	{
		scribblers.Remove(scribbler);
	}

	public bool AreAllScribblersOfTheSameType()
	{
		if (scribblers.Count == 0) return false;

		bool areTypesTheSame = true;
		bool firstScribblerType = scribblers[0].isPen;

		foreach (Scribbler scribbler in scribblers) {
			if (firstScribblerType != scribbler.isPen) {
				areTypesTheSame = false;
				break;
			}
		}
		return areTypesTheSame;
	}

	public int ScribblerCount()
	{
		return scribblers.Count;
	}

	// Use this for initialization
	void Start () 
	{
		scribblers = new List<Scribbler>();
		
		ScribblerSortingPuzzle puzzle = GetComponentInParent<ScribblerSortingPuzzle>();
		puzzle.AddScribblerHolder(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
