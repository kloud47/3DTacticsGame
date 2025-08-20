using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionPointsPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private Transform actionPointsContainerTransform;

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
        UnitControlSystem.Instance.OnActionStarted += UnitControlSystem_OnActionStarted;
        UpdateActionPoints();
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
            
            actionButtonUIList.Add(actionButtonUI); // to change the UI:
        }
    }

    private void UnitControlSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitControlSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UnitControlSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
    
    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitControlSystem.Instance.GetSelectedUnit();
        int actionpoints = selectedUnit.GetActionPoints();

        Debug.Log("Action Points -> " + actionpoints);
        for (int i = 0; i < actionpoints; i++)
        {
            Instantiate(actionPointsPrefab, actionPointsContainerTransform);
        }
    }
}
