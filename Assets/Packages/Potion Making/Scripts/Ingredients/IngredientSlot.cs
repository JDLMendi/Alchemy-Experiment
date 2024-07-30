using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour
{
    public Ingredient ingredient; // Scriptable Object

    private Image imageComponent;

    void Awake()
    {
        // Ensure there's an Image component attached to the same GameObject
        imageComponent = GetComponent<Image>();
    }

    void Start()
    {
        // Set the image component if ingredient is already assigned in the inspector
        if (ingredient != null)
        {
            SetIngredient(ingredient);
        }
    }

    public void SetIngredient(Ingredient newIngredient)
    {
        ingredient = newIngredient;
        UpdateImage();
    }

    private void UpdateImage()
    {
        if (ingredient != null && ingredient.itemSprite != null)
        {
            if (imageComponent == null)
            {
                imageComponent = gameObject.AddComponent<Image>();
            }
            imageComponent.sprite = ingredient.itemSprite;
            imageComponent.color = ingredient.itemColour;
        }
        else if (imageComponent != null)
        {
            Destroy(imageComponent);
            imageComponent = null;
        }
    }

    public void ApplyFallSprite()
    {
        if (ingredient != null && ingredient.fallSprite != null)
        {
            imageComponent.sprite = ingredient.fallSprite;
        }
        else
        {
            Debug.LogWarning("Fall sprite or ingredient is missing for " + gameObject.name);
        }
    }

}
