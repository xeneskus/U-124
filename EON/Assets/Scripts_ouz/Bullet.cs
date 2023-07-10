using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] PlayerVar playerVarScript;
    
    [SerializeField] float bulletRadius;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float selfDestroyTime;
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        playerVarScript = GameObject.Find("Player").GetComponent<PlayerVar>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, bulletRadius, playerLayer);
        foreach(var hitCollider in hitColliders)
        {
            Debug.Log(damage + " vurdu");
            HitDamage();
            Destroy(this.gameObject);
        }
        StartCoroutine(DestroySelf());
    }   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bulletRadius);
    }
    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(selfDestroyTime);
        Destroy(this.gameObject);
    }

    void HitDamage()
    {
        playerVarScript.TakeDamaga(damage);
    }
}
