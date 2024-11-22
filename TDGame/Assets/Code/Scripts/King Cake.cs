//KingCake.cs - handles boss behavior

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCake : MonoBehaviour
{

    [Header("Phase Attributes")]
    [SerializeField] private Sprite[] phaseSprites;
    [SerializeField] private float[] phaseSpeeds;
    [SerializeField] private Vector3[] phaseScales;

    [Header("Candle Attributes")]
    [SerializeField] private Sprite[] candleSprites; //sprites for candle phases when they break
    [SerializeField] private CircleCollider2D stunCollider; //collider to change per phase
    [SerializeField] private float stunDuration = 10f;

    //intializing game objects of other types
    private Movement movement;
    private SpriteRenderer spriteRenderer;
    private Health health;
    private int currentPhase = 0;

    private void Start()
    {
        movement = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();

        stunCollider = transform.Find("stun").GetComponent<CircleCollider2D>();

    }



    private void Update()
    {
        if (currentPhase == 0 && health.currentHealth <= 75) { //condition to transition to next phase
            enterNextPhase(0); //begins second phase
            currentPhase = 1;

            //handles candle phase changes
            if (health.currentHealth <= 55)
            {
                enterNextCandlePhase(0, stunDuration);
            }
            if (health.currentHealth <= 35)
            {
                enterNextCandlePhase(1, stunDuration);
            }
        }
        else if (currentPhase == 1 && health.currentHealth <= 25) //transition to last phase
        {
            enterNextPhase(1);

            //handle candle phase changes
            if (health.currentHealth <= 20)
            {
                enterNextCandlePhase(2, stunDuration);
            }
            if (health.currentHealth <= 15)
            {
                enterNextCandlePhase(3, stunDuration);
            }
            if (health.currentHealth <= 10)
            {
                enterNextCandlePhase(4, stunDuration);
            }
            if (health.currentHealth <= 5)
            {
                enterNextCandlePhase(5, stunDuration);
            }
        }
    }

    //enters next stage of boss
    private void enterNextPhase(int newPhase)
    {
        spriteRenderer.sprite = phaseSprites[newPhase]; //change sprite 
        movement.updateSpeed(phaseSpeeds[newPhase]); //change movement speed
        transform.localScale = phaseScales[newPhase]; //change size
    }

    //stuns tower when enough damage is taken
    public void stunTower(float stunTime)
    {
        StartCoroutine(stunTimer(stunTime));
        stunTowersInRange(stunTime);
    }

    IEnumerator stunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
    }

    //detects towers in collider range to stun 
    public void stunTowersInRange(float stunTime)
    {
        if(stunCollider == null) //checks that stun collider is present
        {
            Debug.LogError("No stun collider");
            return;
        }

        Collider2D[] hitTowers = new Collider2D[20]; //creating an array of towers to store that are in collider range
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Tower")); //ensures taht only objects with "tower" layer are detected by collider
        filter.useTriggers = true;

        int hits = stunCollider.OverlapCollider(filter, hitTowers);

        //iterates through the array to stun each tower that exists
        for(int i = 0; i < hits; i++)
        {
            Turret turret = hitTowers[i].GetComponent<Turret>();
            if(turret != null)
            {
                turret.Stun(stunTime);
                Debug.Log("Tower is Stunned!");
            }
        }
    }

    //function to handle candle phase transitions
    private void enterNextCandlePhase(int newCandlePhase, float stunTime)
    {
        Debug.Log("Stunned");
        stunTowersInRange(stunTime); //calls function above 
        spriteRenderer.sprite = candleSprites[newCandlePhase]; //changes sprite based on provided index
    }

}
