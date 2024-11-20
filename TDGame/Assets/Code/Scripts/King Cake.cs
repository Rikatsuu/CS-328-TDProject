using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCake : MonoBehaviour
{

    [Header("Phase Attributes")]
    [SerializeField] private Sprite[] phaseSprites;
    [SerializeField] private float[] phaseSpeeds;
    [SerializeField] private Vector3[] phaseScales;
    //[SerializeField] private Sprite[] candleSprites; //sprites for candle phases when they break

    //[SerializeField] private float stunDuration = 3f;

    private Movement movement;
    private SpriteRenderer spriteRenderer;
    private Health health;
    private int currentPhase = 0;

    private void Start()
    {
        movement = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();

    }

    private void Update()
    {
        if(currentPhase == 0 && health.currentHealth <= 75){
            enterNextPhase(0);
            currentPhase = 1;
        }
        else if(currentPhase == 1 && health.currentHealth <= 25)
        {
            enterNextPhase(1);
        }
    }

    private void enterNextPhase(int newPhase)
    {
        //Debug.Log($"Entering next phase {(newPhase + 1)}");

        spriteRenderer.sprite = phaseSprites[newPhase];
        movement.updateSpeed(phaseSpeeds[newPhase]);
        transform.localScale = phaseScales[newPhase];

    }
      
    //TODO 2: Assign health the same way to candles. 
    //        If 25 damage is taken, explode a candle and update sprite. Also add a stun effect to towers 
   
    //ALGORITHM:
    // private void breakCandle
    // Input: takes the candle number so it can call the sprite and update the prefab 
    // Using serializeFields, fill it up with candles broken per phase 
    //     - you will need to figure out how many candles will be removed in each phase 
    // scaling will not be needed, so the only line of code needed is: -- spriteRenderer.sprite.phaseSprites[input]
    // this function needs to be called everytime 25 damage is taken to the boss, so figure out how to integrate that 
    // should also STUN 

}
