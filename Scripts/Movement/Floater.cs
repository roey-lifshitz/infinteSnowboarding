using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rb;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 2;

    public float snowDrag = 0.99f;
    public float snowAngularDrag = 0.5f;
    private TerrainHandler terrainHandler;

   
    
    private void Start() {
        terrainHandler = FindObjectOfType<TerrainHandler>();
    }

    private void FixedUpdate() {
        rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        float terrainHeight = terrainHandler.FindHeightAtPosition(transform.position);
        print(transform.position.y - terrainHeight);
        if (transform.position.y < terrainHeight) {
            float displacementMultiplier = Mathf.Clamp01(terrainHeight - transform.position.y /
                depthBeforeSubmerged) * displacementAmount;

            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),
                transform.position, ForceMode.Acceleration);

            //rb.AddForce(displacementMultiplier * -rb.velocity * snowDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            //rb.AddTorque(displacementMultiplier * -rb.angularVelocity * snowAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
       
    }
}
