using UnityEngine;
using System.Collections;

public class GrabbableScribbler : GrabbableObject
{

    protected const string containerTag = "ScribblerContainer";

    public enum scribblerType
    {
        pen,
        pencil
    }

    public scribblerType type;

    protected override void Awake()
    {
        base.Awake();

        grabbedInteractTag = containerTag;
    }

    public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
    {


        return base.OffInteract(pointer, objectToInteractWith);
    }
}
