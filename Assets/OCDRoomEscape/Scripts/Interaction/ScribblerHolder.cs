using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScribblerHolder : MonoBehaviour
{
    public ScribblerSortingPuzzle puzzle;

    public Transform dropPosition;

	List<GrabbableScribbler> scribblers;

    public float dropRadius = 0.05f;

    public int dropPositionCount = 8;

    public class DropPosition
    {
        public int index;
        public bool inUse;
        public Vector3 offset;
    }

    protected List<DropPosition> dropPositions = new List<DropPosition>();

    public DropPosition GetFreeDropPosition()
    {
        foreach (var position in dropPositions)
        {
            if (!position.inUse)
            {
                position.inUse = true;
                return position;
            }
        }

        return null;
    }

    public void FreeDropPosition(DropPosition position)
    {
        if (position == null) return;

        dropPositions[position.index].inUse = false;
    }

    protected void CreateDropPositions()
    {
        dropPositions = new List<DropPosition>();

        var angleStep = 360f/dropPositionCount;

        // Debug.Log("AngleStep = " + angleStep);

        var angle = 180f;

        var startVector = -transform.forward * dropRadius;

        startVector.y = 0;

        for (var i = 0; i < dropPositionCount; i++)
        {
            var newDropPos = new DropPosition();

            newDropPos.offset = Quaternion.AngleAxis(angle, Vector3.up) * startVector;
            newDropPos.inUse = false;
            newDropPos.index = i;

            dropPositions.Add(newDropPos);

            angle += angleStep;
        }
    }

    protected void Awake()
    {
        CreateDropPositions();
    }

    public void AddScribbler(GrabbableScribbler scribbler)
	{
		scribblers.Add(scribbler);
	}
	
	public void RemoveScribbler(GrabbableScribbler scribbler)
	{
		scribblers.Remove(scribbler);
	}
	
	public bool ContainsScribbler(GrabbableScribbler scribbler)
	{
		return scribblers.Contains(scribbler);
	}

	public bool AreAllScribblersOfTheSameType()
	{
		if (scribblers.Count == 0) return false;

		bool areTypesTheSame = true;
		bool firstScribblerType = scribblers[0].isPen;

		foreach (GrabbableScribbler scribbler in scribblers) {
			if (firstScribblerType != scribbler.isPen) {
				areTypesTheSame = false;
				break;
			}
		}
		return areTypesTheSame;
	}

	public int ScribblerCount()
	{
		return scribblers.Count;
	}

	// Use this for initialization
	void Start () 
	{
		scribblers = new List<GrabbableScribbler>();
		
		ScribblerSortingPuzzle puzzle = GetComponentInParent<ScribblerSortingPuzzle>();
		puzzle.AddScribblerHolder(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
