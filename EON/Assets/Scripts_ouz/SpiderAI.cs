using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject bombObj;


    private float distance;
    [SerializeField] bool alreadyAttacked;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] private float bulletSpeedX;
    [SerializeField] private float bulletSpeedY;

    private void Awake()
    {
        playerObj = FindObjectOfType<PlayerMovements>().gameObject;
        agent = this.GetComponent<NavMeshAgent>();
    }
    private void Update()
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
            Rigidbody rb = Instantiate(bombObj, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeedX, ForceMode.Impulse);
            rb.AddForce(transform.up * bulletSpeedY, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        } 
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


}
