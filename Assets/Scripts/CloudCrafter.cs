using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.05f;

    private GameObject[] cloudInstances;

    private void Awake()
    {
        // Make an arra large enough to hold all the Cloud_ instances
        cloudInstances = new GameObject[numClouds];
        // Find the CloudAnchor parent GameObject
        GameObject anchor = GameObject.Find("CloudAnchor");
        // Iterate through and make Cloud_s
        GameObject cloud;
        for(int i = 0; i < numClouds; i++)
        {
            // Make an instance of cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            // Position cloud
            Vector3 cloudPos = Vector3.zero;
            cloudPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cloudPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            // Scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            // Smaller clouds (with smaller scaleU) should be nearer to the ground
            cloudPos.y = Mathf.Lerp(cloudPosMin.y, cloudPos.y, scaleU);
            // Smaller clouds should be further away
            cloudPos.z = 100 - 90 * scaleU;
            // Apply these transforms to the cloud
            cloud.transform.position = cloudPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            // Make cloud a child of the anchor
            cloud.transform.SetParent(anchor.transform);
            // Add the cloud to cloudInstances
            cloudInstances[i] = cloud;
        }
    }

    private void Update()
    {
        // Iterate over each cloud that was created
        foreach(GameObject cloud in cloudInstances)
        {
            // Get the cloud scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cloudPos = cloud.transform.position;
            // Move larger clouds faster
            cloudPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            // If a cloud has moved too far to the left...
            if(cloudPos.x <= cloudPosMin.x)
            {
                // Move it to the far right
                cloudPos.x = cloudPosMax.x;
            }
            // Apply the new position to cloud
            cloud.transform.position = cloudPos;
        }
    }
}
