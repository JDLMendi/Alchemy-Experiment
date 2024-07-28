using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionManager : MonoBehaviour
{
    public ToggleGroup[] toggleGroups; // Assign these in the Inspector

    private Toggle[][] toggles;
    private Toggle[] selectedToggles;

    void Start()
    {
        // Initialize the toggles arrays
        toggles = new Toggle[toggleGroups.Length][];
        selectedToggles = new Toggle[toggleGroups.Length];

        // Populate the toggles arrays
        for (int i = 0; i < toggleGroups.Length; i++)
        {
            toggles[i] = toggleGroups[i].GetComponentsInChildren<Toggle>();
            int index = i;
            foreach (var toggle in toggles[i])
            {
                toggle.onValueChanged.AddListener((isSelected) => OnToggleValueChanged(toggle, index, isSelected));
            }
        }
    }

    private void OnToggleValueChanged(Toggle changedToggle, int groupIndex, bool isSelected)
    {
        if (isSelected)
        {
            selectedToggles[groupIndex] = changedToggle;
        }
    }

    public Toggle[] GetSelectedToggles()
    {
        return selectedToggles;
    }
}
