using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private LayerMask clickableLayer;
    [SerializeField] private Material highlightMaterial;

    private Transform _selection;

    private UiController _uiController;

    void Awake()
    {
        _uiController = GameObject.Find("Selected_Info").GetComponent<UiController>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Select();
        }
    }

    void Select()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
        {
            var selection = hit.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = highlightMaterial;
            }
            _selection = selection;
            _uiController.ToggleInfoMenu(true);
            _uiController.UpdateSelectedInfo(selection.gameObject);
        }
    }
}
