using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private new Renderer renderer;
    [SerializeField] private new Collider collider;
    public HexStack hexStack { get; private set; }

    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
    }

    public void Configure(HexStack hexStack)
    {
        this.hexStack = hexStack;
    }

    internal void DisableCollider()
    {
        collider.enabled = false;
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
