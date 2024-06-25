using UnityEngine;

[CreateAssetMenu(fileName = "New Giraffe Item", menuName = "ScriptableObjects/GiraffeItem")]
public class GiraffeItem : ScriptableObject
{
    public string itemName = "Giraffe";
    public Sprite itemImage;  // Assign in the Unity Editor
    public string itemDescription = "A graceful giraffe.";
    public GameObject item3DModel;  // Assign in the Unity Editor
}
