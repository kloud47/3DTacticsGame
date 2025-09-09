using System;
using System.Collections.Generic;
using Game.Door;
using Game.Grids;
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
        Door door = LevelGrid.Instance.GetDoorAtGirdPosition(gridPosition);
        door.Interact(OnInteractionComplete);
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
                
                Door door = LevelGrid.Instance.GetDoorAtGirdPosition(testGridPosition);
                if (door == null)
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
