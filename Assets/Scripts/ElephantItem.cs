using UnityEngine;

[CreateAssetMenu(fileName = "New Elephant Item", menuName = "ScriptableObjects/ElephantItem")]
public class ElephantItem : ScriptableObject
{
    public string itemName = "Elephant";
    public Sprite itemImage;  // Assign in the Unity Editor
    public string itemDescription = "A majestic elephant.";
    public GameObject item3DModel;  // Assign in the Unity Editor
}
