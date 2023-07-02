using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVar : MonoBehaviour
{
    public int maxHealth = 100;
    private int curHealth;

    private void Start()
    {
        curHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        curHealth -= dmg;
    }

}
