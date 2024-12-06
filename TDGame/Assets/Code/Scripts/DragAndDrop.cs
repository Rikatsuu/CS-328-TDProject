//controls the drag and drop mechanics from the shop to the plot

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

    private SpriteRenderer spriteRenderer;
    public Color validPlacement = Color.green;
    public Color invalidPlacement = Color.red;

    void Start()
    {
        towerScript = GetComponent<Tower>();
        towerScript.isPlaced = false;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) //determines if the placement is valid
        {
            spriteRenderer.color = invalidPlacement;
        }
    }

    void OnMouseDown() //click
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

    void OnMouseUp() //click release
    {
        isDragging = false;
        towerScript.isPlaced = true;

        Vector3 mousePosition = GetMouseWorldPosition();
        if (IsValidPlacement(mousePosition))
        {
            Vector3 snappedPosition = SnapToBlock(mousePosition);
            GameObject placedTower = Instantiate(towerPrefab, snappedPosition, Quaternion.identity);

            //Get the Tower script from the instantiated object and activate it

            RaycastHit2D hit = Physics2D.Raycast(snappedPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Plot"))
            {
                Plot plot = hit.collider.GetComponent<Plot>();
                if(plot != null)
                {
                    plot.placeTower(placedTower);

                    Tower tower = placedTower.GetComponent<Tower>();
                    if(tower != null)
                    {
                        tower.ActivateTower();
                    }
                }
            }

            //destroy the dragged sprite since the tower is now placed
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);  //or reset to initial position, if required such as invalid placement
        }
    }



    void Update()
    {
        if (isDragging)
        {
            //follow the mouse position while dragging
            transform.position = GetMouseWorldPosition() + offset;
        }
        if (IsValidPlacement(transform.position)) //valid placement logic
        {
            spriteRenderer.color = validPlacement;
        }
        else
        {
            spriteRenderer.color = invalidPlacement;
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
        //checking for a valid placement, using a raycast or collision detection
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Plot"))
        {
            return true;
        }

        return false;

    }

    private Vector3 SnapToBlock(Vector3 position)
    {
        float gridSize = 1f;

        //Calculate the snapped position by rounding to the nearest grid size
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;

        //Return the snapped position (center of the grid)
        return new Vector3(snappedX, snappedY, position.z);
    }
}