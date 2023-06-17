using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject playerObj;


    [SerializeField] private float rangeToExplode;
    private float distance;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        agent.SetDestination(playerObj.transform.position);

        distance = Vector3.Distance(this.gameObject.transform.position, playerObj.transform.position);
        boom(); 
    }

    public void boom()
    {
        if(distance <= rangeToExplode)
        {
            Destroy(this.gameObject);
        }
    }
}
