using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [Header("Attributes")]
    [SerializeField] private float movementSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        target = Level1.main.path[pathIndex];
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == Level1.main.path.Length)
            {
                Destroy(gameObject);
                return;

            }
            else
            {
                target = Level1.main.path[pathIndex];
            }
        }
    }
    private void FixedUpdate ()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * movementSpeed;
    }

}



