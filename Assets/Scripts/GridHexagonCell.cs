using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHexagonCell : MonoBehaviour
{
    private HexStack stack;
    public bool isOccupied
    {
        get => stack != null;
        private set { }
    }

    public void AssignStack(HexStack stack)
    {
        this.stack = stack;
    }
}
