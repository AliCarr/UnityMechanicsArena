using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followTarget;
    Vector3 difference;
    // Start is called before the first frame update
    void Start()
    {
        difference = transform.position - followTarget.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        difference = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 1, Vector3.up) * difference;
        transform.SetPositionAndRotation(followTarget.transform.position + difference, transform.rotation);
        transform.LookAt(followTarget.transform);
        
    }
}
