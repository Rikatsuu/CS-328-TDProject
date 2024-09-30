using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turrent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform towerRotationPoint;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attribute")]
    [SerializeField] private float targetRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update(){
        if(target == null){
            findTarget();
            return;
        }

        RotateTowardsTarget();

       if(!CheckTargetIsInRange())
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected(){
        
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetRange); 
    
    }

    private void findTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0) {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 125f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        towerRotationPoint.rotation = Quaternion.RotateTowards(towerRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetRange;
    }
}
