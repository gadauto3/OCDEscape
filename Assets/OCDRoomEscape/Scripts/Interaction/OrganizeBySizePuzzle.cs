using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganizeBySizePuzzle : MonoBehaviour 
{
	List<GrabbableSizedObject> allSizedObjects;
	List<GrabbableSizedObject> sizedAndPlacedObjects;

	// Use this for initialization
	void Start () 
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
	
	public void PlaceSizedObject(GrabbableSizedObject sizedObject) 
	{
		sizedAndPlacedObjects.Add(sizedObject);

		// When an object is placed, check if the puzzle is solved
		bool isSolved = false;
		if (sizedAndPlacedObjects.Count == allSizedObjects.Count) {
			isSolved = true; // Flipped for the for loop
			float previousSize = sizedAndPlacedObjects[0].Size;
			for (int i = 1; i < sizedAndPlacedObjects.Count; i++) {
				GrabbableSizedObject nextObject = sizedAndPlacedObjects[i];
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

	public void UnplaceSizedObject(GrabbableSizedObject sizedObject) 
	{
		sizedAndPlacedObjects.Remove(sizedObject);
	}
}
