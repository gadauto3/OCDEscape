using UnityEngine;
using System.Collections;

/// <summary>
/// This is like Grabbable object but with no physics. 
/// </summary>
public class GrabAndPlaceObject : InteractableObject
{
    public float grabSpeed = 1f;

    public string grabbedInteractTag; // Tag for objects you can interact with while this object is grabbed

    public float verticalInitOffset = 0f;
    public float verticalPlacedOffset = 0f;
    public float verticalAnchorOffset = 0f;

    protected GazePointer pointer;
    protected Transform gazeAnchor;

    protected Vector3 initPosition;
    protected Quaternion initRotation;
    protected Transform initParent;

    protected bool grabbed;

    protected bool movingToAnchor;
    protected bool movingToPlacedPosition;

    protected Vector3 beforeMovePosition;
    protected bool placed = false;


    [Tooltip("This sound will play when the interaction starts")]
    public AudioSource startAudioSource;

    public bool customLoopStartAudio;
    public float customLoopFreq = 1f;

    protected bool doCustomLoopAudio;


    [Tooltip("This sound will play when the object is placed")]
    public AudioSource placedSoundSource;

    protected override void Awake()
    {
        base.Awake();

        initPosition = transform.position;
        initRotation = transform.rotation;
        initParent = transform.parent;
    }

    public override void StartInteraction()
    {
        base.StartInteraction();

        if (startAudioSource)
        {
            if (customLoopStartAudio)
            {
                startAudioSource.loop = false;

                StopCoroutine(CustomLoopAudio());
                StartCoroutine(CustomLoopAudio());
            }
            else
            {
                startAudioSource.loop = true;
                startAudioSource.Play();
            }
        }

        placed = false;
    }

    public override bool OnInteract(GazePointer pointer)
    {
        base.OnInteract(pointer);

        this.pointer = pointer;
        this.gazeAnchor = pointer.Anchor;

        placement = null;

        beforeMovePosition = transform.position;

        if (placed)
        {
            if (startAudioSource && customLoopStartAudio)
            {
//                StopCoroutine(CustomLoopAudio());
//                Debug.Log("Starting Custom Loop Audio");
                if(!doCustomLoopAudio) StartCoroutine(CustomLoopAudio());
            }
            else if (startAudioSource && !startAudioSource.isPlaying)
            {
                startAudioSource.Play();
            }

            placed = false;
        }

        StopCoroutine(MoveToAnchorPosition());
        StopCoroutine(MoveToNotGrabbedPosition());

        StartCoroutine(MoveToAnchorPosition());
//
//        if (doCustomLoopAudio)
//        {
//            StartCoroutine(CustomLoopAudio());
//        }

        return true;
    }

    protected GrabbedInteractablePlacement placement;

    public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
    {
        base.OffInteract(pointer, objectToInteractWith);

        this.pointer = null;
        gazeAnchor = null;

        if (objectToInteractWith != null)
        {
            placement = objectToInteractWith.GetComponent<GrabbedInteractablePlacement>();

            if (placement != null)
            {

            }
        }

//        StopAllCoroutines();

        StopCoroutine(MoveToAnchorPosition());
        StopCoroutine(MoveToNotGrabbedPosition());
        StartCoroutine(MoveToNotGrabbedPosition());

        return true;
    }

    public override bool OnGrabbedHighlight(Transform highlightedObject)
    {
        return highlightedObject.gameObject.tag == grabbedInteractTag;
    }

    protected virtual Vector3 GetMoveToAnchorPosition()
    {
        return gazeAnchor.position + new Vector3(0, verticalAnchorOffset, 0);
    }

    protected virtual Vector3 GetMoveToAnchorOffsetPosition()
    {
        return beforeMovePosition + new Vector3(0, verticalInitOffset, 0);
    }

    protected virtual Vector3 GetMoveToNotGrabbedOffsetPosition()
    {
        return GetMoveToNotGrabbedPosition() + new Vector3(0, verticalPlacedOffset, 0);
    }

    protected virtual Vector3 GetMoveToNotGrabbedPosition()
    {
        if (placement != null)
        {
            return placement.placedPosition.position;
        }

        return initPosition;
    }

    protected virtual Quaternion GetMoveToNotGrabbedRotation()
    {
        if (placement != null)
        {
            return placement.placedPosition.rotation;
        }
        return initRotation;
    }

    protected virtual IEnumerator MoveToAnchorPosition()
    {
        movingToAnchor = true;

        var moveToOffset = true;

        var targetPosition = GetMoveToAnchorOffsetPosition();

        while (moveToOffset)
        {
            var toTarget = targetPosition - transform.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toTarget.magnitude);

            transform.position += toTarget.normalized * moveDistance;

            if (Vector3.Distance(transform.position, targetPosition) < 0.004f)
            {
                moveToOffset = false;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }

//        var initRotation = transform.rotation;
//        var targetRotation = Quaternion.identity;

        var initDist = Vector3.Distance(transform.position, gazeAnchor.position);


        while (movingToAnchor)
        {
            var toAnchor = gazeAnchor.position - transform.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toAnchor.magnitude);

            transform.position += toAnchor.normalized * moveDistance;

            var dist = Vector3.Distance(transform.position, gazeAnchor.position);

            var t = dist / Mathf.Max(initDist, float.Epsilon);


            //            var localUp = pointer.transform.InverseTransformDirection(grabbedUpAxis);
            // transform.rotation = Quaternion.Slerp(targetRotation, initRotation, t);

            if (dist < 0.001f)
            {
                movingToAnchor = false;
                // anchor.parent = gazeAnchor;

//                transform.position = gazeAnchor.position;

                transform.parent = gazeAnchor;

                // myJoint.linearLimit = new SoftJointLimit();
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    protected IEnumerator CustomLoopAudio()
    {
        doCustomLoopAudio = true;
        startAudioSource.loop = false;

        while (doCustomLoopAudio)
        {
            startAudioSource.Play();

            yield return new WaitForSeconds(customLoopFreq);
        }
    }

    protected virtual IEnumerator MoveToNotGrabbedPosition()
    {
        movingToPlacedPosition = true;

        // anchor.parent = lastParent;

        transform.parent = initParent;

        var moveToOffset = true;

        var targetPosition = GetMoveToNotGrabbedOffsetPosition();

        var initRotation = transform.rotation;
        var targetRotation = GetMoveToNotGrabbedRotation();

        var initDist = Vector3.Distance(transform.position, targetPosition);

        while (moveToOffset)
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
                moveToOffset = false;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }

        targetPosition = GetMoveToNotGrabbedPosition();

        while (movingToPlacedPosition)
        {
            var toTarget = targetPosition - transform.position;

            var moveDistance = Mathf.Min(grabSpeed * Time.deltaTime, toTarget.magnitude);

            transform.position += toTarget.normalized * moveDistance;

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                movingToPlacedPosition = false;
                transform.position = targetPosition;

                if (placement != null)
                {
                    if (startAudioSource)
                    {
                        startAudioSource.Stop();

                        doCustomLoopAudio = false;

                        // StopCoroutine(CustomLoopAudio());
                    }

                    if (placedSoundSource)
                    {
                        placedSoundSource.loop = false;
                        placedSoundSource.Play();
                    }
                }

                placed = true;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
