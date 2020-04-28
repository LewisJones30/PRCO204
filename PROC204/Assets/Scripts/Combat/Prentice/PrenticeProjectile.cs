﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrenticeProjectile : Projectile
{    
    [SerializeField] float turnSpeed = 1f;
    [SerializeField] protected ColourValue projectileColour;
    [SerializeField] float seekingDistance = 20f;
    [SerializeField] float reload = 1f;

    //STATES

    //Used when homing in on targets
    Collider targetEnemy;

    //Reserved for Prentice projectiles to help with aiming
    protected bool isHoming = true;

    public float Reload { get => reload; }

    protected override void Update()
    {
        base.Update();

        if (!isHoming) return;

        FindClosestTarget();
        SteerTowardTarget();
    }

    //Destroys projectile when not in view by camera
    //Prevents enemies from being killed offscreen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //Projectile finds closest available target within seeking range
    private void FindClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, seekingDistance, LayerMask.GetMask("Enemy", "Target"));

        float smallestDistance = Mathf.Infinity;
        Collider closestEnemy = null;

        foreach (Collider collider in colliders)
        {
            Health health = collider.gameObject.GetComponent<Health>();
            if (health == null || health.IsDead) continue;

            float distance = Vector3.Distance(collider.transform.position, transform.position);

            if (distance < smallestDistance)
            {
                closestEnemy = collider;
                smallestDistance = distance;
            }
        }

        targetEnemy = closestEnemy;
    }

    //If target is set then steer the projectile toward the target
    private void SteerTowardTarget()
    {
        if (targetEnemy == null) return;

        Vector3 targetDir = targetEnemy.bounds.center - transform.position;
        transform.forward = Vector3.RotateTowards(transform.forward, targetDir, turnSpeed * Time.deltaTime, 0f);
    }

    //Deal damage if hit enemy and apply colour bonus 
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            return;
        }
        CombatTarget combatTarget = other.gameObject.GetComponentInParent<CombatTarget>();
        if (combatTarget != null) combatTarget.TakeDamage(damage, projectileColour, transform.position);

        GameObject instance = Instantiate(projectileHitFX, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
        Destroy(instance, 10f); //Destroy hit effect after alloted time

        Destroy(gameObject);
    }

}
