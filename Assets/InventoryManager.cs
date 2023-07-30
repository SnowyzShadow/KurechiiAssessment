using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private List<FruitInventory> fruitInventoryList = new List<FruitInventory>();
    private List<IconScript> inventoryIconList = new List<IconScript>();
    [System.Serializable]
    public class FruitInventory
    {
        public FruitData fruit;
        public int quantity;
    }
    public RectTransform canvasRT;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject iconParent;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private GameObject iconConfirmPrefab;
    [SerializeField] private GameObject descriptionGameObject;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject infoText;
    [SerializeField] private Transform infoParent;
    public IconScript currentHover;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        descriptionGameObject.SetActive(false);
        inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    public void CollectFruit(GameObject fruitCollided)
    {
        bool hasFruit = false;
        FruitInventory tempFruit = FindFruit(fruitCollided.GetComponent<FruitScript>().fruitType);
        if (tempFruit != null)
        {
            tempFruit.quantity += 1;
            hasFruit = true;
            IconScript tempIcon = FindIcon(fruitCollided.GetComponent<FruitScript>().fruitType);
            if (tempIcon != null)
            {
                tempIcon.quantityText.text = tempFruit.quantity.ToString();
            }
        }
        //foreach (FruitInventory inventory in fruitInventoryList)
        //{
        //    if (inventory.fruit == fruitCollided.GetComponent<FruitScript>().fruitType)
        //    {
        //        inventory.quantity += 1;
        //        foreach (IconScript icon in inventoryIconList)
        //        {
        //            if (icon.fruitData == fruitCollided.GetComponent<FruitScript>().fruitType)
        //            {
        //                icon.quantityText.text = inventory.quantity.ToString();
        //                break;
        //            }

        //        }
        //        hasFruit = true;
        //        break;
        //    }
        //}
        if (!hasFruit)
        {
            FruitInventory tempInfo = new FruitInventory();
            tempInfo.fruit = fruitCollided.GetComponent<FruitScript>().fruitType;
            tempInfo.quantity = 1;
            fruitInventoryList.Add(tempInfo);
            SpawnIcon(fruitCollided.GetComponent<FruitScript>().fruitType);
        }
    }
    private void SpawnIcon(FruitData fruitData)
    {
        GameObject instant = Instantiate(iconPrefab, iconParent.transform);
        instant.GetComponent<IconScript>().fruitData = fruitData;
        instant.GetComponent<IconScript>().InitialiseIcon();
        instant.GetComponent<IconScript>().quantityText.text = "1";
        inventoryIconList.Add(instant.GetComponent<IconScript>());
    }

    public void SpawnDescription(FruitData fruitData)
    {
        if (!descriptionGameObject.activeSelf)
        {
            descriptionGameObject.SetActive(true);
            descriptionText.text = fruitData.fruitDescription;
        }
    }

    public void DespawnDescription()
    {
        descriptionGameObject.SetActive(false);
    }

    public void DropItems(FruitData fruitData)
    {
        FruitInventory tempFruit = FindFruit(fruitData);
        if (tempFruit != null)
        {
            tempFruit.quantity = 0;
            IconScript tempIcon = FindIcon(fruitData);
            if (tempIcon != null)
            {
                inventoryIconList.Remove(tempIcon);
                Destroy(tempIcon.gameObject);
            }
            fruitInventoryList.Remove(tempFruit);
            GameObject tempInfo = Instantiate(infoText, infoParent);
            Transform[] allChildren = tempInfo.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<TMP_Text>() != null)
                {
                    child.GetComponent<TMP_Text>().text = fruitData.name + " dropped!";
                }
            }
            Destroy(tempInfo, 2f);
        }
        //foreach (FruitInventory inventory in fruitInventoryList)
        //{
        //    if (inventory.fruit == fruitData)
        //    {
        //        inventory.quantity = 0;
        //        foreach (IconScript icon in inventoryIconList)
        //        {
        //            if (icon.fruitData == fruitData)
        //            {
        //                inventoryIconList.Remove(icon);
        //                Destroy(icon.gameObject);
        //                break;
        //            }

        //        }
        //        fruitInventoryList.Remove(inventory);
        //        break;
        //    }
        //}
    }

    public void Consume(FruitData fruitData)
    {
        FruitInventory tempFruit = FindFruit(fruitData);
        if (tempFruit != null)
        {
            tempFruit.quantity -= 1;
            IconScript tempIcon = FindIcon(fruitData);
            if (tempIcon != null)
            {
                if (tempFruit.quantity == 0)
                {
                    inventoryIconList.Remove(tempIcon);
                    Destroy(tempIcon.gameObject);
                    fruitInventoryList.Remove(tempFruit);
                }
                else
                {
                    tempIcon.quantityText.text = tempFruit.quantity.ToString();
                }
            }
            GameObject tempInfo = Instantiate(infoText, infoParent);
            Transform[] allChildren = tempInfo.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<TMP_Text>() != null)
                {
                    child.GetComponent<TMP_Text>().text = fruitData.name + " consumed!";
                }
            }
            Destroy(tempInfo, 2f);
        }
    }

    private FruitInventory FindFruit(FruitData fruitData)
    {
        foreach (FruitInventory inventory in fruitInventoryList)
        {
            if (inventory.fruit == fruitData)
            {
                return inventory;
            }
        }
        return null;
    }

    private IconScript FindIcon(FruitData fruitData)
    {
        foreach (IconScript icon in inventoryIconList)
        {
            if (icon.fruitData == fruitData)
            {
                return icon;
            }
        }
        return null;
    }
}