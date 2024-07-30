using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneManager : MonoBehaviour
{
    public IngredientManager ingredientManager;
    public Ingredient[] baseIngredients;
    public Ingredient[] effectIngredients;
    public Ingredient[] bottleIngredients;
    public Potion[] potions;
    public GameObject endButton;

    private int _potionIndex = 0;

    private void Start()
    {
        LoadIngredients();
        potions = new Potion[bottleIngredients.Length];
    }

    private void Update()
    {
        endButton.SetActive(_potionIndex > 0);
    }

    public void MixIngredients()
    {
        Potion resultPotion = ingredientManager.MixIngredients();
        if (resultPotion != null)
        {
            potions[_potionIndex] = resultPotion;
            _potionIndex++;
        }
    }

    public void FindIngredients()
    {
        // This should try and look at an inventory, find which item is an ingredient and adds them to the corresponding arrays
    }

    // This should load all the found ingredients into the slots
    public void LoadIngredients()
    {
        
        ingredientManager.LoadIngredients(baseIngredients, 0);
        ingredientManager.LoadIngredients(effectIngredients, 1);
        ingredientManager.LoadIngredients(bottleIngredients, 2);
    }

    public void EndButton()
    {
        Debug.Log("Ending Night");
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            // Pass the potions array to the next scene? Or add to inventory?
        #endif
    }
}
