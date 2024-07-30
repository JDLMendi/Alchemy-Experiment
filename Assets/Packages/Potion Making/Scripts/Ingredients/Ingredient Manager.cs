using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class IngredientManager : MonoBehaviour
{
    public TMP_Text sceneText;
    public CauldronController cauldronController;
    public string pathToPotionRecipes;
    public ToggleGroup[] toggleGroups;
    public GameObject targetObject;
    public Potion resultPotion;
    public float scale = 1;

    private Vector3 _finalScale;
    private Toggle[][] _toggles;
    private Toggle[] _selectedToggles;
    private Dictionary<Potion, Ingredient[]> _potionRecipes;

    // Loads the recipes and ensures that the toggles are populated and non-used toggles are non-interactable
    void Start()
    {
        _finalScale = new Vector3(scale, scale, scale);
        _toggles = toggleGroups.Select(g => g.GetComponentsInChildren<Toggle>()).ToArray();
        _selectedToggles = new Toggle[toggleGroups.Length];
        InitializeToggles();
        LoadPotionRecipes();
    }

    // Ensures that each toggle button enable the 'Selected' child when isOn is true by adding a listener to them
    private void InitializeToggles()
    {
        for (int i = 0; i < toggleGroups.Length; i++)
        {
            foreach (var toggle in _toggles[i])
            {
                int index = i;
                toggle.onValueChanged.AddListener(isSelected => OnToggleValueChanged(toggle, index, isSelected));
                toggle.isOn = false;
            }
        }
    }

    // Handle toggle button value changes
    private void OnToggleValueChanged(Toggle changedToggle, int groupIndex, bool isSelected)
    {
        if (isSelected) _selectedToggles[groupIndex] = changedToggle;
    }

    // Load potion recipes from resources folder (should be the Potions folder within Resources)
    public void LoadPotionRecipes()
    {
        _potionRecipes = Resources.LoadAll<Potion>(pathToPotionRecipes)
            .Where(p => p != null && p.ingredients != null)
            .ToDictionary(p => p, p => p.ingredients);
    }

    // Load ingredients into a specific toggle group
    public void LoadIngredients(Ingredient[] ingredients, int groupIndex)
    {
        if (groupIndex >= toggleGroups.Length) return;

        var groupToggles = toggleGroups[groupIndex].GetComponentsInChildren<Toggle>();
        for (int i = 0; i < groupToggles.Length; i++)
        {
            var toggle = groupToggles[i];
            var ingredientSlot = toggle.transform.Find("Ingredient")?.GetComponent<IngredientSlot>();
            if (ingredientSlot != null && i < ingredients.Length)
            {
                ingredientSlot.SetIngredient(ingredients[i]);
                toggle.interactable = true;
            }
        }
        UpdateToggleInteractability();
    }

    // Mix selected ingredients and attempt to create a potion
    public Potion MixIngredients()
    {
        // Find the toggle in each group and form an ingredient array that is selected. There is only three ingredients to chose from.
        Ingredient[] selectedIngredients = _selectedToggles
            .Where(t => t != null && t.isOn)
            .Select(t => t.transform.Find("Ingredient")?.GetComponent<IngredientSlot>()?.ingredient)
            .ToArray();

        // Just in case, if there should only be three ingredients chosen.
        if (selectedIngredients.Length != 3)
        {
            StartCoroutine(FadeText("I need a base, effect, and a bottle...", 2f, 2f));
            return null;
        }

        // Calls IsValidRecipe to find the potion that has the ingredients (excluding bottle)
        resultPotion = IsValidRecipe(selectedIngredients);
        if (resultPotion != null)
        {
            var ingredientNames = string.Join(", ", selectedIngredients.Select(i => i.itemName));
            StartCoroutine(FadeText($"I'm mixing {ingredientNames}", 2f, 1f));
            StartCoroutine(FadeText($"{resultPotion.name}...", 2f, 2f));
            StartCoroutine(MoveIngredientsToPoint());
            return resultPotion;
        }

        StartCoroutine(FadeText("Can't do that.", 2f, 2f));
        return null;
    }

    // Check if the selected ingredients form a valid recipe by iterating through each key-value and see if the provided ingredients matches the ingredients within the value
    private Potion IsValidRecipe(Ingredient[] ingredients)
    {
        foreach (var potionEntry in _potionRecipes)
        {
            var recipeIngredients = potionEntry.Value;
            if (ingredients.Count(i => recipeIngredients.Contains(i)) >= 2)
            {
                return potionEntry.Key;
            }
        }
        sceneText.text = "No valid potion found.";
        return null;
    }

    // Update toggle interactability based on ingredient availability
    private void UpdateToggleInteractability()
    {
        foreach (var toggleGroup in toggleGroups)
        {
            foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
            {
                var ingredientSlot = toggle.transform.Find("Ingredient")?.GetComponent<IngredientSlot>();
                toggle.interactable = ingredientSlot?.ingredient != null;
            }
        }
    }

    // Move selected ingredients to the target position provided by the target transform
    private IEnumerator MoveIngredientsToPoint()
    {
        // Iterate through each toggle and see if an 'Ingredient' child object is available.
        foreach (var toggle in _selectedToggles)
        {
            if (toggle != null)
            {
                Transform ingredientTransform = toggle.transform.Find("Ingredient");
                if (ingredientTransform != null)
                {
                    yield return StartCoroutine(MoveToPositionAndScale(ingredientTransform, targetObject.transform.position, _finalScale, 1f));

                    var ingredientSlot = ingredientTransform.GetComponent<IngredientSlot>();
                    if (ingredientSlot != null) ingredientSlot.ApplyFallSprite(); // This calls a funcction within the SO to check if it has a falling sprite assigned

                    // Allows the ingredient to fall
                    Rigidbody2D rb = ingredientTransform.GetComponent<Rigidbody2D>();
                    if (rb != null) rb.simulated = true;

                    if (ingredientSlot?.ingredient?.itemtype == itemType.Bottle)
                    {
                        // If the ingredient is a bottle, then it has a Potion child attached to the same parent
                        Transform potionTransform = ingredientTransform.parent?.Find("Potion");
                        if (potionTransform != null)
                        {
                            // This would cause the cauldron to change colour to the resulting potion's colour and then restart the cauldron to its original colour
                            yield return new WaitForSeconds(0.5f);
                            cauldronController.ToPotionColour(resultPotion.color);
                            yield return new WaitForSeconds(2f);
                            potionTransform.GetComponent<PotionSlot>().SetPotion(resultPotion);
                            potionTransform.gameObject.SetActive(true);
                            cauldronController.RestartCauldron();
                        }
                    }
                    // Once the ingredient has been used, toggle becomes non-interactable
                    toggle.interactable = false;
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }

    // Move and scale a transform over a duration 
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

    // Fade in and out a text message that is displayed in the scene
    private IEnumerator FadeText(string message, float fadeInTime, float fadeOutTime)
    {
        sceneText.color = new Color(sceneText.color.r, sceneText.color.g, sceneText.color.b, 0);
        sceneText.text = message;

        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            sceneText.color = new Color(sceneText.color.r, sceneText.color.g, sceneText.color.b, elapsedTime / fadeInTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        sceneText.color = new Color(sceneText.color.r, sceneText.color.g, sceneText.color.b, 1);

        yield return new WaitForSeconds(1f);

        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            sceneText.color = new Color(sceneText.color.r, sceneText.color.g, sceneText.color.b, 1 - elapsedTime / fadeOutTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        sceneText.color = new Color(sceneText.color.r, sceneText.color.g, sceneText.color.b, 0);
    }
}
