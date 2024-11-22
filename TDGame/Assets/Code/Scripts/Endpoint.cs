//endpoint; manages what happens if the enemy reaches the end

using UnityEngine;

public class Endpoint : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Deduct health from the player
            Level1.main.DeductHealth(damageAmount);
            Spawner.onEnemyDestroy.Invoke();
            Destroy(other.gameObject);
        }
    }
}