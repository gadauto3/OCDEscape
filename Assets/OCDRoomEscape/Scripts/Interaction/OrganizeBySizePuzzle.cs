using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganizeBySizePuzzle : MonoBehaviour 
{
	List<GrabbableSizedObject> allSizedObjects;
	List<GrabbableSizedObject> sizedAndPlacedObjects;

	// Use this for initialization
	void Awake () 
	{
		allSizedObjects = new List<GrabbableSizedObject>();
		sizedAndPlacedObjects = new List<GrabbableSizedObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetPuzzle()
	{
		Debug.Log("Do we need ResetPuzzle?");
	}

	public void AddSizedObject(GrabbableSizedObject sizedObject) 
	{
		allSizedObjects.Add(sizedObject);
	}
	
	public void PlaceSizedObjectOnObject(GrabbableSizedObject sizedObject, GrabbableSizedObject belowObject)
	{
		if (!sizedAndPlacedObjects.Contains(belowObject)) {
			sizedAndPlacedObjects.Add(belowObject);
			Debug.Log("Add sizedPlacedObj: "+belowObject);
		}
		sizedAndPlacedObjects.Add(sizedObject);

		// When an object is placed, check if the puzzle is solved
		bool isSolved = false;
		if (sizedAndPlacedObjects.Count == allSizedObjects.Count && sizedAndPlacedObjects.Count > 0) {
			isSolved = true; // Flipped for the for loop
			int previousSize = sizedAndPlacedObjects[0].size;
			for (int i = 1; i < sizedAndPlacedObjects.Count; i++) {
				GrabbableSizedObject nextObject = sizedAndPlacedObjects[i];
				if (nextObject.size > previousSize) {
					isSolved = false;
					break;
				}
				previousSize = nextObject.size;
			}
		}

		if (isSolved) {
			Debug.Log("OrganizeBySizePuzzle is solved");
		}
	}

	public void UnplaceSizedObject(GrabbableSizedObject sizedObject) 
	{
		sizedAndPlacedObjects.Remove(sizedObject);
	}
}
