using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject bossObj;
    [SerializeField] Rigidbody missileRb;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] PlayerVar playerVarScript;
    [SerializeField] BossAI bossAI;
    [SerializeField] LayerMask charLayers;
    public delegate void MissileDestroyedDelegate(Missile destroyedMissile);
    public event MissileDestroyedDelegate OnMissileDestroyed;

    [SerializeField] float missileSpeed;
    [SerializeField] float blastPower;
    [SerializeField] float blastRadius;
    [SerializeField] float missileRotationSpeed = 1f;
    [SerializeField] int blastDamage;
    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("player");
        bossObj = GameObject.Find("Boss");
        playerVarScript = GameObject.Find("player").GetComponent<PlayerVar>();
        bossAI = GameObject.Find("Boss").GetComponent<BossAI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = (playerObj.transform.position - transform.position).normalized;
  
        Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
        Quaternion rotation = Quaternion.Lerp(transform.rotation, toRotation, missileRotationSpeed * Time.fixedDeltaTime);

        missileRb.MoveRotation(rotation);
        missileRb.velocity = transform.forward * missileSpeed * Time.fixedDeltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius, charLayers);
        foreach(Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Patlama gücü uygula
                rb.AddExplosionForce(blastPower, transform.position, blastRadius);
            }

            // Boss veya oyuncuya hasar uygula
            BossAI boss = collider.GetComponent<BossAI>();
            if (boss != null)
            {
                boss.TakeDamage(blastDamage);
            }

            PlayerVar player = collider.GetComponent<PlayerVar>();
            if (player != null)
            {
                player.TakeDamaga(blastDamage);
                Debug.Log("git player");
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
    private void OnDestroy()
    {
        OnMissileDestroyed?.Invoke(this);
    }
}
