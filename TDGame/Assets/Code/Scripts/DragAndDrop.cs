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

        Vector3 mousePosition = GetMouseWorldPosition();
        if (IsValidPlacement(mousePosition))
        {
            Vector3 snappedPosition = SnapToBlock(mousePosition);
            Instantiate(towerPrefab, snappedPosition, Quaternion.identity);

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
        // Implement logic to snap to the block center
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Plot"))
        {
            return hit.collider.transform.position;  // Snap to the block's position
        }

        return position;  // If no valid block is found, just return the original position
    }
}