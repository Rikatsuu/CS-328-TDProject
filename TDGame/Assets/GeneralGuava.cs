using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGuava : MonoBehaviour
{
    [Header("Boost Attributes")]
    [SerializeField] private float rangeBoost = 2f;
    [SerializeField] private float damageBoost = 0.1f;
    [SerializeField] private float boostRadius = 50f;
    [SerializeField] private LayerMask towerLayer;

    private List<Turret> boostedTowers = new List<Turret>();
    public bool isPlaced = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced)
        {
            UpdateBoosts();
        }
    }

    public void EnableBoost()
    {
        isPlaced = true;
    }

    private void UpdateBoosts()
    {
        Collider2D[] towersInRange = Physics2D.OverlapCircleAll(transform.position, boostRadius, towerLayer);

        foreach (var collider in towersInRange)
        {
            Turret turret = collider.GetComponent<Turret>();
            if (turret != null && !boostedTowers.Contains(turret))
            {
                float currentRange = turret.GetTargetRange();

                turret.SetTargetRange(currentRange + rangeBoost);

                boostedTowers.Add(turret);

                Bullet bulletPrefab = turret.getBulletPrefab().GetComponent<Bullet>();
                if (bulletPrefab != null)
                {
                    bulletPrefab.BulletDamage += damageBoost;
                    Debug.Log($"{name}: Damage boost applied to bullets fired by {turret.name}. New damage: {bulletPrefab.BulletDamage}");
                }

                Debug.Log($"{name}: Boost applied to {turret.name}. New range: {turret.GetTargetRange()}");
            }
        }

        //remove boosts
        boostedTowers.RemoveAll(turret =>
        {
            if (turret == null || Vector2.Distance(turret.transform.position, transform.position) > boostRadius)
            {
                if (turret != null)
                {
                    float currentRange = turret.GetTargetRange();
                    turret.SetTargetRange(currentRange - rangeBoost);

                    Bullet bulletPrefab = turret.getBulletPrefab().GetComponent<Bullet>();
                    if (bulletPrefab != null)
                    {
                        bulletPrefab.BulletDamage -= damageBoost;
                        Debug.Log($"{name}: Damage boost removed for bullets fired by {turret.name}. Restored damage: {bulletPrefab.BulletDamage}");
                    }

                    Debug.Log($"{name}: Boost removed from {turret.name}. Restored range: {turret.GetTargetRange()}");
                }
                return true;
            }
            return false;
        });
    }

    private void OnDestroy()
    {
        foreach (var turret in boostedTowers)
        {
            if (turret != null)
            {
                float currentRange = turret.GetTargetRange();
                turret.SetTargetRange(currentRange - rangeBoost);
                Bullet bulletPrefab = turret.getBulletPrefab().GetComponent<Bullet>();
                if (bulletPrefab != null)
                {
                    bulletPrefab.BulletDamage -= damageBoost;
                    Debug.Log($"{name}: Damage boost reverted for bullets fired by {turret.name}. Restored damage: {bulletPrefab.BulletDamage}");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, boostRadius);
    }
}