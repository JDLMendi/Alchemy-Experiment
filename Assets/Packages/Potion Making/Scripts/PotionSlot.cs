using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour
{
    public Potion potion;
    public Image imageComponent;

    public void SetPotion(Potion newPotion)
    {
        if (imageComponent == null)
        {
            imageComponent = gameObject.AddComponent<Image>();
        }

        imageComponent.sprite = newPotion.potionSprite;
        imageComponent.color = newPotion.color;
    }
}
