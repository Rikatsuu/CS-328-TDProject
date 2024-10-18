using UnityEngine;
using System;

[Serializable]
public class Tower
{

    public string name;
    public int cost;
    public GameObject prefab;
    public bool isPlaced = false; 

    public Tower (string _name, int _cost, GameObject _prefab)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
