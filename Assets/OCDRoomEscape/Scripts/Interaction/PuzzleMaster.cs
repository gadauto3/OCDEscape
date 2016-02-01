using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleMaster : MonoBehaviour 
{
	public float timeBetweenSounds = 15f;
	public WallManager wallMgr;
	public List<Puzzle> puzzles;
	
	float increment;
	float weightedIncrementTotal;
	bool isGameOver = false;

	// Use this for initialization
	void Awake () 
	{
		foreach (var puzzle in puzzles) {
			Debug.Log("Puzzle added: "+puzzle);
		}
	}

	void Start()
	{
		// We'll consider the puzzle's weight when determining how much it affects the room movement
		foreach (var puzzle in puzzles) {
			weightedIncrementTotal += puzzle.puzzleWeight;
		}
	}
	
	public void PuzzleCompleted(Puzzle puzzle)
	{
		if (isGameOver) {
			Debug.Log("GAME OVER!!! Let the player know!!");
			return;
		}

		var growthIncrement = puzzle.puzzleWeight / weightedIncrementTotal;
		// Notify room of growth increment
		wallMgr.Resize(wallMgr.transformRoom + growthIncrement);
		KickOffSoundForPuzzle(puzzle);

		if (puzzle.IsResetable() && puzzles.Contains(puzzle)) {
			Debug.Log("Puzzle marked for reset");
			puzzle.MarkForReset();
		}

		// Remove the puzzle, but also allow it to remain in the list multiple times
		puzzles.Remove(puzzle);
		Debug.Log("Finished puzzle: "+puzzle+", "+puzzles.Count+" left, room growing to: "+(wallMgr.transformRoom + growthIncrement));

		if (wallMgr.transformRoom >= 1f) {
			isGameOver = true;
			Debug.Log("GAME OVER!!!");
		}
	}

	private void KickOffSoundForPuzzle(Puzzle puzzle) {
		AudioSource source = puzzle.SoundForPuzzle();

		if (source) {
			StartCoroutine(PlaySound(source));
		} else {
			Debug.Log(puzzle+" does not have an associated AudioSource");
		}
	}

	private IEnumerator PlaySound(AudioSource source) {
		while (true) {
			source.Play();
			yield return new WaitForSeconds(timeBetweenSounds);
		}
	}
}
