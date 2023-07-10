using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform target; 
   private NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>(); 
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        nav.destination = target.position; 
    }
}
