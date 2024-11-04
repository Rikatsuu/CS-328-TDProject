using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    Transform parentAfterDrag;
    private Tower towerScript;
    public GameObject towerPrefab;

    void Start()
    {
        towerScript = GetComponent<Tower>();
        towerScript.isPlaced = false;
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = gameObject.transform.position - GetMouseWorldPosition();
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        transform.SetAsLastSibling();

        towerScript.isPlaced = false;
        Collider2D collider = GetComponentInChildren<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        towerScript.isPlaced = true;

        Vector3 mousePosition = GetMouseWorldPosition();
        if (IsValidPlacement(mousePosition))
        {
            Vector3 snappedPosition = SnapToBlock(mousePosition);
            GameObject placedTower = Instantiate(towerPrefab, snappedPosition, Quaternion.identity);

            // Get the Tower script from the instantiated object and activate it
            Tower tower = placedTower.GetComponent<Tower>();
            if (tower != null)
            {
                tower.ActivateTower();
            }

            // Destroy the dragged sprite since the tower is now placed
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);  // or reset to initial position, if required
        }
    }



    void Update()
    {
        if (isDragging)
        {
            // Follow the mouse position while dragging
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private bool IsValidPlacement(Vector3 position)
    {
        // Example of checking for a valid placement, e.g., using a raycast or collision detection
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Plot"))  // Assuming your valid blocks are tagged "Plot"
        {
            return true;
        }

        return false;
    }

    private Vector3 SnapToBlock(Vector3 position)
    {
        // Define the size of your grid (adjust this to your grid size)
        float gridSize = 1f;  // Example: 1 unit per grid cell

        // Calculate the snapped position by rounding to the nearest grid size
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;

        // Return the snapped position (center of the grid)
        return new Vector3(snappedX, snappedY, position.z);  // Z remains the same
    }

}