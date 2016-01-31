using UnityEngine;
using System.Collections;

public class CoffeePotPuzzleLinker : MonoBehaviour
{

    public GrabAndPlaceObject placement;

    public CoffeePotPuzzle coffeePuzzle;


    protected void Start()
    {
        placement.onObjectPlacedAction += OnPuzzleComplete;
    }

    protected void OnPuzzleComplete()
    {
        coffeePuzzle.SetPuzzleIsSolved();
    }

}
