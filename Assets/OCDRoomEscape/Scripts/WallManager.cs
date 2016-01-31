using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WallManager : MonoBehaviour
{

    [Range(0f,1f)]
    public float transformRoom = 0;

    public Transform ceiling;

    public Vector2 wallHeightRange = new Vector2(2.5f, 3.4f);
    public Vector2 wallWidthRange = new Vector2(3f, 6f);

    public Vector2 wallOffsetRange = new Vector2(2f, 8f);

    [Serializable]
    public class Wall
    {
        public Transform transform;
        public Transform mesh;
    }

    public Wall[] walls;


    protected void Update()
    {
        var t = transformRoom;

        for (var i = 0; i < walls.Length; i++)
        {
            var wall = walls[i];

            if(wall.transform == null || wall.mesh == null) continue;

            wall.transform.position = transform.position - wall.transform.forward*Mathf.Lerp(wallOffsetRange.x, wallOffsetRange.y, t);
        }
    }
}
