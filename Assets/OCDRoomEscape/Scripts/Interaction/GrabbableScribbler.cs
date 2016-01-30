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
            if (puzzle && holder)
            {
                puzzle.RemoveFromHolder(this);
            }

        }
        holder = null; // We just removed from the holder

        return interact;
    }

    protected ScribblerHolder holder;

    public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
    {
        if (objectToInteractWith)
        {
            holder = objectToInteractWith.GetComponent<ScribblerHolder>();

            if (holder)
            {
                holder.puzzle.PutInHolder(this, holder);
            }
        }
        else
        {
            Debug.Log("objectToInteractWith is null");
            holder = null;
        }

        return base.OffInteract(pointer, objectToInteractWith);
    }

    protected override Vector3 GetMoveToNotGrabbedOffsetPosition()
    {
        return base.GetMoveToNotGrabbedPosition() + Vector3.up * 0.5f;
    }

    protected override Vector3 GetMoveToNotGrabbedPosition()
    {

        if (holder)
        {
            return holder.transform.position;
        }

        return lastPosition;
    }
}
