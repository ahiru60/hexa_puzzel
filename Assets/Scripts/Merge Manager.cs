using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{


    [Header(" Elements ")]
    private List<GridHexagonCell> updatedCells = new List<GridHexagonCell>();

    private void Awake()
    {
        StackController.onStackPlaced += StackPlacedCallback;
    }
    private void OnDestroy()
    {
        StackController.onStackPlaced -= StackPlacedCallback;
    }

    private void StackPlacedCallback(GridHexagonCell gridCell)
    {
        StartCoroutine(
            StackPlacedCoroutine(gridCell));
        
    }

    IEnumerator StackPlacedCoroutine(GridHexagonCell gridCell)
    {

        updatedCells.Add(gridCell);
        while (updatedCells.Count > 0)
        {
            yield return CheckForMerge(updatedCells[0]); 
        }
        yield return CheckForMerge(gridCell);
    }

    IEnumerator CheckForMerge(GridHexagonCell gridCell)
    {
        updatedCells.Remove(gridCell);
        // Dose this cell has neigbhors
        List<GridHexagonCell> neighborGridCells = GetNeighborGridCell(gridCell);

        if (neighborGridCells.Count <= 0)
        {
            Debug.Log("No neighbor for this cell");
            yield break;
        }

        Color gridCellTopHexagonColor = gridCell.stack.GetTopHexColor();

        // Do these naibhors hs the same top hex color
        List<GridHexagonCell> similarNeighborGridCells = getSimilarNeighborGridCells(gridCellTopHexagonColor, neighborGridCells.ToArray());

        if (similarNeighborGridCells.Count <= 0)
        {
            Debug.Log("No similar neighbors detected for this cell");
            yield break;
        }
        updatedCells.AddRange(similarNeighborGridCells);
        List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(gridCellTopHexagonColor, similarNeighborGridCells.ToArray());

        Debug.Log($"Here we need to add {hexagonsToAdd.Count}");

        // We need to merge

        // removing hex
        removeHexFromStack(hexagonsToAdd, similarNeighborGridCells.ToArray());

        // add hex
        MoveHexToStack(gridCell, hexagonsToAdd);

        yield return new WaitForSeconds(.2f + (hexagonsToAdd.Count+1) * .01f);

        // merge everything inside this cell
       yield return CheckForCompleteStack(gridCell, gridCellTopHexagonColor);
    }
    private IEnumerator CheckForCompleteStack(GridHexagonCell gridCell,Color gridCellTopHexColor)
    {
        if(gridCell.stack.Hexagons.Count < 10)
        {
            yield break;
        }
        List<Hexagon> similarHexagons = new List<Hexagon>();
        for(int i = gridCell.stack.Hexagons.Count - 1; i >= 0; i--)
        {
            Debug.Log("for loop");
            Hexagon hex = gridCell.stack.Hexagons[i];
            if(hex.Color != gridCellTopHexColor)
            {
                break;
            }
            similarHexagons.Add(hex);
        }

        int similarHexCount = similarHexagons.Count;
        if (similarHexagons.Count < 10)
        {
            yield break;
        }
        float delay = 0;
        while (similarHexagons.Count > 0)
        {
            similarHexagons[0].SetParent(null);
            similarHexagons[0].Vanish(delay);
            delay += .01f;
            gridCell.stack.Remove(similarHexagons[0]);
            similarHexagons.RemoveAt(0);
        }

        updatedCells.Add(gridCell);

        yield return new WaitForSeconds(.2f+(similarHexCount+1) * .01f);
    }

    private void MoveHexToStack(GridHexagonCell gridCell, List<Hexagon> hexagonsToAdd)
    {
        float initialY = gridCell.stack.Hexagons.Count * .2f;

        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];
            float targetY = initialY + i * .2f;
            Vector3 targetLocalPosition = Vector3.up * targetY;

            gridCell.stack.Add(hexagon);
            hexagon.MoveToLocal(targetLocalPosition);
            //hexagon.transform.localPosition = targetLocalPosition;
        }
    }

    private void removeHexFromStack(List <Hexagon> hexagonsToAdd, GridHexagonCell[] similarNeighborGridCells)
    {
        foreach (GridHexagonCell neighborCell in similarNeighborGridCells)
        {
            HexStack stack = neighborCell.stack;
            foreach (Hexagon hexagon in hexagonsToAdd)
            {
                if (stack.Contains(hexagon))
                {

                    stack.Remove(hexagon);

                }
            }
        }
    }

    private List<Hexagon> GetHexagonsToAdd(Color gridCelllTopHexagonColor, GridHexagonCell[] similarNeighborGridCells)
    {
      List<Hexagon> hexagonsToAdd =  new List<Hexagon>();

        foreach (GridHexagonCell naighborCell in similarNeighborGridCells)
        {
            HexStack neiborCellHexStack = naighborCell.stack;

            for (int i = neiborCellHexStack.Hexagons.Count - 1; i >= 0; i--)
            {

                Hexagon hexagon = neiborCellHexStack.Hexagons[i];

                if (hexagon.Color != gridCelllTopHexagonColor)
                {
                    break;
                }
                hexagonsToAdd.Add(hexagon);
                hexagon.SetParent(null);
            }
        }

        return hexagonsToAdd;
    }

    private List<GridHexagonCell> getSimilarNeighborGridCells(Color gridCelllTopHexagonColor, GridHexagonCell[] neighborGridCells)
    {
        List<GridHexagonCell> similarNeighborGridCells = new List<GridHexagonCell>();

        foreach (GridHexagonCell col in neighborGridCells)
        {
            Color neiborGridCellTopHexagonColor = col.stack.GetTopHexColor();

            if (gridCelllTopHexagonColor == neiborGridCellTopHexagonColor)
            {
                similarNeighborGridCells.Add(col);
            }
        };

        return similarNeighborGridCells;
    }

    private List<GridHexagonCell> GetNeighborGridCell(GridHexagonCell gridCell)
    {
        LayerMask gridCellMask = 1 << gridCell.gameObject.layer;

      List<GridHexagonCell> neighborGridCells =  new List<GridHexagonCell>();
        Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position, 2, gridCellMask);

        // At this point, we have the grid cell collider neighbors
        foreach (Collider col in neighborGridCellColliders)
        {
            GridHexagonCell neighborGridCell = col.GetComponent<GridHexagonCell>();

            if (!neighborGridCell.isOccupied)
            {
                continue;
            }
            if (neighborGridCell == gridCell)
            {
                continue;
            }
            neighborGridCells.Add(neighborGridCell);
        }

        return neighborGridCells;
    }
}
