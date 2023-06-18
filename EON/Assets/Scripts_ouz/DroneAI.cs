using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject bulletObj;

    private float distance;
    [SerializeField] bool alreadyAttacked;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float bulletSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(playerObj.transform.position);

        distance = Vector3.Distance(this.gameObject.transform.position, playerObj.transform.position);

        if (distance <= agent.stoppingDistance)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(playerObj.transform);
            Attack();
        }
    }
    private void Attack()
    {
        if (!alreadyAttacked)
        {
            //bullet spawn & move
            Rigidbody rb = Instantiate(bulletObj, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
