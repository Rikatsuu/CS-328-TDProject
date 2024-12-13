//Bullet.cs - script for managing bullet behavior like speed and damage
//  this class is specific to machine gun mango for now, will make more modular later

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //initializing editable attributes for bullet
    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float bulletDamage = .25f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    private Transform target;

    private void FixedUpdate()
    {
        if (!target)
        {
            return;
        }
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * bulletSpeed * 1.2f;
    }

    //function to handle damage on collision with enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //When bullet collides with another game object, it gets its health and deals damage
        collision.gameObject.GetComponent<Health>().TakeDamage(bulletDamage); 
        Destroy(gameObject); //destroys bullet upon collision to free memory and avoid performance issues
    }

    //sets the enemy for bullet to track
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void increaseDamage(float damageIncrease)
    {
        bulletDamage += damageIncrease;
    }

    public void decreaseDamage(float damageIncrease)
    {
        bulletDamage -= damageIncrease;
    }
}
