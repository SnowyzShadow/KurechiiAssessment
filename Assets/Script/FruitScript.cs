using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    public FruitData fruitType;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = fruitType.fruitImage;
        gameObject.name = fruitType.fruitName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
