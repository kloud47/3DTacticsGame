using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    private void Start()
    {
        // when changing Units available action buttons should also change
        UnitControlSystem.Instance.OnSelectedUnitChanged += UnitControlSystem_OnSelectedUnitChanged;
        UnitControlSystem.Instance.OnSelectedActionChanged += UnitControlSystem_OnSelectedActionChanged;
        CreateUnitActionButton();
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButton()
    {
        foreach (Transform actionButton in actionButtonContainerTransform)
        {
            Destroy(actionButton.gameObject);
        }
        
        actionButtonUIList.Clear();
        
        Unit selectedUnit = UnitControlSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionsArray())
        {
            Transform actionButtonUITransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonUITransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            
            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UnitControlSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButton();
        UpdateSelectedVisual();
    }

    private void UnitControlSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    
    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
}
