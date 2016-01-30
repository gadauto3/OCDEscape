using UnityEngine;
using System.Collections;

public class GrabbableScribbler : GrabbableObject {


    public override bool OffInteract(GazePointer pointer, Transform objectToInteractWith)
    {


        return base.OffInteract(pointer, objectToInteractWith);
    }
}
