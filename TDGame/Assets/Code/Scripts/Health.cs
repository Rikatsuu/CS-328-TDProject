using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float hp = 2;
    [SerializeField] private int currencyWorth = 50;

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




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
