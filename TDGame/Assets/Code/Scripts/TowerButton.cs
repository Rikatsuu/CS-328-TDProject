using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject towerPrefab;
    private GameObject draggedTower;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Instantiate the tower prefab to drag it from the shop
        draggedTower = Instantiate(towerPrefab);
        draggedTower.GetComponent<Collider2D>().enabled = false;  // Disable collisions for dragging

        // Get the cost from the specific turret component (e.g., MachineGunMango)
        Turret turretScript = draggedTower.GetComponent<Turret>();
        if (turretScript != null && Level1.main.spendCurrency(turretScript.towerCost))
        {
            // Allow the drag if there's enough currency
        }
        else
        {
            Debug.Log("Not enough currency to place this tower.");
            Destroy(draggedTower);  // Cancel dragging if no currency
            draggedTower = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // If no tower is being dragged, skip dragging logic
        if (draggedTower == null) return;

        // Follow the mouse position while dragging
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set z to 0 if it's a 2D game
        draggedTower.transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // If no tower is being dragged, skip placement logic
        if (draggedTower == null) return;

        // Place it at the current mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Optionally, check if it's placed on a valid spot
        if (IsValidPlacement(mousePosition))
        {
            // Snap the tower to the center of the plot
            mousePosition = SnapToPlotCenter(mousePosition);
            draggedTower.transform.position = mousePosition;
            draggedTower.GetComponent<Collider2D>().enabled = true;  // Enable collisions again

            // Activate the Tower (ActivateTower in Tower.cs)
            Tower towerScript = draggedTower.GetComponent<Tower>();
            if (towerScript != null)
            {
                towerScript.ActivateTower();  // Ensure the tower gets activated after placement
            }
        }
        else
        {
            // Destroy the tower if placed in an invalid position and refund the cost
            Turret turretScript = draggedTower.GetComponent<Turret>();
            if (turretScript != null)
            {
                Level1.main.increaseCurrency(turretScript.towerCost);  // Refund the currency if placement was invalid
            }
            Destroy(draggedTower);
        }
    }

    private bool IsValidPlacement(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Plot"))
        {
            return true;
        }
        return false;
    }

    private Vector3 SnapToPlotCenter(Vector3 position)
    {
        // Cast a ray to detect the plot
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        // Check if the raycast hit a plot
        if (hit.collider != null && hit.collider.CompareTag("Plot"))
        {
            // Return the center of the plot (using the plot's collider or transform position)
            return hit.collider.bounds.center;  // Snaps to the center of the plot's collider
        }

        // If no valid plot is found, return the original position
        return position;
    }
}
