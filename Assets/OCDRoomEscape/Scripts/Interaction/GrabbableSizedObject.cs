using UnityEngine;
using System.Collections;

public class GrabbableSizedObject : GrabbableObject 
{
	GrabbableSizedObject objectBelow;
	OrganizeBySizePuzzle puzzle;

	// Use this for initialization
	void Start () 
	{
		puzzle = GetComponentInParent<OrganizeBySizePuzzle>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float Size { 
		get { 
			BoxCollider collider = GetComponent<BoxCollider>();
			return collider.size.x * collider.size.z;
		} 
	}

	public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
	{
		if (objectToInteractWith)
		{
			objectBelow = objectToInteractWith.GetComponent<GrabbableSizedObject>();
			
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
