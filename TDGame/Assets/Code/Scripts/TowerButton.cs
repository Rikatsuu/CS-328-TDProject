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
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Follow the mouse position while dragging
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set z to 0 if it's a 2D game
        draggedTower.transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Here you could snap the tower to a valid place in the scene
        // For now, just place it at the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Optionally, check if it's placed on a valid spot
        if (IsValidPlacement(mousePosition))
        {
            // Finalize the tower placement
            draggedTower.transform.position = mousePosition;
            draggedTower.GetComponent<Collider2D>().enabled = true;  // Enable collisions again
        }
        else
        {
            // Destroy the tower if placed in an invalid position
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
}