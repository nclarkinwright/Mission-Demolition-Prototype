﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMultiplier = 8f;
    
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPosition;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody projectileRigidbody;
    private TrailRenderer trail;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPosition;
        }
    }

    private void Awake()
    {
        S = this;
        Transform launchPointTransform = transform.Find("LaunchPoint");
        launchPoint = launchPointTransform.gameObject;
        launchPoint.SetActive(false);
        launchPosition = launchPointTransform.position;
    }

    private void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        // Instantiate a Projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        // Start it as the launchPoint
        projectile.transform.position = launchPosition;
        // Set it to isKinematic now
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        //If Slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return;

        // Get the current mouse position in 2D screen coordinates
        Vector3 mousePosition2D = Input.mousePosition;
        mousePosition2D.z = -Camera.main.transform.position.z;
        Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(mousePosition2D);

        // Find the delta from the launchPosition to the mousePosition3D
        Vector3 mouseDelta = mousePosition3D - launchPosition;
        // Limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // Move the projectile to this new position
        Vector3 projectilePosition = launchPosition + mouseDelta;
        projectile.transform.position = projectilePosition;

        if(Input.GetMouseButtonUp(0))
        {
            // The mouse has been released
            // Enable TrailRenderer of Projectile
            trail = projectile.GetComponent<TrailRenderer>();
            trail.emitting = true;
            
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMultiplier;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
        }
    }
}
