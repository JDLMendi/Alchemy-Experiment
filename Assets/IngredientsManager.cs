using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientManager : MonoBehaviour
{
    public Ingredient[] effectIngredients;
    public Ingredient[] baseIngredients;
    public Ingredient[] bottles;
    public Ingredient[] special;

    public Ingredient[] craftingIngredients;

    private Ingredient[][] _ingredients;
    private int currentCategory = 0;
    private int currentIndex = 0;

    private GameObject previousSelectedObject;

    private const int maxCraftingIngredients = 4; // Assuming one from each category
    private int craftingIngredientIndex = 0;

    void Start()
    {
        _ingredients = new Ingredient[4][];
        _ingredients[0] = effectIngredients;
        _ingredients[1] = baseIngredients;
        _ingredients[2] = bottles;
        _ingredients[3] = special;

        craftingIngredients = new Ingredient[maxCraftingIngredients];

        UpdateSelection();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput);
        if (context.phase == InputActionPhase.Performed)
        {
            HandleMoveInput(moveInput);
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            HandleClickInput();
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SelectCurrentIngredient();
        }
    }

    private void HandleMoveInput(Vector2 moveInput)
    {
        if (moveInput.y > 0)
        {
            MoveRight();
        }
        else if (moveInput.y < 0)
        {
            MoveLeft();
        }

        if (moveInput.x < 0)
        {
            MoveUp();
        }
        else if (moveInput.x > 0)
        {
            MoveDown();
        }
    }

    private void HandleClickInput()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D clickedCollider = Physics2D.OverlapPoint(worldPosition);
        if (clickedCollider != null)
        {
            Ingredient clickedIngredient = clickedCollider.GetComponent<Ingredient>();
            if (clickedIngredient != null)
            {
                UpdateSelection(clickedIngredient);
            }
        }
    }

    private void MoveUp()
    {
        currentIndex = (currentIndex - 1 + _ingredients[currentCategory].Length) % _ingredients[currentCategory].Length;
        UpdateSelection();
    }

    private void MoveDown()
    {
        currentIndex = (currentIndex + 1) % _ingredients[currentCategory].Length;
        UpdateSelection();
    }

    private void MoveLeft()
    {
        currentCategory = (currentCategory - 1 + _ingredients.Length) % _ingredients.Length;
        currentIndex = Mathf.Min(_ingredients[currentCategory].Length - 1, currentIndex);
        UpdateSelection();
    }

    private void MoveRight()
    {
        currentCategory = (currentCategory + 1) % _ingredients.Length;
        currentIndex = Mathf.Min(_ingredients[currentCategory].Length - 1, currentIndex);
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        if (previousSelectedObject != null)
        {
            Transform previousSelectedTransform = previousSelectedObject.transform.Find("Selected");
            if (previousSelectedTransform != null)
            {
                previousSelectedTransform.gameObject.SetActive(false);
            }
        }

        Ingredient currentIngredient = _ingredients[currentCategory][currentIndex];
        GameObject currentSelectedObject = currentIngredient.gameObject; // Assuming the Ingredient script is attached to the GameObject

        Transform currentSelectedTransform = currentSelectedObject.transform.Find("Selected");
        if (currentSelectedTransform != null)
        {
            currentSelectedTransform.gameObject.SetActive(true);
        }

        previousSelectedObject = currentSelectedObject;

        // Implement additional logic if needed
        Debug.Log($"Selected {currentIngredient.name}");
    }

    private void UpdateSelection(Ingredient ingredient)
    {
        // Find the category and index of the clicked ingredient
        for (int category = 0; category < _ingredients.Length; category++)
        {
            for (int index = 0; index < _ingredients[category].Length; index++)
            {
                if (_ingredients[category][index] == ingredient)
                {
                    currentCategory = category;
                    currentIndex = index;
                    UpdateSelection();
                    return;
                }
            }
        }
    }

    private void SelectCurrentIngredient()
    {
        Ingredient currentIngredient = _ingredients[currentCategory][currentIndex];
        GameObject currentSelectedObject = currentIngredient.gameObject;

        // Enable and color the "Selected" child red
        Transform selectedTransform = currentSelectedObject.transform.Find("Selected");
        if (selectedTransform != null)
        {
            selectedTransform.gameObject.SetActive(true);
            selectedTransform.GetComponent<SpriteRenderer>().color = Color.red;
        }

        // Clear selection if an item in the special category is selected, or vice versa
        if (currentCategory == 3)
        {
            ClearSelectionInCategory(0); // Clear effect ingredients if special is selected
        }
        else if (currentCategory == 0)
        {
            ClearSelectionInCategory(3); // Clear special ingredients if effect is selected
        }

        // Add the ingredient to the craftingIngredients array
        craftingIngredients[currentCategory] = currentIngredient;

        Debug.Log($"Added {currentIngredient.name} to crafting ingredients");
    }

    private void ClearSelectionInCategory(int category)
    {
        foreach (var ingredient in _ingredients[category])
        {
            Transform selectedTransform = ingredient.gameObject.transform.Find("Selected");
            if (selectedTransform != null)
            {
                selectedTransform.gameObject.SetActive(false);
            }
        }
        craftingIngredients[category] = null;
    }
}
