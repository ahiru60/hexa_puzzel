using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHexagonCell : MonoBehaviour
{
    public HexStack stack { get; private set; }
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
