//TowerButton.cs - script to handle behavior for tower in the shop.

using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    public GameObject towerPrefab;
    private GameObject draggedTower;

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedTower = Instantiate(towerPrefab);               //instantiates the tower prefab to drag it from the shop
        draggedTower.GetComponent<Collider2D>().enabled = false;  // Disable collisions for dragging

        //gets the cost of the selected tower
        Turret turretScript = draggedTower.GetComponent<Turret>();
        if (turretScript != null && Level1.main.spendCurrency(turretScript.towerCost))
        {
            //if the player has enough money, allow drag
        }
        else
        {
            Debug.Log("Not enough seeds to place this tower.");
            Destroy(draggedTower);  //if the player does not have enough money, it won't allow the player to drag and place
            draggedTower = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedTower == null) return;

        //ensures that the sprite follows the mouse position wherever it goes
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; //z=0 because we are in 2d
        draggedTower.transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedTower == null) return;

        //places tower at current mouse position when lifted up
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        //check if the tower is being placed in a valid spot
        if (IsValidPlacement(mousePosition))
        {
            //ensures that the tower snaps to the center wherever it is placed on a valid spot
            mousePosition = SnapToPlotCenter(mousePosition);
            draggedTower.transform.position = mousePosition;
            draggedTower.GetComponent<Collider2D>().enabled = true;  //enables tower functionality

            //activate tower when all conditions are met
            Tower towerScript = draggedTower.GetComponent<Tower>();
            if (towerScript != null)
            {
                towerScript.ActivateTower();  
            }
        }
        else
        {
            //if tower is placed in an invalid spot, it destroys the object and refunds the currency
            Turret turretScript = draggedTower.GetComponent<Turret>();
            if (turretScript != null)
            {
                Level1.main.increaseCurrency(turretScript.towerCost);  //refund currency 
            }
            Destroy(draggedTower);
        }
    }

    //function to check if the tower is in a proper spot
    private bool IsValidPlacement(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Plot")) //achieves this by checking if the tag is a plot
        {
            return true;
        }
        return false;
    }

    //function to make the tower snap to the center of the plot
    private Vector3 SnapToPlotCenter(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Plot"))
        {
            return hit.collider.bounds.center;  //snaps to the center of the plot's collider
        }

        //if no valid plot is found, return the original position
        return position;
    }
}
