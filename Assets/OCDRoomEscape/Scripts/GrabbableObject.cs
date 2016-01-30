using UnityEngine;
using System.Collections;

/// <summary>
/// This object can be grabbed
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : InteractableObject
{
    public float grabSpeed = 1f;

    public float springStrength = 600f;
    public float springDamping = 4f;

    public float freeDrag = 0f;
    public float grabbedDrag = 10f;

    public Vector3 grabMoveOffset = Vector3.zero; // This is an offset that the object will move to when grabbing before moving to the anchor. 

    public Vector3 rotationAxis;

    protected GazePointer pointer;
    protected Transform gazeAnchor;

    protected Transform anchor;
    protected Rigidbody anchorBody;

    protected Vector3 lastPosition;
    protected Quaternion lastRotation;

    protected Transform lastParent;

    protected Rigidbody myRigidbody;

    protected ConfigurableJoint myJoint;

    protected bool grabbed;

    protected bool moveToAnchor;
    protected bool moveToLastPosition;

    protected override void Awake()
    {
        base.Awake();

        lastPosition = transform.position;
        lastRotation = transform.rotation;
        lastParent = transform.parent;

        myRigidbody = GetComponent<Rigidbody>();

        CreateAnchor();
        CreateJoint();
//        myRigidbody.isKinematic = true;
    }

    // This will be called by the Gaze pointer after an object is grabbed. 
    public override void PhysicsUpdate()
    {
        if (moveToAnchor || moveToLastPosition) return;

        anchor.position = gazeAnchor.position;
    }

    public override bool OnInteract(GazePointer pointer)
    {
        base.OnInteract(pointer);

        this.pointer = pointer;
        this.gazeAnchor = pointer.Anchor;

//        myRigidbody.isKinematic = false;

        StopAllCoroutines();
        StartCoroutine(MoveToAnchor());

        return true;
    }

    public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
    {
        base.OffInteract(pointer, objectToInteractWith);

        this.pointer = null;
        gazeAnchor = null;

        StopAllCoroutines();
        StartCoroutine(MoveToLastPosition());

        return true;
    }

    protected IEnumerator MoveToAnchor()
    {
        moveToAnchor = true;

        anchor.position = transform.position;
        anchor.rotation = transform.rotation;

        myRigidbody.drag = grabbedDrag;

        LimitJoint();

        // myJoint.linearLimit = new SoftJointLimit() {limit = 0.001f};

        var moveToTargetPos = true;

        var targetPosition = anchor.position + grabMoveOffset;

        while (moveToTargetPos)
        {
            var toTarget = targetPosition - anchor.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toTarget.magnitude);

            anchor.position += toTarget.normalized * moveDistance;

            if (Vector3.Distance(anchor.position, targetPosition) < 0.004f)
            {
                moveToTargetPos = false;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }

        while (moveToAnchor)
        {
            var toAnchor = gazeAnchor.position - anchor.position;

            var moveDistance = Mathf.Min(grabSpeed*Time.deltaTime, toAnchor.magnitude);

            anchor.position += toAnchor.normalized*moveDistance;

            if (Vector3.Distance(anchor.position, gazeAnchor.position) < 0.001f)
            {
                moveToAnchor = false;
                // anchor.parent = gazeAnchor;

                anchor.position = gazeAnchor.position;

                // myJoint.linearLimit = new SoftJointLimit();
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    protected IEnumerator MoveToLastPosition()
    {
        moveToLastPosition = true;

        // anchor.parent = lastParent;

        var moveToTargetPos = true;

        var targetPosition = lastPosition + grabMoveOffset;

        while (moveToTargetPos)
        {
            var toTarget = targetPosition - anchor.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toTarget.magnitude);

            anchor.position += toTarget.normalized * moveDistance;

            if (Vector3.Distance(anchor.position, targetPosition) < 0.004f)
            {
                moveToTargetPos = false;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }

        while (moveToLastPosition)
        {
            var toTarget = lastPosition - anchor.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toTarget.magnitude);

            anchor.position += toTarget.normalized * moveDistance;

            if (Vector3.Distance(anchor.position, lastPosition) < 0.01f)
            {
                moveToLastPosition = false;
                anchor.position = lastPosition;
                // myJoint.linearLimit = new SoftJointLimit();

                FreeJoint();

                myRigidbody.drag = freeDrag;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    // This anchor will move around this object
    protected void CreateAnchor()
    {
        var go = new GameObject(gameObject.name + " anchor");
        anchor = go.transform;
        anchor.position = transform.position;
        anchor.rotation = transform.rotation;
        anchor.parent = transform.parent;

        anchorBody = anchor.gameObject.AddComponent<Rigidbody>();
        anchorBody.isKinematic = true;
    }

    // This will create a spring joint to connect this object with the anchor
    protected void CreateJoint()
    {
        myJoint = gameObject.AddComponent<ConfigurableJoint>();

        myJoint.linearLimit = new SoftJointLimit() {limit = 0.001f};
        myJoint.linearLimitSpring = new SoftJointLimitSpring() {spring = springStrength, damper = springDamping};

        myJoint.connectedBody = anchorBody;
    }

    protected void FreeJoint()
    {
        myJoint.xMotion = ConfigurableJointMotion.Free;
        myJoint.yMotion = ConfigurableJointMotion.Free;
        myJoint.zMotion = ConfigurableJointMotion.Free;
    }

    protected void LimitJoint()
    {
        myJoint.xMotion = ConfigurableJointMotion.Limited;
        myJoint.yMotion = ConfigurableJointMotion.Limited;
        myJoint.zMotion = ConfigurableJointMotion.Limited;
    }
}
