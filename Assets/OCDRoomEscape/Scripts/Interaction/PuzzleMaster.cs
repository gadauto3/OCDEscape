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
		puzzles = new List<Puzzle>(); // TODO: Is this how editor is supposed to work?

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

		// Remove the puzzle, but also allow it to remain in the list
		puzzles.RemoveAt(puzzles.IndexOf(puzzle));

		if (puzzle.IsResetable() && puzzles.Contains(puzzle)) {
			puzzle.MarkForReset();
		}
	}
}
