using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System;

public class PlayerVar : MonoBehaviour
{
    public float maxHealth = 100;
    [HideInInspector]public float curHealth;
    public Slider healtSlid;
    public TextMeshProUGUI healthText;


    public int _bulletVar;
    public TextMeshProUGUI _textBullet;


    public float maxPil = 100;
    [HideInInspector] public float curPil;
    public Slider PilSlider;

    


    private void Start()
    {
        curHealth = maxHealth;
        _textBullet.text = _bulletVar.ToString();
    }
    private void Update()
    {
        
    }
    public void TakeDamaga(int dmg)
    {
        if (curHealth <= 0) return;
        curHealth -= dmg;
        if(curHealth < 0) curHealth = 0;
        healtSlid.value = curHealth / 100;
        healthText.text = curHealth.ToString();
    }

    public void RegenHealth(int regen)
    {
        if (curHealth >= 100) return;
        curHealth += regen;
        if(curHealth > 100) { curHealth = 100; }
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

    public void UsePil(float pil)
    {
        if(curPil <= 0) return;
        curPil -= pil;
        if(curPil <0) curPil= 0;
        PilSlider.value = curPil / 100;
    }

    public void TakePil(float pil)
    {
        if (curPil >= 100) return;
        curPil += pil;
        if(curPil > 100) curPil= 100;
        PilSlider.value = curPil / 100;
    }



}
