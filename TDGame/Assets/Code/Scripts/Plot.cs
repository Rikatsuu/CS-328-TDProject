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

    
    // Start is called before the first frame update
    void Start()
    {
        startColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null) return;
        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > Level1.main.currency)
        {
            Debug.Log("brokie");
            return;
        }

        Level1.main.spendCurrency(towerToBuild.cost);

    }
}
