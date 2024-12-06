//Plot.cs - handles plot behavior 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    private GameObject tower;
    private Color startColor;
    public Turret turret;
    public bool IsOccupied => tower != null;

    void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor; //changes plot to a certain color when hovered over
    }

    private void OnMouseExit()
    {
        sr.color = startColor; //resets plot to original color
    }

    private void OnMouseDown()
    {
        if (IsOccupied)
        {
            turret.openUpgradeMenu();
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > Level1.main.currency)
        {
            Debug.Log("brokie");
            return;
        }

        Level1.main.spendCurrency(towerToBuild.cost);

    }

    public void placeTower(GameObject placedTower)
    {
        tower = placedTower;
        turret = tower.GetComponent<Turret>();

        if(turret != null)
        {
            turret.isPlaced = true;
        }

        tower.transform.SetParent(transform);
    }
}
