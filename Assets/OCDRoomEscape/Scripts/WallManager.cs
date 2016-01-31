using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WallManager : MonoBehaviour
{

    [Range(0f,1f)]
    public float transformRoom = 0;

    public Transform ceiling;
    public Transform ceilingMesh;


    public Vector2 wallHeightRange = new Vector2(2.5f, 3.4f);
    public Vector2 wallWidthRange = new Vector2(3f, 6f);

    public Vector2 wallOffsetRange = new Vector2(2f, 8f);

    public bool autoGrowRoom;

    public float growStep = 0.005f;

    public float growTime = 10f;
    protected float growTimer;

    [Serializable]
    public class Wall
    {
        public Transform transform;
        public Transform mesh;

        public Transform[] moveWithRoom;

        public WallTransformCache[] attachedObjects;
    }

    public class WallTransformCache
    {
        public Vector3 initLocalPos;
        public Transform transform;
    }

    public Wall[] walls;

    protected void Awake()
    {

        foreach (var wall in walls)
        {
            if (wall.transform == null || wall.mesh == null) continue;

            wall.attachedObjects = new WallTransformCache[wall.moveWithRoom.Length];

            for (var i = 0; i < wall.moveWithRoom.Length; i++)
            {
                var moveWithRoom = wall.moveWithRoom[i];

                if(moveWithRoom == null) continue;

                var attachedObject = new WallTransformCache();

                attachedObject.transform = moveWithRoom;
                attachedObject.initLocalPos = moveWithRoom.localPosition;

                wall.attachedObjects[i] = attachedObject;
                
            }
        }

        growTimer = growTime;
    }

    protected void Update()
    {
        if (autoGrowRoom)
        {
            growTimer -= Time.deltaTime;

            if (growTimer < 0)
            {
                transformRoom += growStep;
                growTimer = growTime;
                ResizeRoom();
            }
        }
#if UNITY_EDITOR
    // ResizeRoom();
#endif

    }

    public void Resize(float t)
    {
        transformRoom = t;
        ResizeRoom();
    }

    protected void ResizeRoom()
    {

        var t = transformRoom;

        var width = Mathf.Lerp(wallWidthRange.x, wallWidthRange.y, t);
        var height = Mathf.Lerp(wallHeightRange.x, wallHeightRange.y, t);

        var scaler = new Vector3(width, height, 1f);

        for (var i = 0; i < walls.Length; i++)
        {
            var wall = walls[i];

            if (wall.transform == null || wall.mesh == null) continue;

            wall.transform.position = transform.position - wall.transform.forward * Mathf.Lerp(wallOffsetRange.x, wallOffsetRange.y, t);

//            var wallMeshPos = wall.mesh.localPosition;
//            // wallMeshPos.y = height * 0.5f;
//            wall.mesh.localPosition = wallMeshPos;

            wall.mesh.localScale = scaler;

            if (wall.attachedObjects == null) continue;

            foreach (var attachedObject in wall.attachedObjects)
            {
                if (attachedObject == null) continue;
                if (attachedObject.transform == null) continue;

                attachedObject.transform.localPosition = Vector3.Scale(attachedObject.initLocalPos, new Vector3(1 + t, 1, 1));
            }
        }

        ceiling.position = transform.position + Vector3.up * height;

        ceilingMesh.localScale = new Vector3(width, width, 1);
    }
}
