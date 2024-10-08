using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

   

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float bulletDamage = .25f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;


    private Transform target;
    private int bulletDuration; //THIS SHIT


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!target)
        {
            return;
        }
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(gameObject);
        //Add something that will delete missed bullets after some time so memory can be freed
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
