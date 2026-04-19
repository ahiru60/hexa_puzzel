using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexStack : MonoBehaviour
{
    public List<Hexagon> Hexagons {  get; private set; }
    public void Add(Hexagon hexagon)
    {
        if(Hexagons == null) {  Hexagons = new List<Hexagon>(); }
        Hexagons.Add(hexagon);

        hexagon.SetParent(transform);
    }

    

    internal Color GetTopHexColor()
    {
        return Hexagons[^1].Color;
    }

    internal void place()
    {
        foreach(Hexagon hexagon in Hexagons)
        {
            hexagon.DisableCollider();
        }
    }
    internal bool Contains(Hexagon hexagon)
    {
        return Hexagons.Contains(hexagon);
    }
    internal void Remove(Hexagon hexagon)
    {
        Hexagons.Remove(hexagon);
        if(Hexagons.Count == 0)
        {
            DestroyImmediate(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
