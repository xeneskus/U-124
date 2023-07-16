using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject playerObj;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] PlayerVar playerVarScript;
    [SerializeField] GameObject missileObj;
    [SerializeField] GameObject missilePlaceHolder1;
    [SerializeField] GameObject missilePlaceHolder2;
    [SerializeField] MissileSpawner missileSpawnerScript;
    [SerializeField] Minigun minigunScript;


    [SerializeField] float closeDistance;
    [SerializeField] float forcePush;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;
    [SerializeField] float shockwaveCooldown = 5f;
    [SerializeField] float minigunCooldown = 2f;
    [SerializeField] float missileCooldown = 4f;
    private float shockwaveTimer = 0f;
    private float minigunTimer = 0f;
    private float missileTimer = 0f;
    private float distance;

    void Start()
    {
        playerVarScript = GameObject.Find("player").GetComponent<PlayerVar>();
        missileSpawnerScript = GameObject.Find("missile place holder1").GetComponent<MissileSpawner>();
        minigunScript = GameObject.Find("Minigun").GetComponent<Minigun>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {


        if(distance > 25f)
        {
            Homingmissile();
            
        }
        if(distance> 15f && distance < 25f)
        {
            Minigun();
        }
        if(distance < 10f)
        {
            ShockWave();
        }
    }

    public void ShockWave()
    {
        if (Vector3.Distance(transform.position, playerObj.transform.position) <= closeDistance)
        {
            playerObj.GetComponent<Rigidbody>().AddForce((playerObj.transform.position - transform.position).normalized * forcePush, ForceMode.Impulse);
        }
    }
    public void Homingmissile()
    {
        missileSpawnerScript.Attack();
    }
    public void Minigun()
    {
        minigunScript.Attack();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
