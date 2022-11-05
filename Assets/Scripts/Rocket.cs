using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float damageRadius = 5.0f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private GameObject explosionPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        MakeDamage();
        Destroy(gameObject);
    }

    private void MakeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(damage);
            } else continue;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(100, 100, 100, .3f);
        Gizmos.DrawSphere(transform.position, damageRadius);
    }
}
