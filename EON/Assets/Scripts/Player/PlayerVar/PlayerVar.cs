using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerVar : MonoBehaviour
{
    public float maxHealth = 100;
    private float curHealth;
    public Slider healtSlid;

    private void Start()
    {
        curHealth = maxHealth;
    }
    private void Update()
    {
        print(curHealth);
        if (Input.GetKeyDown(KeyCode.F)) { TakeDamaga(10); }
        
    }
    public void TakeDamaga(int dmg)
    {    
        curHealth -= dmg;
        healtSlid.value = curHealth / 100;
    }

}
