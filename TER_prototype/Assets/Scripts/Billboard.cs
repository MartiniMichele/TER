using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform Cam;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Cam.forward);   
    }
}
