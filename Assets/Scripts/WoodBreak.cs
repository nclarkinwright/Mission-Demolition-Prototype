using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBreak : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject brokenPiece;
    public int numBrokenPieces;
    
    private Rigidbody rb;
    private Vector3 vector;
    private float velocity;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            rb = collision.gameObject.GetComponent<Rigidbody>();
            vector = rb.velocity;
            velocity = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));

            if (velocity >= 10)
            {
                Destroy(gameObject);
                for (int i = 0; i < numBrokenPieces; i++)
                {
                    Instantiate<GameObject>(brokenPiece);
                    brokenPiece.transform.position = gameObject.transform.position;
                }
            }
        }
    }
}
