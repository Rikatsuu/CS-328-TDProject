using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateCharger : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float firstSpeed = 4f;
    [SerializeField] private float secondSpeed = 8f;
    [SerializeField] private float firstChangeTime = 2f;
    [SerializeField] private float secondChangeTime = 5f;

    [Header("Sprites")]
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private Sprite firstStage;
    [SerializeField] private Sprite secondStage;

    private SpriteRenderer spriteRenderer;
    private float elapsedTime = 0f;
    private Movement movementScript;
    private bool firstChangeDone = false;
    private bool secondChangeDone = false;

    private void Awake()
    {
        movementScript = GetComponent<Movement>();
        if(movementScript == null)
        {
            Debug.LogError("Movement not found");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer not found");
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if(!firstChangeDone && elapsedTime >= firstChangeTime)
        {
            movementScript.updateSpeed(firstSpeed);
            spriteRenderer.sprite = firstStage;
            firstChangeDone = true;
        }

        if(!secondChangeDone && elapsedTime >= secondChangeTime)
        {
            movementScript.updateSpeed(secondSpeed);
            spriteRenderer.sprite = secondStage;
            secondChangeDone = true;
        }
    }
}

