using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<ElephantItem> elephants = new List<ElephantItem>();
    [SerializeField] private List<GiraffeItem> giraffes = new List<GiraffeItem>();
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private ItemButtonManager itemButtonManager;

    private void Start()
    {
        GameManager.instance.OnItemsMenu += CreateButtons;
    }

    private void CreateButtons()
    {
        // Create buttons for elephants
        foreach (var elephant in elephants)
        {
            CreateItemButton(elephant);
        }

        // Create buttons for giraffes
        foreach (var giraffe in giraffes)
        {
            CreateItemButton(giraffe);
        }

        // Unsubscribe from event to prevent multiple button creations
        GameManager.instance.OnItemsMenu -= CreateButtons;
    }

    private void CreateItemButton(ScriptableObject item)
    {
        ItemButtonManager itemButton;
        itemButton = Instantiate(itemButtonManager, buttonContainer.transform);
        itemButton.ItemName = item.name;
        itemButton.ItemDescription = GetItemDescription(item);
        itemButton.ItemImage = GetItemImage(item);
        itemButton.Item3DModel = GetItem3DModel(item);
        itemButton.name = item.name;
    }

    private string GetItemDescription(ScriptableObject item)
    {
        if (item is ElephantItem elephantItem)
        {
            return elephantItem.itemDescription;
        }
        else if (item is GiraffeItem giraffeItem)
        {
            return giraffeItem.itemDescription;
        }
        else
        {
            return "No description available";
        }
    }

    private Sprite GetItemImage(ScriptableObject item)
    {
        if (item is ElephantItem elephantItem)
        {
            return elephantItem.itemImage;
        }
        else if (item is GiraffeItem giraffeItem)
        {
            return giraffeItem.itemImage;
        }
        else
        {
            return null;
        }
    }

    private GameObject GetItem3DModel(ScriptableObject item)
    {
        if (item is ElephantItem elephantItem)
        {
            return elephantItem.item3DModel;
        }
        else if (item is GiraffeItem giraffeItem)
        {
            return giraffeItem.item3DModel;
        }
        else
        {
            return null;
        }
    }
}
