using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StackController : MonoBehaviour
{

    [Header(" Settings ")]
    [SerializeField] private LayerMask hexagonLayerMask;
    [SerializeField] private LayerMask gridHexagonLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    private HexStack currentHexStack;
    private Vector3 currentHexStackInitialPos;


    [Header(" Data ")]
    private GridHexagonCell targetCell;

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        ManageControl();
    }

    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ManageMouseDown();
        }
        else if (Input.GetMouseButton(0) && currentHexStack != null)
        {
            ManageMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0) && currentHexStack != null)
        {
            ManageMouseUp();
        }
    }
    private void ManageMouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(getClickedRay(), out hit, 500, hexagonLayerMask);
        Debug.Log("Clicked");
        if (hit.collider == null)
        {
            Debug.Log("hexagon not detected");
            return;
        }

        currentHexStack = hit.collider.GetComponent<Hexagon>().hexStack;
        currentHexStackInitialPos = currentHexStack.transform.position;
    }
    private void ManageMouseUp()
    {
        if(targetCell == null)
        {
            Debug.Log(" Target Cell null ");
            currentHexStack.transform.position = currentHexStackInitialPos;
            currentHexStack = null;
            return;
        }

        currentHexStack.transform.position = targetCell.transform.position.With(y: 0);
        currentHexStack.transform.SetParent(targetCell.transform);
        currentHexStack.place();

        targetCell.AssignStack(currentHexStack);

        targetCell = null;
        currentHexStack = null;
    }

    private void ManageMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(getClickedRay(), out hit, 500, gridHexagonLayerMask);

        if(hit.collider == null)
        {
            DraggingAboveGround();
        }
        else
        {
            DraggingAboveGridCell(hit);
        }

    }

    private void DraggingAboveGridCell(RaycastHit hit)
    {
        GridHexagonCell gridHexagon = hit.collider.GetComponent<GridHexagonCell>();
        if (gridHexagon.isOccupied)
        {
            DraggingAboveGround();
        }
        else
        {
            Debug.Log("Dragging above the grid cell");
            DraggingAboveNonOccupiedGrid(gridHexagon);
        }
    }

    private void DraggingAboveNonOccupiedGrid(GridHexagonCell gridHexagon)
    {
        Vector3 currentStackTargetPos = gridHexagon.transform.position.With(y: 2);
        currentHexStack.transform.position = Vector3.MoveTowards(
           currentHexStack.transform.position,
           currentStackTargetPos, Time.deltaTime * 30);

        targetCell = gridHexagon;

    }

    private void DraggingAboveGround()
    {
        RaycastHit hit;
        Physics.Raycast(getClickedRay(), out hit, 500, groundLayerMask);


        if(hit.collider == null)
        {
            Debug.Log("No ground detected");
            return;
        }

        Vector3 currentStackTargetPos = hit.point.With(y: 2);
        currentHexStack.transform.position = Vector3.MoveTowards(
            currentHexStack.transform.position,
            currentStackTargetPos, Time.deltaTime * 30);
        
        targetCell = null;
    }

    

    private Ray getClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
}
