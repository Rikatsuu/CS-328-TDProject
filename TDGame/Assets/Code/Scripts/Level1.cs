using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level1 : MonoBehaviour
{
    // Start is called before the first frame update
    public static Level1 main;

    public Transform startPoint;
    public Transform[] path;

    public int currency;

    private void Start()
    {
        currency = 100;
    }
    private void Awake()
    {
        main = this;
    }

    public void increaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool spendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("get a job");
            return false;
        }
    }
}