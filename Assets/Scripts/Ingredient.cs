using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Base,
    Effect,
    Special,
    Bottle
}
public class Ingredient : MonoBehaviour
{
    public IngredientType ingredientType;
    public string ingredientName;

    // public IngredientSO ingredientSO; // Make a scriptable object for each object for later!

}
