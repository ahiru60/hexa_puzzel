using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StackSpawner : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Transform stackPositionParent;
    [SerializeField] private Hexagon hexagon;
    [SerializeField] private float hexagonHeight;
    [SerializeField] private HexStack hexagonStack;


    [Header(" Settings ")]
    [SerializeField] private Color[] colors;
    [NaughtyAttributes.MinMaxSlider(2,8)]
    [SerializeField] private Vector2Int minMaxHexCount;
    private int stackCounter;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        StackController.onStackPlaced += StackPlacedCallback;
    }

    private void OnDestroy()
    {
        StackController.onStackPlaced -= StackPlacedCallback;
    }

    private void StackPlacedCallback(GridHexagonCell cell)
    {
        stackCounter++;

        if(stackCounter >= 3)
        {
            stackCounter = 0;
            GenerateStacks();
        }
    }

    void Start()
    {
        GenerateStacks();
    }

    private void GenerateStacks()
    {
        for(int i = 0; i < stackPositionParent.childCount; ++i)
        {
            GenerateStack(stackPositionParent.GetChild(i));
        }
    }

    private void GenerateStack(Transform parent)
    {
        HexStack hexStack = Instantiate(hexagonStack, parent.position,Quaternion.identity,parent);
        hexStack.name = $"Stack {parent.GetSiblingIndex()}";

        Color stackColor = colors[Random.Range(0,colors.Length)];

        int amount = Random.Range(minMaxHexCount.x, minMaxHexCount.y);

        int firstColorHexagonCount = Random.Range(0, amount);

        Color[] colorArray = GetRandomColors();

        for(int i = 0; i < amount; i++)
        {
            Vector3 hexagonLocalPos = Vector3.up * i * hexagonHeight;
            Vector3 spawnPosition = hexStack.transform.TransformPoint(hexagonLocalPos);
            Hexagon hexagonInstance = Instantiate(hexagon, spawnPosition, Quaternion.identity, hexStack.transform);
            hexagonInstance.Color = i < firstColorHexagonCount ? colorArray[0] : colorArray[1];
            hexagonInstance.Configure(hexStack);
            hexStack.Add(hexagonInstance);

        }
    }

    private Color[] GetRandomColors()
    {
        List<Color> colorList = new List<Color>();
        colorList.AddRange(colors);

        if(colorList.Count <= 0)
        {
            Debug.LogError("No color found");
            return null;
        }

        Color firstColor = colorList.OrderBy(x => Random.value).First();
        colorList.Remove(firstColor);
        if (colorList.Count <= 0)
        {
            Debug.LogError("Only one color was found");
            return null;
        }

        Color secondColor = colorList.OrderBy(x => Random.value).First();

        return new Color[] {firstColor,secondColor};

    }
}
