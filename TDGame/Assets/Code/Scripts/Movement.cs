//movement of enemies on the path

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
        target = Level1.main.path[pathIndex]; //sets the inital target to the first waypoint in the path
    }

    private void Update()
    {
        //check if the enemy is close enough to the current target waypoint
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex >= Level1.main.path.Length)  //last waypoint
            {
                Spawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }

            //update target to next waypoint
            target = Level1.main.path[pathIndex];
        }
    }
    private void FixedUpdate() //calculate normalized direction vector towards the target waypoint
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * movementSpeed;
    }

    public void updateSpeed(float newSpeed)
    {
        movementSpeed = newSpeed;
    }

}