using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleMaster : MonoBehaviour 
{
	public List<Puzzle> puzzles;
	float increment;
	float weightedIncrementTotal;

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
		var growthIncrement = puzzle.puzzleWeight / weightedIncrementTotal;
		// TODO: Notify room of growth increment
		Debug.Log("Puzzle completed: "+puzzle+"\nNotify room of growth increment: "+growthIncrement);

		if (puzzle.IsResetable() && puzzles.Contains(puzzle)) {
			Debug.Log("Puzzle marked for reset");
			puzzle.MarkForReset();
		}

		Debug.Log("Index: "+puzzles.IndexOf(puzzle)+" in puzzles: "+puzzles+" with count "+puzzles.Count);
		// Remove the puzzle, but also allow it to remain in the list multiple times
		puzzles.Remove(puzzle);
		Debug.Log("Count after: "+puzzles.Count);
	}
}
