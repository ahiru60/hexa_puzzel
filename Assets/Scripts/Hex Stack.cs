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
    }

    internal void place()
    {
        foreach(Hexagon hexagon in Hexagons)
        {
            hexagon.DisableCollider();
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
