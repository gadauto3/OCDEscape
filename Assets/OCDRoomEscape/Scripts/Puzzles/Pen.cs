using UnityEngine;
using System.Collections;

public class Pen : InteractableObject
{
	public PenPuzzle penPuzzle;
	public bool isVerticle = true;

	public void FlipPen ()
	{
		isVerticle = !isVerticle;
		// Perform tranform on pen to flip it visibly
		Debug.Log ("Need to flip pen vertically");
	}

	public virtual bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
	{
		penPuzzle.PlacePen (this);
		return true;
	}
}
