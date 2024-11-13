using System.Collections;
using System.Collections.Generic; 
using UnityEngine;


public class ghostPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f; 
    public Vector2 pointA;  
    public Vector2 pointB;   

    private Vector2 targetPoint;

    void Start()
    {
        
        targetPoint = pointB;
    }

    void Update()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        
        if ((Vector2)transform.position == targetPoint)
        {
            
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }
}
