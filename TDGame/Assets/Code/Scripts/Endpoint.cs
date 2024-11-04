using UnityEngine;

public class Endpoint : MonoBehaviour
{
    public int damageAmount = 1;  // Amount of health to deduct when an enemy reaches the endpoint

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is an enemy
        if (other.CompareTag("Enemy"))
        {
            // Deduct health from the player
            Level1.main.DeductHealth(damageAmount);

            // Optionally destroy the enemy or handle it as needed
            Destroy(other.gameObject);
        }
    }
}