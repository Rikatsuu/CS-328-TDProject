using UnityEngine;

public class Endpoint : MonoBehaviour
{
    public int damageAmount = 1; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Deduct health from the player
            Level1.main.DeductHealth(damageAmount);

            // Notify the Spawner that an enemy has reached the endpoint
            Spawner.onEnemyDestroy.Invoke();

            // Destroy the enemy to remove it from the scene
            Destroy(other.gameObject);
        }
    }
}
