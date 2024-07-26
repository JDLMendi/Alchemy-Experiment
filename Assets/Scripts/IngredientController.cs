using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientController : MonoBehaviour
{
    // Variable to store the current selected object
    private GameObject currentSelectedObject;

    void Update()
    {
        // Get the current selected GameObject from the EventSystem
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        // Check if the selected object has changed
        if (selectedObject != currentSelectedObject)
        {
            currentSelectedObject = selectedObject;

            // If you want to log or perform some action when the selected object changes
            if (currentSelectedObject != null)
            {
                Debug.Log("Selected Object: " + currentSelectedObject.name);
            }
        }
    }

    // Method to get the current selected object
    public GameObject GetCurrentSelectedObject()
    {
        return currentSelectedObject;
    }
}
