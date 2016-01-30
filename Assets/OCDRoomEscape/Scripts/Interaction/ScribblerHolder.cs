using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScribblerHolder : MonoBehaviour
{
    public ScribblerSortingPuzzle puzzle;

    public Transform dropPosition;

	List<GrabbableScribbler> scribblers;
	
	public void AddScribbler(GrabbableScribbler scribbler)
	{
		scribblers.Add(scribbler);
	}
	
	public void RemoveScribbler(GrabbableScribbler scribbler)
	{
		scribblers.Remove(scribbler);
	}
	
	public bool ContainsScribbler(GrabbableScribbler scribbler)
	{
		return scribblers.Contains(scribbler);
	}

	public bool AreAllScribblersOfTheSameType()
	{
		if (scribblers.Count == 0) return false;

		bool areTypesTheSame = true;
		bool firstScribblerType = scribblers[0].isPen;

		foreach (GrabbableScribbler scribbler in scribblers) {
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
		scribblers = new List<GrabbableScribbler>();
		
		ScribblerSortingPuzzle puzzle = GetComponentInParent<ScribblerSortingPuzzle>();
		puzzle.AddScribblerHolder(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
