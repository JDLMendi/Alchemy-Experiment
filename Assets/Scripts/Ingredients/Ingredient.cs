using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BaseIngredient : ScriptableObject
{
    public string baseName;
    public Color baseColour;
    public Sprite baseSprite;
}

[CreateAssetMenu]
public class EffectIngredient : ScriptableObject
{
    public string effectName;
    public Color effectColour;
    public Sprite effectSprite;
}

[CreateAssetMenu]
public class BottleIngredient : ScriptableObject
{
    public string bottleName;
    public Sprite bottleSprite;
}
