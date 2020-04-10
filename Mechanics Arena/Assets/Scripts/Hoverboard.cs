﻿#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hoverboard : MonoBehaviour
{
    public Transform[] trans = new Transform[4];
    public bool isThisThePlayerHoverCraft;

    Vector3 [] edgePoints = new Vector3[4];
    Vector3 [] hitPoints = new Vector3[4];
    Vector3 forces= Vector3.zero;
    Vector3 diff = Vector3.zero;
    Rigidbody myRigidBody;
    

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateTheEdgePoints();
        CastRays();
        SlantCheck();

        if(isThisThePlayerHoverCraft)
            HandleInputs();

        for(int c = 0; c < 4; c++)
        {
            diff = (edgePoints[c] - hitPoints[c])*1.3f;
            forces = new Vector3(0, (Mathf.Pow(0.78f, ((diff.y*2) - 1.7f)) - 0.2f)* 600, 0);
            myRigidBody.AddForceAtPosition(forces, edgePoints[c]);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidBody.AddForce(transform.forward * 50000);
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    int ax = Random.Range(0, 4);
        //    myRigidBody.AddForceAtPosition(new Vector3(0, 1200, 0), edgePoints[ax]);
        //}
    }

    void HandleInputs()
    {
        if(Input.GetKey(KeyCode.W))
            myRigidBody.AddForce(transform.forward * 1200);

        if (Input.GetKey(KeyCode.A))
            myRigidBody.AddTorque(Vector3.up * -230);

        if (Input.GetKey(KeyCode.D))
            myRigidBody.AddTorque(Vector3.up * 230);
    }

    void UpdateTheEdgePoints()
    {
        for(int c = 0; c < 4; ++c)
            edgePoints[c] = trans[c].position; //Front left
    }

    void CastRays()
    {
        RaycastHit hit;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        for (int c = 0; c < 4; ++c)
            if (Physics.Raycast(edgePoints[c], new Vector3(0, -1, 0), out hit, Mathf.Infinity, layerMask))             
                hitPoints[c] = new Vector3(edgePoints[c].x, edgePoints[c].y - hit.distance, edgePoints[c].z);

    }

    void SlantCheck()
    {
        //get the current angle relative to the X axis. If it is greater than a value, add a small amount of force in that direction
        float myAngleX = WrapAngle(transform.eulerAngles.x);
        float myAngleZ = WrapAngle(transform.eulerAngles.z);

        if (myAngleX >= 20 || myAngleX <= -20)
            myRigidBody.AddForce(transform.forward * 15 * myAngleX);

        if (myAngleZ >= 20 || myAngleZ <= -20)
            myRigidBody.AddForce(-transform.right * 15 * myAngleZ);
    }

    private float WrapAngle(float angle)
    {
        angle %= 360;

        if (angle > 180)
            return angle - 360;

        return angle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int c = 0; c < 4; c++)
        {
            Gizmos.DrawSphere(edgePoints[c], 0.3f);
            Gizmos.DrawSphere(hitPoints[c], 0.3f);
            Debug.DrawRay(edgePoints[c], Vector3.up * (hitPoints[c].y - edgePoints[c].y), Color.red);
        }
    }
}


/*
 BUG LIST:
    If it flips, the logic for sliding fucks up. 


FEATURE LIST:
    
     
     
     
     */