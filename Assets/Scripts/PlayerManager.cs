using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{

    private Ingredient baseIngredient;
    private Ingredient effectIngredient;
    private Ingredient bottle;
    public EventSystem eventSystem;

    // Update is called once per frame
    // void Update()
    // {
    //     if (eventSystem.currentSelectedGameObject != null)
    //     {
    //         // Get the currently selected GameObject
    //         GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

    //         // Output the name of the selected GameObject
    //         Debug.Log("Selected Object: " + selectedObject.name);
    //     }
    //     else
    //     {
    //         Debug.Log("No object selected.");
    //     }
    // }
}
