using UnityEngine;
using System.Collections;

/// <summary>
/// This is a pointer for gazing and interacting with things
/// It should be attached to the camera, and the object's forward direction
/// should match with where it is pointing. 
/// </summary>
public class GazePointer : MonoBehaviour
{
    public LayerMask interactableMask; // Interactive objects should be on their own layer and this mask should match that layer

    // If an object is grabbed, this layer defines what can be interacted with while grabbed
    // For example, the holder of the pens after a pen has been grabbed. 
    public LayerMask grabbedInteractableMask; 


    public float anchorDistance = 1f; // This is the distance from the viewer that a grabbed object will be brought to. 

    protected Transform anchor; // This object is created by this script and is a anchor point that objects will be attached to. 

    protected const float raycastDistance = 100f;

    protected InteractableObject highlightedObject;

    protected InteractableObject interactingObject;

    protected bool interacting = false;

    #region Properties
    public bool IsInteracting { get { return interacting; } }
    public InteractableObject HighlightedObject { get { return highlightedObject; } }
    public InteractableObject InteractingObject { get { return interactingObject; } }
    public Transform Anchor { get { return anchor; } }
    #endregion

    #region UnityMethods
    protected void Awake()
    {
        CreateAnchor();
    }

    protected void Update()
    {
        if (!interacting) // Nothing is grabbed
        {
            UpdateNotInteracting();
        }
        else
        {
            UpdateInteracting();
        }
    }

    protected void FixedUpdate()
    {
        anchor.localPosition = new Vector3(0, 0, anchorDistance);

        if(interacting) InteractingObject.PhysicsUpdate();
    }
    #endregion
    
    #region EventMethods

    protected void UpdateNotInteracting()
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, interactableMask.value, QueryTriggerInteraction.Collide))
        {
            var interactable = hit.transform.gameObject.GetComponent<InteractableObject>();
            if (interactable == null) return;

            OnHit(hit, interactable);
        }
        else
        {
            UpdateHighlight(null);
        }

    }

    protected void UpdateInteracting()
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Transform objectToInteractWith = null;

        if (Physics.Raycast(ray, out hit, raycastDistance, grabbedInteractableMask.value, QueryTriggerInteraction.Collide))
        {
            objectToInteractWith = hit.transform;
        }

        InteractingObject.LogicUpdate();

        if (IsClickDown())
        {
            interacting = false;
            interactingObject.OffInteract(this, objectToInteractWith);
        }
    }

    protected void OnHit(RaycastHit hitInfo, InteractableObject interactable)
    {
        UpdateHighlight(interactable);

        if (IsClickDown())
        {
            interacting = interactable.OnInteract(this);

            if (interacting)
            {
                interactingObject = interactable;

                interactable.OffHighlight(this); // Dehighlight the object when it's interacted with
                highlightedObject = null;
            }
        }
    }

    #endregion

    #region HelperMethods

    protected bool IsClickDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    protected void UpdateHighlight(InteractableObject interactable)
    {
        if (interactable == null) // DeHighlight when not pointing at an object
        {
            if (highlightedObject != null)
            {
                highlightedObject.OffHighlight(this);
                highlightedObject = null;
            }
        }
        else if (!interactable.Equals(highlightedObject))
        {
            if (highlightedObject != null)
            {
                highlightedObject.OffHighlight(this);
            }

            highlightedObject = interactable;
            highlightedObject.OnHighlight(this);
        }
    }

    protected void CreateAnchor()
    {
        var go = new GameObject("GazeAnchor");
        anchor = go.transform;
        anchor.position = transform.position;
        anchor.rotation = transform.rotation;
        anchor.parent = transform;
    }
    #endregion


}
