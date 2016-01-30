using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScribblerSortingPuzzle : MonoBehaviour 
{
	List<GrabbableScribbler> allScribblers;
	List<ScribblerHolder> allHolders;

	// Use this for initialization
	void Awake () 
	{
		allHolders = new List<ScribblerHolder>();
		allScribblers = new List<GrabbableScribbler>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddScribbler(GrabbableScribbler scribbler)
	{
		allScribblers.Add(scribbler);
	}
	
	public void AddScribblerHolder(ScribblerHolder holder)
	{
		allHolders.Add(holder);
	}

    public void RemoveFromHolder(GrabbableScribbler scribbler)
    {
        
    }

	public void PutInHolder(GrabbableScribbler scribbler, ScribblerHolder targetHolder)
	{
		targetHolder.AddScribbler(scribbler);

		bool isPuzzleSolved = true;
		if (NumScribblersInHolders() == allScribblers.Count) {
			foreach (ScribblerHolder holder in allHolders) {
				if (holder.AreAllScribblersOfTheSameType()) {
					isPuzzleSolved = false;
					break;
				}
			}
		}

		if (isPuzzleSolved) {
			Debug.Log("Scribbler sorting puzzle is solved!");
		}
	}

	int NumScribblersInHolders()
	{
		int heldScribblers = 0;
		foreach (ScribblerHolder holder in allHolders) {
			heldScribblers += holder.ScribblerCount();
		}
		return heldScribblers;
	}
}
