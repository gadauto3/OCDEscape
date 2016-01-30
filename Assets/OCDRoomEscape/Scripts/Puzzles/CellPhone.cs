using UnityEngine;
using System.Collections;

public class CellPhone : InteractableObject
{
	AudioSource phoneAudio;

	bool isRinging = true;
	Coroutine ringAction = null;

	int numRings = 3;
	float timeBetweenRings = 3f;
	float timeBetweenCallsMin = 2f;
	float timeBetweenCallsMax = 10f;

	void Start ()
	{
		phoneAudio = GetComponent<AudioSource> ();
	}

	void Update ()
	{
		if (isRinging && ringAction == null) {
			ringAction = StartCoroutine (Ring ());
		}
	}

	public override bool OnInteract(GazePointer pointer)
	{
		base.OnInteract(pointer);

		isRinging = false;
		StopCoroutine (ringAction);

		return false;
	}

	IEnumerator Ring ()
	{
		while (isRinging) {

			// The dripping occurs at somewhat random intervals
			yield return new WaitForSeconds (Random.Range (timeBetweenCallsMin, timeBetweenCallsMax));

			for (int i = 0; i < numRings; i++) {
				phoneAudio.Play ();
				yield return new WaitForSeconds (timeBetweenRings);
			}
		}
	}
}
