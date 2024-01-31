using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 upVector;
    public Transform planet;
    
    public float gravity = 9.81f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(UpdateVector());

        transform.up = (transform.position - planet.position).normalized;
    }
    
    public void SetUpVector(Vector3 vector3)
    {
        upVector = vector3;
    }

    IEnumerator UpdateVector()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            var direction = transform.position - planet.position;
            direction.Normalize();
            
            SetUpVector(direction);
        }
    }
    
    private void Update()
    {
        rb.velocity += -upVector * gravity * Time.deltaTime;
    }
}
