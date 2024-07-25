using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectedObject : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject _selected;

    private void Start()
    {
        _selected = transform.Find("Selected")?.gameObject;
    }

    public void EnableOutline()
    {
        if (_selected != null)
        {
            _selected.SetActive(true);
        }
    }

    public void DisableOutline()
    {
        if (_selected != null)
        {
            _selected.SetActive(false);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DisableOutline();
    }

    public void OnSelect(BaseEventData eventData)
    {
        EnableOutline();
    }
}
