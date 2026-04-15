using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private new Renderer renderer;

    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
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
