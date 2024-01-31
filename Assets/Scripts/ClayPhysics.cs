using System;
using System.Collections;
using System.Collections.Generic;
using Clayxels;
using DG.Tweening;
using UnityEngine;

public class ClayPhysics : MonoBehaviour
{
    public GameObject clayEquiv;

    public float blend;

    private GameObject claySpawned;
    private Transform clayContainer;
    
    public Transform planet;
    public Spawner spawner;

    private bool hasLanded;

    private Vector3 scale;
    
    private void Start()
    {
        clayContainer = FindObjectOfType<ClayContainer>().transform;

        scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, 0.3f);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Static") && !hasLanded)
        {
            StartCoroutine(StopPhysics());
            hasLanded = true;
        }
    }

    IEnumerator StopPhysics()
    {
        yield return new WaitForSeconds(0.1f);
        
        SpawnClay();

        tag = "Static";
        
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<CustomGravity>());

        transform.DOScale(Vector3.zero, 0.25f).OnComplete(DestroyComponents);
    }

    private void DestroyComponents()
    {
        Destroy(gameObject.GetComponent<MeshFilter>());
        Destroy(gameObject.GetComponent<MeshRenderer>());
        Destroy(gameObject.GetComponent<ClayPhysics>());

        transform.localScale = scale;
    }

    private void SpawnClay()
    {
        claySpawned = Instantiate(clayEquiv, transform.position, transform.rotation, clayContainer);
        claySpawned.transform.localScale = transform.localScale;
        claySpawned.GetComponent<ClayObject>().blend = blend;

        var clayColor = claySpawned.GetComponent<ClayColor>();
        clayColor.spawner = spawner;
        
        var dist = (transform.position - planet.position).magnitude;
        clayColor.distanceFromPlanet = dist;
        
        clayColor.Initialize();
        
        FindObjectOfType<MultipleTargetCamera>().targets.Add(claySpawned.transform);
    }
}
