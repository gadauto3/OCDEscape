using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenPuzzle : MonoBehaviour
{

	public float percentOfFlippedPens = 0.3f;

	List<Pen> pens;

	void Awake ()
	{
		pens = new List<Pen> ();
	}

	public void PlacePen (Pen placedPen)
	{
		bool isSolved = true;
		bool isFirstPenVerticle = pens [0].isVerticle;

		// If all pens are oriented in the same direction, the puzzle is solved
		foreach (Pen pen in pens) {
			if (pen.isVerticle != isFirstPenVerticle) {
				isSolved = false;
				break;
			}
		}

		if (isSolved) {
			Debug.Log ("pen puzzle is solved");
		}
	}

	void Reset ()
	{
		foreach (Pen pen in pens) {
			// Only flip a certain percent of pens
			pen.isVerticle = (Random.value <= percentOfFlippedPens);
		}
	}
}
