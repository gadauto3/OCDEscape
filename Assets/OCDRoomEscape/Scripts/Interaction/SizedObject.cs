using UnityEngine;
using System.Collections;

public class SizedObject : GrabbableObject 
{
	SizedObject objectBelow;
	OrganizeBySizePuzzle puzzle;

	// Use this for initialization
	void Start () 
	{
		puzzle = GetComponentInParent<OrganizeBySizePuzzle>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float Size { get { return 1f; } }

	public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
	{
		if (objectToInteractWith)
		{
			objectBelow = objectToInteractWith.GetComponent<SizedObject>();
			
			if (objectBelow)
			{
				puzzle.AddSizedObject(this);
				
//				dropPosition = objectBelow.transform.position; //TODO: Gotta handle the drop
			}
		}
		else
		{
			Debug.Log("objectBelow is null");
			objectBelow = null;
		}
		
		return base.OffInteract(pointer, objectToInteractWith);
	}

}
