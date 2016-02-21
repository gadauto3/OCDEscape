using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleMaster : MonoBehaviour 
{
	public float timeBetweenSounds = 15f;
	public WallManager wallMgr;
	public List<Puzzle> puzzles;
	public AudioSource puzzleWinSound;
	public UIManager uiManager;
	public Material highlightMaterial;  // This shouldn't probably be in this class
	
	float increment;
	float weightedIncrementTotal;
	bool isGameOver = false;

	// Use this for initialization
	void Awake () 
	{
		string puzzleDesc = "Puzzles: ";
		foreach (var puzzle in puzzles) {
			puzzleDesc += puzzle.ToString() + ", ";
		} 
		Debug.Log(puzzleDesc);
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
		growthIncrement *= 2;
		// Notify room of growth increment
		wallMgr.Resize(wallMgr.transformRoom + growthIncrement);
		KickOffSoundForPuzzle(puzzle);

		if (puzzle.IsResetable() && puzzles.Contains(puzzle)) {
			Debug.Log("Puzzle marked for reset");
			puzzle.MarkForReset();
		}

		AdjustHighlightColor(puzzle);

		// Remove the puzzle, but also allow it to remain in the list multiple times
		puzzles.Remove(puzzle);
		Debug.Log("Finished puzzle: "+puzzle+", "+puzzles.Count+" left, room growing to: "+(wallMgr.transformRoom + growthIncrement));
		if (puzzles.Count == 1) {
			Debug.Log("Final puzzle: "+puzzles[0]);
		} else if (puzzles.Count == 0) {
			isGameOver = true;
		}

		if (isGameOver) {
			
			StopAllCoroutines(); // Turn off sounds
			if (puzzleWinSound) {
				puzzleWinSound.Stop();
			}

			isGameOver = true;
			uiManager.SetInstructionsForTime("Well done, you've brought order to chaos!", 15f);

			AudioSource finalSource = GetComponentInParent<AudioSource>();
			if (finalSource) {
				finalSource.Play();
			}
		}
	}

	private void AdjustHighlightColor(Puzzle puzzle)
	{
		GameObject obj = puzzle.gameObject;
		Component[] highlightMgrs = obj.GetComponentsInChildren<HighlightManager>();
		foreach (HighlightManager highlighter in highlightMgrs) {
			highlighter.AdjustHighlightMaterial(highlightMaterial);
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
