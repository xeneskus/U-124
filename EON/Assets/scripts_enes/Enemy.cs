using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject explosionEffect; 
   // public AudioClip explosionSound; 
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    void Explode()
    {
        //  AudioSource.PlayClipAtPoint(explosionSound, transform.position); 
        Instantiate(explosionEffect, transform.position, Quaternion.identity); 

        Destroy(gameObject);
    }

}