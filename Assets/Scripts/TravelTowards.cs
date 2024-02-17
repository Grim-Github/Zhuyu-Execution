using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelTowards : MonoBehaviour
{
    [SerializeField] public float travelSpeed = 20;
    [SerializeField] public Vector3 destination;
    [SerializeField] public bool isOnTarget = false;
    private void Update()
    {
        transform.position = Vector3.LerpUnclamped(transform.position, destination, Time.deltaTime * travelSpeed); 
        if(Vector3.Distance(transform.position , destination) < .2f)
        {
            isOnTarget = true;
            Destroy(gameObject);
        }
    }
}
