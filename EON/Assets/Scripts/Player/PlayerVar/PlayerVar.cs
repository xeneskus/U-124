using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class PlayerVar : MonoBehaviour
{
    public float maxHealth = 100;
    private float curHealth;
    public Slider healtSlid;
    public TextMeshProUGUI healthText;


    public int _bulletVar;
    public TextMeshProUGUI _textBullet;


    private void Start()
    {
        curHealth = maxHealth;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) { TakeDamaga(10); }
        if (Input.GetKeyDown(KeyCode.S)) { RegenHealth(10); }
        if (Input.GetKeyDown(KeyCode.D)) { UseBullet(1); }
    }
    public void TakeDamaga(int dmg)
    {
        if (curHealth <= 0) return;
        curHealth -= dmg;
        healtSlid.value = curHealth / 100;
        healthText.text = curHealth.ToString();
    }

    public void RegenHealth(int regen)
    {
        if (curHealth >= 100) return;
        curHealth += regen;
        healtSlid.value = curHealth / 100;
        healthText.text = curHealth.ToString();
    }


    public void UseBullet(int bullet)
    {
        if (_bulletVar <= 0) return;
        _bulletVar -= bullet;
        _textBullet.text = _bulletVar.ToString();
    }
    public void TakeBullet(int tbullet)
    {
        _bulletVar += tbullet;
        _textBullet.text = _bulletVar.ToString();
    }

}
