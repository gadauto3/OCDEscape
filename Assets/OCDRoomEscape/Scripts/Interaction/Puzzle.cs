using UnityEngine;
using System.Collections;

public class Puzzle : MonoBehaviour {

	public PuzzleMaster master;
	public int puzzleWeight = 1; // If the puzzle has more weight, it has a greater affect on room growth

	float waitBeforeTryingToReset = 1f;
	float waitBetweenResetChecks = 2f;

	// Use this for initialization
	public virtual void Start () 
	{
		GameObject room = GameObject.Find("PuzzleMaster");
		Debug.Log("Room: "+room);
		master = room.GetComponent<PuzzleMaster>();
		Debug.Log("Master: "+master);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void CompletePuzzle()
	{
		master.PuzzleCompleted(this);
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
