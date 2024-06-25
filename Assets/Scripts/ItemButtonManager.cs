using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonManager : MonoBehaviour
{
    private string itemName;
    public string ItemName { set => itemName = value; }

    private string itemDescription;
    public string ItemDescription { set => itemDescription = value; }

    private string price;
    public string Price { set => price = value; }

    private Sprite itemImage;
    public Sprite ItemImage { set => itemImage = value; }

    private GameObject item3DModel;
    public GameObject Item3DModel { set => item3DModel = value; }

    private ARInteractionManager interactionManager;

    private void Start()
    {
        // Set item name text
        var nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (nameText != null)
        {
            nameText.text = itemName;
        }

        // Set item image
        var rawImage = transform.GetChild(1).GetComponent<RawImage>();
        if (rawImage != null && itemImage != null)
        {
            rawImage.texture = itemImage.texture;
        }

        // Set item description text
        var descriptionText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        if (descriptionText != null)
        {
            descriptionText.text = itemDescription;
        }

        // Add button listeners
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => GameManager.instance.ARPosition());
            button.onClick.AddListener(Create3DModel);
        }

        interactionManager = FindObjectOfType<ARInteractionManager>();
    }

    private void Create3DModel()
    {
        if (interactionManager != null && item3DModel != null)
        {
            interactionManager.Item3DModel = Instantiate(item3DModel);
        }
    }
}
