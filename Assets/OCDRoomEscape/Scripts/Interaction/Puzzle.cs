using UnityEngine;
using System.Collections;

public class Puzzle : MonoBehaviour {
	
	float waitBeforeTryingToReset = 8f;
	float waitBetweenResetChecks = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual bool IsPuzzleComplete() {
		return false;
	}
	
	public virtual bool IsResetable() {
		return false;
	}

	public virtual void MarkForReset() {
		StartCoroutine(WaitForPlayerToLookAwayToReset());
	}
	
	public virtual void ResetPuzzle() {
		
	}

	private IEnumerator WaitForPlayerToLookAwayToReset()
	{
		yield return new WaitForSeconds(waitBeforeTryingToReset);

		while (IsPuzzleInCameraView()) {
			yield return new WaitForSeconds(waitBetweenResetChecks);
		}

		ResetPuzzle();

		yield break;
	}

	private bool IsPuzzleInCameraView()
	{
		// TODO: Define
		return true;
	}
}
