using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private int health = 150;
    [SerializeField] private int layers = 2;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float scaleReduction = 0.8f;
    [SerializeField] private float speedIncrease = 1.5f;

    private int healthPerLayer; //change to two layers with their own unique healths
    private int currentHealth;
    private int currentLayer;


    private void Start()
    {
        currentHealth = health;
        healthPerLayer = health / layers;
        currentLayer = layers;
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= (currentLayer - 1) * healthPerLayer)
        {
            breakLayer();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void breakLayer()
    {
        currentLayer--;
        transform.localScale *= scaleReduction;
        speed *= speedIncrease;

        //explodeCandle();

        Debug.Log("Layer broken");
    }

    private void Die()
    {
        Debug.Log("King cake defeated!");
        Destroy(gameObject);
    }

    /*private void explodeCandle()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hitColliders)
        {
            Tower tower = hit.GetComponent<Tower>();
            if(tower != null)
            {
                StartCoroutine(stunTower(tower));
            }
        }
        Debug.Log("Candle Exploded");
    }

    private IEnumerator stunTower(Tower tower)
    {
        tower.Stun(true);
        yield return new WaitForSeconds(stunDuration);
        tower.Stun(false);
    }*/

}
