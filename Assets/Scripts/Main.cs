using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private MainCharacter Character;

    private Inventory _inventory;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(typeof(string[,]).IsSerializable);
            //CollectedObjectConfig.FindConfigByName(" ");
            _inventory.ToggleViewVisibility();
        }
    }

    private void Start()
    {
        _inventory = new Inventory();
        Character.OnCollect += _inventory.TryAdd;
    }
}
