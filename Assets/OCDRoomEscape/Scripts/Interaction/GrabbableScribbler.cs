using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

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

    public override void LogicUpdate()
    {
        if (moveToAnchor || moveToLastPosition) return;

        holder = null;
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
                holder.FreeDropPosition(dropPosition);
                puzzle.RemoveFromHolder(this);
            }

        }
        // holder = null; // We just removed from the holder

        return interact;
    }

    protected ScribblerHolder holder;


    protected static Vector3 RandomVector()
    {
        var v = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);
        return v.normalized;
    }

    protected ScribblerHolder.DropPosition dropPosition;

    public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
    {
        if (objectToInteractWith)
        {
            holder = objectToInteractWith.GetComponent<ScribblerHolder>();

            if (holder)
            {
                holder.puzzle.PutInHolder(this, holder);

                dropPosition = holder.GetFreeDropPosition();
            }
        }
        else
        {
            Debug.Log("objectToInteractWith is null");
            holder = null;
        }

        return base.OffInteract(pointer, objectToInteractWith);
    }

    protected override Quaternion GetMoveToNotGrabbedRotation()
    {
        if (holder)
        {
            return holder.dropPosition.rotation;
        }
        else
        {
            return lastRotation;
        }
    }

    protected override Vector3 GetMoveToAnchorOffsetPosition()
    {
        if (holder)
        {
            return holder.dropPosition.position;
        }
        else
        {
            return transform.position;
        }
    }

    protected override Vector3 GetMoveToNotGrabbedOffsetPosition()
    {
        if (holder)
        {
            if (dropPosition != null) return holder.dropPosition.position + dropPosition.offset;

            Debug.Log("Drop Position is null");
            return holder.dropPosition.position;
        }
        else
        {
            return lastPosition + Vector3.up * 0.2f;
        }

//        return base.GetMoveToNotGrabbedPosition() + Vector3.up * 0.5f;
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
