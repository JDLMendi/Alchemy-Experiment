using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import the TMPro namespace for TextMeshPro

public class CategoryController : MonoBehaviour
{
    public GameObject slots;
    public ToggleGroup toggleGroup;
    public TMP_Text itemNameText; // Reference to TMP_Text for displaying item name

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
        UpdateItemNameText(changedToggle);
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

    // Method to update the TMP_Text with the item name of the selected toggle
    void UpdateItemNameText(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            Transform ingredientTransform = changedToggle.transform.Find("Ingredient");
            if (ingredientTransform != null)
            {
                IngredientSlot ingredientSlot = ingredientTransform.GetComponent<IngredientSlot>();
                if (ingredientSlot != null)
                {
                    // Set the text to the itemName of the ingredient
                    itemNameText.text = ingredientSlot.ingredient.itemName;
                }
                else
                {
                    Debug.LogWarning("IngredientSlot component not found on " + ingredientTransform.gameObject.name);
                }
            }
            else
            {
                Debug.LogWarning("Ingredient child not found in toggle " + changedToggle.gameObject.name);
            }
        }
        else
        {
            // Optionally clear the text if the toggle is not selected
            itemNameText.text = "";
        }
    }
}
