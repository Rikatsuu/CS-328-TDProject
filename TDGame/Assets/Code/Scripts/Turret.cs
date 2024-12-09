using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform towerRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float offset = 125f;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attribute")]
    [SerializeField] private float targetRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bulletsPerSecond = 2f;

    [Header("Tower Attributes")]
    public int towerCost = 175;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer; 
    [SerializeField] private Sprite upgradedSprite; 


    [Header("Detection")]
    [SerializeField] private bool camoDetection = false;

    private Transform target;
    private float timeUntilFire;
    public bool isPlaced = false;
    
    public bool isStunned = false;

    void Start()
    {
        isPlaced = false;
        
    }

    private void Update()
    {

        if (!isStunned) //executes if the tower is not in a stunned state
        {
            if (isPlaced) //ensures that tower only activates when it is properly placed 
            {
                if (target == null)
                {
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
                    timeUntilFire += Time.deltaTime;
                    if (timeUntilFire >= 2f / bulletsPerSecond)
                    {
                        bulletFire();
                        timeUntilFire = 0f;
                    }
                }
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{

    //    Handles.color = Color.cyan;
    //    Handles.DrawWireDisc(transform.position, transform.forward, targetRange);

    //}

    private void findTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetRange, (Vector2)transform.position, 0f, enemyMask);


        foreach(var hit in hits)
        {
            GameObject enemy = hit.transform.gameObject;
            if(enemy.CompareTag("Camo") && !camoDetection)
            {
                continue;
            }
            target = enemy.transform;
            break;
        }
    }

    //allowers turret to visually turn towards target, so the bullets dont just spawn where it doesnt make sense
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - offset;
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

    //adding a stunned state for the boss functionality
    public void Stun(float stunDuration)
    {
        if (!isStunned)
        {
            isStunned = true;
            StartCoroutine(stunState(stunDuration));
        }
    }

    private IEnumerator stunState(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    public void openUpgradeMenu()
    {
        upgradeUI.SetActive(true);
    }

    public void closeUpgradeMenu()
    {
        upgradeUI.SetActive(false);
    }

    public void UpgradeTower()
    {
        bulletsPerSecond += 10f;

        spriteRenderer.sprite = upgradedSprite;
        Level1.main.spendCurrency(75);

        closeUpgradeMenu();
    }

    public void upgradeSniper()
    {
        bulletsPerSecond += 1f;
        Level1.main.spendCurrency(125);
        closeUpgradeMenu();
    }

    public void SellTower()
    {

        if (Level1.main != null)
        {
            int sellPrice = Mathf.FloorToInt(towerCost * 0.5f);
            Level1.main.increaseCurrency(sellPrice);
        }

        Destroy(this.gameObject);
        Debug.Log("Sold");
        closeUpgradeMenu();
    }

    private void OnMouseDown()
    {
        if (isPlaced) // Ensure the tower has been placed
        {
            openUpgradeMenu(); // Open the upgrade menu when clicked
            Debug.Log("Tower clicked!");
        }
    }
}
