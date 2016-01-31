using UnityEngine;
using System.Collections;

public class InteractionProxy : InteractableObject
{
    public InteractableObject interactable;

    public override void OnHighlight(GazePointer pointer)
    {
        interactable.OnHighlight(pointer);
    }

    public override void OffHighlight(GazePointer pointer)
    {
        interactable.OffHighlight(pointer);
    }

    public override bool OnInteract(GazePointer pointer)
    {
        return interactable.OnInteract(pointer);
    }

    public override bool OnGrabbedHighlight(Transform highlightedObject)
    {
        return interactable.OnGrabbedHighlight(highlightedObject);
    }

    public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
    {
        return interactable.OffInteract(pointer, objectToInteractWith);
    }

    public override void LogicUpdate()
    {
        interactable.LogicUpdate();
    }

    // Fixed Update, called by the GazePointer
    public override void PhysicsUpdate()
    {
        interactable.PhysicsUpdate();
    }
}
