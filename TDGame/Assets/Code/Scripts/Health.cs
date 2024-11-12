using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float hp = 2;
    [SerializeField] private int currencyWorth = 25;

    private bool isDestroyed = false;
    
    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0 && !isDestroyed)
        {
            Spawner.onEnemyDestroy.Invoke();
            Level1.main.increaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    public void enemyDamage(Enemy enemy)
    {
        TakeDamage(enemy.damage);
    }

}
