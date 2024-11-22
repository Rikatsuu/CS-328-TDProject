using UnityEngine;
using System;

[Serializable]
public class Tower : MonoBehaviour

{
    public string towerName;
    public int cost;
    public GameObject prefab;
    public bool isPlaced = false;

    // Constructor for initializing a new tower
    public Tower(string _name, int _cost, GameObject _prefab)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
    }

    // Method to activate the tower after it is placed
    public void ActivateTower()
    {
        isPlaced = true;

        // Find and activate relevant components like shooting
        Turret turretScript = this.gameObject.GetComponent<Turret>();  // Access current object, not prefab
        if (turretScript != null)
        {
            turretScript.isPlaced = true;  // Activate the turret functionality
        }
    }

    // Method to deactivate the tower (e.g., during dragging)
    public void DeactivateTower()
    {
        isPlaced = false;

        // Find and deactivate relevant components like shooting
        Turret turretScript = this.gameObject.GetComponent<Turret>();  // Access current object
        if (turretScript != null)
        {
            turretScript.isPlaced = false;  // Deactivate the turret functionality
        }
    }

}
