using System;
using System.Collections.Generic;
using Game.Grids;
using UnityEngine;

public class GranadeAction : BaseAction
{
    [SerializeField] private Transform grenadeProjectilePrefab;
    
    private int maxThrowDistance = 7;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Granade";
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                // for maintaining a circular radius:
                float testDistance = Mathf.Pow(x, 2) + Mathf.Pow(z, 2);
                testDistance = Mathf.Sqrt(testDistance);
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }

                validGridPositions.Add(testGridPosition);
            }
        }
        return validGridPositions;
    }

    public override EnemyAIActions GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIActions
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
    
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
        
        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
}
