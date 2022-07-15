using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowboardController : MonoBehaviour
{
    public static float zAxisChange;

    public float gravityForce;

    private Vector3 movementDirection;
    public Vector3 velocity;

    private Rigidbody rb;

    private float lastPosition;
    

    private void Awake() {
        lastPosition = transform.position.z;
    }

    private void Start() {
        this.rb = GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void Update()
    {
        zAxisChange = Input.GetAxisRaw("Vertical") / 100;
        lastPosition = rb.transform.position.z;

        RaycastHit ray;
        if (Physics.Raycast(transform.position, Vector3.down, out ray)) {
            rb.AddForce(Vector3.up * 2f);
        }
    }

    private void ClearZAxisVelocity() {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

}
