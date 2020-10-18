﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // The static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float cameraZ;

    private void Awake()
    {
        cameraZ = this.transform.position.z;   
    }

    private void FixedUpdate()
    {
        // Do nothing if there is no POI
        if (POI == null) return;

        // Get the position of the POI
        Vector3 destination = POI.transform.position;
        // Limit the X & Y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        // Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        // Force destination.z to be cameraZ to keep the camera far enough away
        destination.z = cameraZ;
        // Set the camera to the destination
        transform.position = destination;
        // Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;
    }
}
