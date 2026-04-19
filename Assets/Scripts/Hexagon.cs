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

    public void SetParent(Transform parent)
    {
        transform.parent = parent; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void MoveToLocal(Vector3 targetLocalPosition)
    {
        float delay = transform.GetSiblingIndex() * .01f;
        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, targetLocalPosition, .2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);

        Vector3 direction = (targetLocalPosition - transform.localPosition).With(y:0).normalized;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
        LeanTween.rotateAround(gameObject, rotationAxis, 180, .2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);
    }

    internal void Vanish(float delay)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, .2f)
            .setEase(LeanTweenType .easeInBack)
            .setDelay(delay)
            .setOnComplete(()=> {
                Destroy(gameObject);
            });
    }
}
