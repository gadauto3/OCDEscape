using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour
{
    public bool highlightable = true; // Set to true if you want the object to be able to be highlighted

    protected HighlightManager highlightManager;

    protected bool canInteract = true; // used to temporarily disable interaction. 

    protected bool startInteractionWithGameStart = false;


    #region UnityMethods
    protected virtual void Awake()
    {
        highlightManager = GetComponent<HighlightManager>();
    }
    #endregion

    public virtual void StartInteraction()
    {
        
    }

    public virtual void OnHighlight(GazePointer pointer)
    {
        if (!highlightable || !canInteract) return;

        if(highlightManager) highlightManager.OnHighlight();

        // Debug.Log("On Highlight");

    }

    public virtual void OffHighlight(GazePointer pointer)
    {
        if (!highlightable || !canInteract) return;

        if (highlightManager) highlightManager.OffHighlight();


        // Debug.Log("Off Highlight");
    }

    /// <summary>
    /// Returns true if the object is to be grabbed for example grabbing a pen.
    /// Returns false if the object is just a click and that's it
    /// </summary>
    public virtual bool OnInteract(GazePointer pointer)
    {
        if (!canInteract) return false;
//        Debug.Log("On Interact");

        return false;
    }

    public virtual bool OnGrabbedHighlight(Transform highlightedObject)
    {
        return true;
    }

    /// <summary>
    /// Returns true if it can be released
    /// objectToInteractWith is 
    /// </summary>
    /// <returns></returns>
    public virtual bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
    {
        if (!canInteract) return false;

//        Debug.Log("Off Interact");

        return true;
    }

    // Regular Update, called by the GazePointer
    public virtual void LogicUpdate()
    {
        
    }

    // Fixed Update, called by the GazePointer
    public virtual void PhysicsUpdate()
    {
        
    }
}
