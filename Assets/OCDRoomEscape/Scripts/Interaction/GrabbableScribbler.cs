using UnityEngine;
using System.Collections;

public class GrabbableScribbler : GrabbableObject
{

    protected const string containerTag = "ScribblerContainer";

    protected ScribblerSortingPuzzle puzzle;

    public enum scribblerType
    {
        pen,
        pencil
    }

    public bool isPen { get { return type == scribblerType.pen; } }

    public scribblerType type;

    protected override void Awake()
    {
        base.Awake();

        grabbedInteractTag = containerTag;
    }

	void Start()
	{
		puzzle = GetComponentInParent<ScribblerSortingPuzzle>();
		puzzle.AddScribbler(this);
	}

    public override bool OnInteract(GazePointer pointer)
    {
        var interact = base.OnInteract(pointer);

        if (interact)
        {
            if (puzzle)
            {
                puzzle.RemoveFromHolder(this);
            }
        }

        return interact;
    }

    public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
    {
        if (objectToInteractWith)
        {
            var holder = objectToInteractWith.GetComponent<ScribblerHolder>();

            if (holder)
            {
                holder.puzzle.PutInHolder(this, holder);
            }
        }

        return base.OffInteract(pointer, objectToInteractWith);
    }
}
