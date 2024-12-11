using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowliPop : MonoBehaviour
{
    //Add an animation for hypnosis 

    [SerializeField] private CircleCollider2D slowCollider;
    [SerializeField] private float slowFactor = 0.1f;

    private HashSet<Turret> affectedTowers = new HashSet<Turret>();

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            Turret turret = collision.GetComponent<Turret>();
            if(turret != null && !affectedTowers.Contains(turret))
            {
                turret.applySlow(slowFactor);
                affectedTowers.Add(turret);
                Debug.Log("Slowed");
            } 
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            Turret turret = collision.GetComponent<Turret>();
            if(turret != null && affectedTowers.Contains(turret))
            {
                turret.removeSlow(slowFactor);
                affectedTowers.Remove(turret);
                Debug.Log("Normal");
            }
        }
    }
}

