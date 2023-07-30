using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class IconScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public FruitData fruitData;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject confirmButton;
    public TMP_Text quantityText;
    [SerializeField] private float hoverCountdown;
    [SerializeField] private float dragCountdown;
    private GameObject movingIcon;
    private float hoverTimer;
    private float dragTimer;
    private bool hasCursor;
    private bool isDragging;
    private bool canThrow;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            dragTimer += Time.deltaTime;
            if (dragTimer >= dragCountdown)
            {
                if (movingIcon == null)
                {
                    movingIcon = new GameObject("IMG_MovingIcon");
                    movingIcon.AddComponent<Image>().sprite = icon.sprite;
                    movingIcon.transform.SetParent(InventoryManager.instance.canvasRT.transform);
                    movingIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(icon.GetComponent<RectTransform>().rect.width, icon.GetComponent<RectTransform>().rect.height);
                    movingIcon.GetComponent<Image>().raycastTarget = false;
                }
                // Get the mouse position in screen coordinates
                Vector2 mousePositionScreen = Input.mousePosition;

                // Convert the mouse position from screen coordinates to the local space of the parent
                Vector2 mousePositionParent;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), mousePositionScreen, null, out mousePositionParent);

                // Convert the mouse position from screen coordinates to canvas coordinates
                Vector2 mousePositionCanvas;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), mousePositionScreen, null, out mousePositionCanvas);
                // Set the image's local position to the converted mouse position within the parent's local space
                icon.transform.localPosition = mousePositionParent;
                movingIcon.GetComponent<RectTransform>().anchoredPosition = mousePositionCanvas;
            }
            
        }
        else
        {
            if (hasCursor)
            {
                hoverTimer += Time.deltaTime;
                if (hoverTimer >= hoverCountdown)
                {
                    InventoryManager.instance.SpawnDescription(fruitData);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    confirmButton.SetActive(false);
                }
            }
        }

        
    }

    public void InitialiseIcon()
    {
        icon.sprite = fruitData.fruitImage;
        gameObject.name = "Icon_" + fruitData.name;
        confirmButton.SetActive(false);
        //icon.GetComponent<FruitIconScript>().iconScript = this;
    }

    public void DescriptionCountdown()
    {
        hasCursor = true;
    }

    public void DesscriptionCountdownRevert()
    {
        hoverTimer = 0;
        hasCursor = false;
        InventoryManager.instance.DespawnDescription();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionCountdown();
        InventoryManager.instance.currentHover = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTimer = 0;
        hasCursor = false;
        InventoryManager.instance.DespawnDescription();
        InventoryManager.instance.currentHover = null;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        isDragging = true;
        hoverTimer = 0;
        hasCursor = false;
        InventoryManager.instance.DespawnDescription();
        //icon.transform.SetParent(InventoryManager.instance.canvasRT.transform);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragTimer = 0;
        isDragging = false;
        // icon.transform.SetParent(this.gameObject.transform);
        icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        if (movingIcon != null)
        {
            Destroy(movingIcon);
            movingIcon = null;
        }
        if (canThrow)
        {
            InventoryManager.instance.DropItems(fruitData);
        }
        if (InventoryManager.instance.currentHover != null)
        {
            int index = InventoryManager.instance.currentHover.transform.GetSiblingIndex();
            int originalIndex = transform.GetSiblingIndex();
            InventoryManager.instance.currentHover.transform.SetSiblingIndex(originalIndex);
            transform.SetSiblingIndex(index);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        confirmButton.SetActive(true);
        hasCursor = true;
    }

    public void Consume()
    {
        Debug.Log("Consume");
        InventoryManager.instance.Consume(fruitData);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isDragging)
        {
            if (collision.transform == this.gameObject.transform.parent)
            {
                canThrow = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDragging)
        {
            if (collision.transform == this.gameObject.transform.parent)
            {
                canThrow = false;
            }
        }

    }
}
