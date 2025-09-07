using System;
using System.Collections.Generic;
using Game.Grids;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;
    
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        // Setting owning unit in all actions:
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositionList();
        return validGridPositions.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete?.Invoke();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public EnemyAIActions GetBestEnemyAIAction()
    {
        List<EnemyAIActions> enemyAIActionList = new List<EnemyAIActions>();
        List<GridPosition> validActionGridPositions = GetValidActionGridPositionList();
    
        foreach (GridPosition gridPosition in validActionGridPositions)
        {
            EnemyAIActions enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if (enemyAIActionList.Count == 0) return null;
        enemyAIActionList.Sort((EnemyAIActions a, EnemyAIActions b) => b.actionValue - a.actionValue);
        return enemyAIActionList[0];
    }

    public abstract EnemyAIActions GetEnemyAIAction(GridPosition gridPosition);
}
