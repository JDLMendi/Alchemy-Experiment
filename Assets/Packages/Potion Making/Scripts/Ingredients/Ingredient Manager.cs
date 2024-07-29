using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IngredientManager : MonoBehaviour
{
    public ToggleGroup[] toggleGroups; // Assign these in the Inspector
    public GameObject targetObject; // Assign this in the Inspector

    public float scale = 1;
    private Vector3 finalScale; // Final scale for the ingredients

    private Toggle[][] toggles;
    private Toggle[] selectedToggles;

    void Start()
    {
        finalScale = new Vector3(scale, scale, scale);
        toggles = new Toggle[toggleGroups.Length][];
        selectedToggles = new Toggle[toggleGroups.Length];

        for (int i = 0; i < toggleGroups.Length; i++)
        {
            toggles[i] = toggleGroups[i].GetComponentsInChildren<Toggle>();
            int index = i;
            foreach (var toggle in toggles[i])
            {
                toggle.onValueChanged.AddListener((isSelected) => OnToggleValueChanged(toggle, index, isSelected));
            }
        }
    }

    private void OnToggleValueChanged(Toggle changedToggle, int groupIndex, bool isSelected)
    {
        if (isSelected)
        {
            selectedToggles[groupIndex] = changedToggle;
        }
    }

    public Toggle[] GetSelectedToggles()
    {
        return selectedToggles;
    }

    public void MixIngredients()
    {
        if (isValidRecipe())
        {
            string ingredientNames = "Mixing the following ingredients: ";
            string bottleName = string.Empty;

            foreach (var toggle in selectedToggles)
            {
                if (toggle != null)
                {
                    Transform ingredientTransform = toggle.transform.Find("Ingredient");
                    if (ingredientTransform != null)
                    {
                        IngredientSlot ingredient = ingredientTransform.GetComponent<IngredientSlot>();
                        if (ingredient != null)
                        {
                            if (ingredient.ingredient.itemName == "Bottle")
                            {
                                bottleName = ingredient.ingredient.itemName;
                            }
                            else
                            {
                                ingredientNames += ingredient.ingredient.itemName + ", ";
                            }
                        }
                        else
                        {
                            Debug.LogWarning("IngredientSlot component not found on " + ingredientTransform.gameObject.name);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Ingredient child not found in toggle " + toggle.gameObject.name);
                    }
                }
            }

            // Remove the trailing comma and space if needed
            if (ingredientNames.EndsWith(", "))
            {
                ingredientNames = ingredientNames.Substring(0, ingredientNames.Length - 2);
            }

            Debug.Log(ingredientNames);

            // Start the coroutine to move ingredients and enable Rigidbody2D
            StartCoroutine(MoveIngredientsToPoint());
        }
    }

    // Should it read a file containing all the valid recipe for the night?
    public bool isValidRecipe()
    {
        return true;
    }

    private IEnumerator MoveIngredientsToPoint()
    {
        foreach (var toggle in selectedToggles)
        {
            if (toggle != null)
            {
                Transform ingredientTransform = toggle.transform.Find("Ingredient");
                if (ingredientTransform != null)
                {
                    // Move the ingredient to the target object's position and scale it
                    yield return StartCoroutine(MoveToPositionAndScale(ingredientTransform, targetObject.transform.position, finalScale, 1.0f)); // Adjust the duration as needed

                    // Enable Rigidbody2D to let it fall
                    Rigidbody2D rb = ingredientTransform.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.simulated = true;
                    }

                    // Set the toggle as inactive
                    toggle.interactable = false;

                    // Wait for a short time before moving the next ingredient
                    yield return new WaitForSeconds(0.5f); // Adjust the delay as needed
                }
            }
        }
    }

    // Moves the ingredient to the Drop Point position and scale it during the movement
    private IEnumerator MoveToPositionAndScale(Transform transform, Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.localScale = targetScale;
    }
}
