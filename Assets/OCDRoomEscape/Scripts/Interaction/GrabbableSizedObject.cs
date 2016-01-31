using UnityEngine;
using System.Collections;

public class GrabbableSizedObject : GrabbableObject 
{
	GrabbableSizedObject objectBelow;
	OrganizeBySizePuzzle puzzle;
	Vector3 dropPosition;

    public GameObject trigger;


	// Use this for initialization
	void Start () 
	{
		puzzle = GetComponentInParent<OrganizeBySizePuzzle>();
		puzzle.AddSizedObject(this);
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

    public override bool OnInteract(GazePointer pointer)
    {
        var interact =  base.OnInteract(pointer);

        if (interact)
        {
            trigger.SetActive(false);
        }

        return interact;
    }

    public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
	{
		if (objectToInteractWith && objectToInteractWith != transform)
		{
			objectBelow = objectToInteractWith.GetComponent<GrabbableSizedObject>();

			if (objectBelow)
			{
				dropPosition = objectBelow.transform.position;
			}
		}
		else
		{
			Debug.Log("objectBelow is null");
			objectBelow = null;
		}
		
		return base.OffInteract(pointer, objectToInteractWith);
	}

    protected override Vector3 GetMoveToAnchorOffsetPosition()
    {
        return beforeMovePosition + Vector3.up * 0.3f;
    }

    protected override Vector3 GetMoveToNotGrabbedOffsetPosition()
	{
		if (objectBelow) {
//            Debug.Log("Object is below");
			return objectBelow.transform.position + Vector3.up * 0.25f;
		} 

        return lastPosition + Vector3.up * 0.4f;
	}

    protected override Vector3 GetMoveToNotGrabbedPosition()
    {
        if (objectBelow)
        {
            return objectBelow.transform.position + Vector3.up * 0.15f;
        }

        return lastPosition;
    }

    protected override Quaternion GetMoveToNotGrabbedRotation()
	{
		if (objectBelow) {
			return objectBelow.transform.rotation;
		} else {
			return base.GetMoveToNotGrabbedRotation();
		}
	}

    protected override IEnumerator MoveToAnchorPosition()
    {
        moveToAnchor = true;

        anchor.position = transform.position;
        anchor.rotation = transform.rotation;

        myRigidbody.drag = grabbedDrag;

//        LimitJoint();

        myRigidbody.isKinematic = true;

        // myJoint.linearLimit = new SoftJointLimit() {limit = 0.001f};

        var moveToTargetPos = true;

        var targetPosition = GetMoveToAnchorOffsetPosition();

        while (moveToTargetPos)
        {
            var toTarget = targetPosition - transform.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toTarget.magnitude);

            transform.position += toTarget.normalized * moveDistance;

            if (Vector3.Distance(transform.position, targetPosition) < 0.004f)
            {
                moveToTargetPos = false;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }

        var initRotation = transform.rotation;
        var targetRotation = Quaternion.identity;

        var initDist = Vector3.Distance(transform.position, gazeAnchor.position);


        while (moveToAnchor)
        {
            var anchorPos = gazeAnchor.position + Vector3.up*-0.1f;
            var toAnchor = anchorPos - transform.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toAnchor.magnitude);

            transform.position += toAnchor.normalized * moveDistance;

            var dist = Vector3.Distance(transform.position, anchorPos);

            var t = dist / Mathf.Max(initDist, float.Epsilon);

            // var localUp = pointer.transform.InverseTransformDirection(grabbedUpAxis);
            // transform.rotation = Quaternion.Slerp(targetRotation, initRotation, t);

            if (dist < 0.001f)
            {
                moveToAnchor = false;
                // anchor.parent = gazeAnchor;

                transform.position = anchorPos;

                myRigidbody.isKinematic = true;
                FreeJoint();

                anchor.position = transform.position;

                transform.parent = gazeAnchor;

                // myJoint.linearLimit = new SoftJointLimit();
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    protected override IEnumerator MoveToNotGrabbedPosition()
    {
        moveToLastPosition = true;

        // anchor.parent = lastParent;

        transform.parent = lastParent;
        anchor.position = transform.position;

        myRigidbody.isKinematic = true;

//        LimitJoint();

        var moveToTargetPos = true;


        var targetPosition = GetMoveToNotGrabbedOffsetPosition();

//        Debug.Log("Target Position");
//        Debug.Log(targetPosition);

        var initRotation = transform.rotation;
        var targetRotation = GetMoveToNotGrabbedRotation();

        var initDist = Vector3.Distance(transform.position, targetPosition);

        while (moveToTargetPos)
        {
            var toTarget = targetPosition - transform.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toTarget.magnitude);

            transform.position += toTarget.normalized * moveDistance;

            // Handle Rotation Slerp
            var dist = Vector3.Distance(transform.position, targetPosition);
            var t = dist / Mathf.Max(initDist, float.Epsilon);

            transform.rotation = Quaternion.Slerp(targetRotation, initRotation, t);

            if (Vector3.Distance(transform.position, targetPosition) < 0.004f)
            {
                moveToTargetPos = false;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }

        targetPosition = GetMoveToNotGrabbedPosition();

        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;

        while (moveToLastPosition)
        {
            var toTarget = targetPosition - transform.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toTarget.magnitude);

            transform.position += toTarget.normalized * moveDistance;

            transform.rotation = targetRotation;

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                moveToLastPosition = false;
                transform.position = targetPosition;
                // myJoint.linearLimit = new SoftJointLimit();

                FreeJoint();
                anchor.position = transform.position;

                myRigidbody.velocity = Vector3.zero;
                myRigidbody.angularVelocity = Vector3.zero;

                myRigidbody.drag = freeDrag;

//                yield return  new WaitForSeconds(0.05f);
                myRigidbody.isKinematic = false;

                trigger.SetActive(true);

				// Now that we're done moving, notify the puzzle
				puzzle.PlaceSizedObjectOnObject(this, objectBelow);
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
