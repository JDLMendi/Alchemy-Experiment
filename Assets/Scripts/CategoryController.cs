using UnityEngine;
using UnityEngine.UI;

public class CategoryController : MonoBehaviour
{
    public GameObject slots;
    public ToggleGroup toggleGroup;       // Reference to the ToggleGroup

    void Start()
    {
        // Register to listen for toggle changes
        foreach (Toggle toggle in slots.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(delegate
            {
                OnToggleValueChanged(toggle);
            });
        }

        // Initialize the states based on the initial toggle states
        UpdateToggleStates();
    }

    // Method called whenever a toggle's value changes
    void OnToggleValueChanged(Toggle changedToggle)
    {
        UpdateToggleStates();
    }

    // Method to update the states for all toggles
    void UpdateToggleStates()
    {
        foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            GameObject selectedObject = toggle.transform.Find("Selected").gameObject;
            if (toggle.isOn)
            {
                selectedObject.SetActive(true);  // Activate the 'Selected' object when the toggle is active
            }
            else
            {
                selectedObject.SetActive(false); // Deactivate the 'Selected' object when the toggle is inactive
            }
        }
    }
}
