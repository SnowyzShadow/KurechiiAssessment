using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class IconScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public FruitData fruitData;
    [SerializeField] private Image icon;
    public TMP_Text quantityText;
    [SerializeField] private float hoverCountdown;
    private float hoverTimer;
    private bool hasCursor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCursor)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= hoverCountdown)
            {
                InventoryManager.instance.SpawnDescription(fruitData);
            }
        }
    }

    public void InitialiseIcon()
    {
        icon.sprite = fruitData.fruitImage;
        gameObject.name = "Icon_" + fruitData.name;
        Debug.Log(fruitData.fruitName);
    }

    public void DescriptionCountdown()
    {
        hasCursor = true;
        
    }

    public void DescriptionCountdownRevert()
    {
        hoverTimer = 0;
        hasCursor = false;
        InventoryManager.instance.DespawnDescription();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionCountdown();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionCountdownRevert();
    }
}
