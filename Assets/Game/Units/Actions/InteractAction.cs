using System;
using System.Collections.Generic;
using Game.Door;
using Game.Grids;
using Unity.VisualScripting;
using UnityEngine;

public class InteractAction : BaseAction
{
    private int maxInteractDistance = 1;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        ActionComplete();
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractible interactible = LevelGrid.Instance.GetInteractibleAtGirdPosition(gridPosition);
        interactible.Interact(OnInteractionComplete);
        ActionStart(onActionComplete);
    }

    private void OnInteractionComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                IInteractible interactible = LevelGrid.Instance.GetInteractibleAtGirdPosition(testGridPosition);
                if (interactible == null)
                {
                    // No door at grid position:
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override EnemyAIActions GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIActions
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
}
