using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] PlayerVar playerVarScript;

    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 10;

    private void Start()
    {
        // Belirli bir süre sonra mermiyi yok etmek için Invoke fonksiyonunu kullanabilirsiniz
        Invoke("DestroyBullet", lifetime);
        playerVarScript = GameObject.Find("player").GetComponent<PlayerVar>();
    }

    private void Update()
    {
        // Mermiyi ileri doðru hareket ettirme
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Mermi baþka bir objeye çarptýðýnda yapýlacaklar
        if (other.CompareTag("Player"))
        {
            // Oyuncuya zarar verme kodunu buraya yerleþtirin
            playerVarScript.TakeDamaga(damage);
            // Çarptýktan sonra mermiyi yok etme
            DestroyBullet();
        }

    }

    private void DestroyBullet()
    {
        // Mermiyi yok etme kodunu buraya yerleþtirin
        Destroy(gameObject);
    }
}
