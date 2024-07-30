using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    Base,
    Special,
    Effect,
    Bottle
}

[CreateAssetMenu]
public class Ingredient : ScriptableObject
{
    public string itemName;
    public int itemID;
    public itemType itemtype;
    public Color itemColour;
    public Sprite itemSprite;
    public Sprite fallSprite;

    public void ApplySprite(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = itemSprite;
        }
    }

    public void ApplyFallSprite(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = fallSprite;
        }
    }
}
