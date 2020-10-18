using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // A static field accessible by code anywhere
    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        // When the trigger is hit by something
        // Check to see if it's a Projectile
        if(other.gameObject.tag == "Projectile")
        {
            // If so, set goalMet to true
            Goal.goalMet = true;
            // Also se the alpha of the color to higher opacity
            Material mat = GetComponent<Renderer>().material;
            Color color = mat.color;
            color.a = 1;
            mat.color = color;
        }
    }
}
