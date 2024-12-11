//Tower.cs - class for dealing with tower behavior relate to shop

using UnityEngine;
using System;

[Serializable]
public class Tower : MonoBehaviour

{
    public string towerName;
    public int cost;
    public GameObject prefab;
    public bool isPlaced = false;

    //constructor for initializing a new tower
    public Tower(string _name, int _cost, GameObject _prefab) 
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
    }

    //function to activate the tower after it is placed
    public void ActivateTower()
    {
        isPlaced = true;

        //activates shooting 
        Turret turretScript = this.gameObject.GetComponent<Turret>();
        if (turretScript != null)
        {
            turretScript.isPlaced = true; 
        }
    }

    //function to deactivate tower 
    public void DeactivateTower()
    {
        isPlaced = false;

        Turret turretScript = this.gameObject.GetComponent<Turret>();
        if (turretScript != null)
        {
            turretScript.isPlaced = false; 
        }
    }

}
