using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleMaster : MonoBehaviour 
{
	public float timeBetweenSounds = 15f;
	public WallManager wallMgr;
	public List<Puzzle> puzzles;
	public AudioSource puzzleWinSound;
	
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
		puzzleWinSound = GetComponent<AudioSource>();

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
			
			StopAllCoroutines(); // Turn off sounds
			if (puzzleWinSound) {
				puzzleWinSound.Stop();
			}

			isGameOver = true;
			Debug.Log("GAME OVER!!!");

			AudioSource finalSource = GetComponentInParent<AudioSource>();
			if (finalSource) {
				finalSource.Play();
			}
		}
	}

	private void KickOffSoundForPuzzle(Puzzle puzzle) 
	{
		StopAllCoroutines();

		// Now kick off the discordant sound
		AudioSource source = puzzle.SoundForPuzzle();

		if (!source) {
			Debug.Log(puzzle+" does not have an associated AudioSource");
		}
		
		StartCoroutine(PlaySound(source));
	}

	private IEnumerator PlaySound(AudioSource source) 
	{	
		// Play a win sound
		if (puzzleWinSound) {
			puzzleWinSound.Play();
			yield return new WaitForSeconds(7f);
		}

		while (source) {
			source.Play();
			yield return new WaitForSeconds(timeBetweenSounds);
		}
	}
}
