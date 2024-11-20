using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform towerRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bulletsPerSecond = 2f;

    [Header("Tower Attributes")]
    public int towerCost = 175;

    private Transform target;
    private float timeUnitlFire;
    public bool isPlaced = false;


    // Start is called before the first frame update
    void Start()
    {
        isPlaced = false;
    }

    private void Update()
    {
 
        if (isPlaced)
        {
            if(target == null){
                findTarget();
                return;
            }

            RotateTowardsTarget();

            if (!CheckTargetIsInRange())
            {
                target = null;
            }
            else
            {
                timeUnitlFire += Time.deltaTime;
                if (timeUnitlFire >= 2f / bulletsPerSecond)
                {
                    bulletFire();
                    timeUnitlFire = 0f;
                }
            }
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

    private void bulletFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript =  bullet.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

}
