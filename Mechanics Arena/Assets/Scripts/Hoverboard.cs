#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hoverboard : MonoBehaviour
{
    Vector3 [] edgePoints = new Vector3[4];
    Vector3 [] hitPoints = new Vector3[4];
    public Vector3 forces= Vector3.zero;
    public Vector3 diff = Vector3.zero;
    public Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        hitPoints[0] = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z + 2); //Front left
        hitPoints[1] = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 2); //Front right
        hitPoints[2] = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z - 2); //Back left
        hitPoints[3] = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - 2); //Back right
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTheEdgePoints();
        CastRays();

        for(int c = 0; c < 4; c++)
        {
            diff = edgePoints[c] - hitPoints[c];
            if (diff.y < 3)
                forces = new Vector3(0, 240 - diff.y * 100, 0);
            else
                forces = Vector3.zero;

            //forces = new Vector3(0, 10, 0);

            rigidbody.AddForceAtPosition(forces, edgePoints[c]);
        }

        //Vector3 diff = hitPoints[0] - edgePoints[0];
        //rigidbody.AddForceAtPosition(diff.normalized, edgePoints[0]);
    }

    void UpdateTheEdgePoints()
    {
        edgePoints[0] = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z + 2); //Front left
        edgePoints[1] = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 2); //Front right
        edgePoints[2] = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z - 2); //Back left
        edgePoints[3] = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - 2); //Back right
    }

    void CastRays()
    {
        RaycastHit hit;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        for (int c = 0; c < 4; ++c)
        {
            if (Physics.Raycast(edgePoints[c], new Vector3(0, -1, 0), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(edgePoints[c], -Vector3.up * hit.distance, Color.red);
                hitPoints[c] = new Vector3(edgePoints[c].x, edgePoints[c].y - hit.distance, edgePoints[c].z);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int c = 0; c < 4; c++)
        {
            Gizmos.DrawSphere(edgePoints[c], 0.3f);
            Gizmos.DrawSphere(hitPoints[c], 0.3f);
        }
    }
}
