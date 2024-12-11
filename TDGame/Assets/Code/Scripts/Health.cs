//manages the health of enemies

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float hp = 2;
    [SerializeField] private int currencyWorth = 25;

    private bool isDestroyed = false;
    private JukingJellybean bean;

    private void Start()
    {
        bean = GetComponent<JukingJellybean>();
    }

    public void TakeDamage(float dmg)
    {
        // Check if the enemy dodges
        if (bean.TryDodge())
        {
            return;
        }

        hp -= dmg;
        if (hp <= 0 && !isDestroyed)
        {
            Spawner.onEnemyDestroy.Invoke();
            Level1.main.increaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    public float currentHealth
    {
        get { return hp; }
    }
}