using UnityEngine;
using UnityEngine.UI;

public class CanvasCycler : MonoBehaviour
{
    [SerializeField] private Canvas[] _categories;
    private int _index = 0;

    private void Start()
    {
        UpdateCanvas();
    }

    public void OnUp()
    {
        // Move to the previous canvas in the array
        Debug.Log("Moving Up");
        _index--;
        if (_index < 0)
        {
            _index = _categories.Length - 1;
        }
        UpdateCanvas();
    }

    public void OnDown()
    {
        Debug.Log("Moving Down");
        // Move to the next canvas in the array
        _index++;
        if (_index >= _categories.Length)
        {
            _index = 0;
        }
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        // Enable the current canvas's GraphicRaycaster and disable the others
        for (int i = 0; i < _categories.Length; i++)
        {
            GraphicRaycaster raycaster = _categories[i].GetComponent<GraphicRaycaster>();
            if (raycaster != null)
            {
                raycaster.enabled = (i == _index);
            }
        }
    }
}
