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
    [SerializeField] private GameObject iconParent;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private GameObject iconConfirmPrefab;
    [SerializeField] private GameObject descriptionGameObject;
    [SerializeField] private TMP_Text descriptionText;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectFruit(GameObject fruitCollided)
    {
        bool hasFruit = false;
        foreach (FruitInventory inventory in fruitInventoryList)
        {
            if (inventory.fruit == fruitCollided.GetComponent<FruitScript>().fruitType)
            {
                inventory.quantity += 1;
                foreach (IconScript icon in inventoryIconList)
                {
                    if (icon.fruitData == fruitCollided.GetComponent<FruitScript>().fruitType)
                    {
                        icon.quantityText.text = inventory.quantity.ToString();
                    }
                }
                hasFruit = true;
            }
        }
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
        Debug.Log(fruitData.fruitName);
        GameObject instant= Instantiate(iconPrefab, iconParent.transform);
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
}
