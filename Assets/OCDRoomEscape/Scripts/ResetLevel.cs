using UnityEngine;
using System.Collections;

public class ResetLevel : MonoBehaviour {

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(0);
        }
    }
}
