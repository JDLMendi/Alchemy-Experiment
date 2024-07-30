using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Potion : ScriptableObject
{
    public string potionName;
    public int potionID;

    public Color color;

    public Sprite potionSprite;
    public Ingredient[] ingredients;
}


