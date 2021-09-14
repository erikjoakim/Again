using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLookat : MonoBehaviour
{
    public Transform cameraToLookAt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraToLookAt, Vector3.up);
    }
}
