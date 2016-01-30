using UnityEngine;
using System.Collections;

public class CoffeePot : InteractableObject
{
	bool isDripping = true;
	Coroutine dripAction = null;

	float dripTimeMin = 2f;
	float dripTimeMax = 10f;

	void Start ()
	{
	
	}

	void Update ()
	{
		if (isDripping && dripAction == null) {
			dripAction = StartCoroutine (Drip ());
		}
	}

	public override bool OnInteract(GazePointer pointer)
	{
		base.OnInteract(pointer);

		isDripping = false;
		StopCoroutine (dripAction);

		return false;
	}

	IEnumerator Drip ()
	{
		while (isDripping) {

			// The dripping occurs at somewhat random intervals
			yield return new WaitForSeconds (Random.Range (dripTimeMin, dripTimeMax));

			// Make a drip occur
		}
	}
}
