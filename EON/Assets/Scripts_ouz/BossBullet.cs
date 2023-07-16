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
        // Belirli bir s�re sonra mermiyi yok etmek i�in Invoke fonksiyonunu kullanabilirsiniz
        Invoke("DestroyBullet", lifetime);
        playerVarScript = GameObject.Find("player").GetComponent<PlayerVar>();
    }

    private void Update()
    {
        // Mermiyi ileri do�ru hareket ettirme
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Mermi ba�ka bir objeye �arpt���nda yap�lacaklar
        if (other.CompareTag("Player"))
        {
            // Oyuncuya zarar verme kodunu buraya yerle�tirin
            playerVarScript.TakeDamaga(damage);
            // �arpt�ktan sonra mermiyi yok etme
            DestroyBullet();
        }

    }

    private void DestroyBullet()
    {
        // Mermiyi yok etme kodunu buraya yerle�tirin
        Destroy(gameObject);
    }
}
