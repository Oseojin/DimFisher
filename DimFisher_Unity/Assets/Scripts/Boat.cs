using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 10f;
    public float turnSpeed = 50f;

    public Transform MotorVec;

    public bool onPlayer = false;

    private void FixedUpdate()
    {
        if (!onPlayer)
        {
            return;
        }

        float moveAmount = Input.GetAxis("Vertical") * speed;
        float turnAmount = Input.GetAxis("Horizontal") * turnSpeed;

        rb.AddForceAtPosition(transform.forward * moveAmount, transform.localPosition);

        Vector3 rotationPoint = transform.TransformPoint(MotorVec.localPosition);
        rb.AddForceAtPosition(-transform.right * turnAmount, rotationPoint);
    }
}
