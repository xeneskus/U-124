using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, this.GetComponent<SphereCollider>().radius/2.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            Debug.Log("oyuncuya vurdun!");
        }
    }
}
