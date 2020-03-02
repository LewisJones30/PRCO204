﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 50;

    public void DealDamage(int damage)
    {
        health -= damage;

        if (health < 1) Destroy(gameObject);
    }
}
