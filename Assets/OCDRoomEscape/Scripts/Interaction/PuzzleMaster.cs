using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleMaster : MonoBehaviour 
{
	public List<Puzzle> puzzles;
	float increment;

	// Use this for initialization
	void Awake () 
	{
		puzzles = new List<Puzzle>();
	}

	void Start()
	{
		// This is how much the room should grow by on each puzzle completion
		increment = 1f / puzzles.Count;
	}
	
	public void PuzzleCompleted(Puzzle puzzle)
	{
		// Notify room of growth increment

		// Remove the puzzle, but also allow it to remain in the list
		puzzles.RemoveAt(puzzles.IndexOf(puzzle));

		if (puzzle.IsResetable() && puzzles.Contains(puzzle)) {
			puzzle.MarkForReset();
		}
	}
}
