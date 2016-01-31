using UnityEngine;
using System.Collections;

public class PutPhoneOnDaHook : MonoBehaviour
{

    public GrabAndPlaceObject placement;

    public PhonePuzzle phonePuzzle;


    protected void Start()
    {
        placement.onObjectPlacedAction += OnPuzzleComplete;
    }

    protected void OnPuzzleComplete()
    {
        phonePuzzle.SetPuzzleIsSolved();
    }

}
