using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukingJellybean : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float dodgeChance = 0.8f;

    public bool TryDodge()
    {
        if (Random.value <= dodgeChance)
        {
            Debug.Log("Dodged");
            return true;
        }
        else
        {
            return false;
        }
    }
}