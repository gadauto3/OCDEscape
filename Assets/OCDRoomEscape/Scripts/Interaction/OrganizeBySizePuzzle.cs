using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganizeBySizePuzzle : MonoBehaviour 
{
	List<SizedObject> allSizedObjects;
	List<SizedObject> sizedAndPlacedObjects;

	// Use this for initialization
	void Start () 
	{
		allSizedObjects = new List<SizedObject>();
		sizedAndPlacedObjects = new List<SizedObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetPuzzle()
	{
		Debug.Log("Do we need ResetPuzzle?");
	}

	public void AddSizedObject(SizedObject sizedObject) 
	{
		allSizedObjects.Add(sizedObject);
	}
	
	public void PlaceSizedObject(SizedObject sizedObject) 
	{
		sizedAndPlacedObjects.Add(sizedObject);

		// When an object is placed, check if the puzzle is solved
		bool isSolved = false;
		if (sizedAndPlacedObjects.Count == allSizedObjects.Count) {
			isSolved = true; // Flipped for the for loop
			float previousSize = sizedAndPlacedObjects[0].Size;
			for (int i = 1; i < sizedAndPlacedObjects.Count; i++) {
				SizedObject nextObject = sizedAndPlacedObjects[i];
				if (nextObject.Size > previousSize) {
					isSolved = false;
					break;
				}
			}
		}

		if (isSolved) {
			Debug.Log("OrganizeBySizePuzzle is solved");
		}
	}

	public void UnplaceSizedObject(SizedObject sizedObject) 
	{
		sizedAndPlacedObjects.Remove(sizedObject);
	}
}
