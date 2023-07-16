using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject playerObj;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

public Transform gunTip;
    public float fireRate = 10f;
    public float rotationSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    private float nextFireTime;

    private void Update()
    {

    }
    public void Attack()
    {
        FollowPlayer();

        if (Time.time >= nextFireTime)
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        // Düþmanýn sahip olduðu scripte eriþerek oyuncuya zarar verme kodunu burada yazabilirsiniz

        nextFireTime = Time.time + 1f / fireRate;
    }

    void FollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        if (playerObj.transform.position.y <= 10)
        {
            Vector3 direction = playerObj.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.2f);
        }
        animator.enabled = true;
    }
}
