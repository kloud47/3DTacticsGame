using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Grids;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    public static event EventHandler<OnShootEventArgs> OnAnyShootShoot;
    public event EventHandler<OnShootEventArgs> OnShoot;
    
    [SerializeField] private LayerMask obstaclesLayerMask;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    private State state;
    private int maxShootDistance = 6;
    private float stateTimer;
    private Unit TargetUnit;
    private bool CanShootBullet;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;
        
        switch (state)
        {
            case State.Aiming:
                float rotateSpeed = 10f;
                Vector3 aimDir = (TargetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, rotateSpeed * Time.deltaTime);
                break;
            case State.Shooting:
                if (CanShootBullet)
                {
                    Shoot();
                    CanShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }
    
    private void NextState()
    {
        // Setting data for NextState:
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
        // Debug.Log(state);
    }

    private void Shoot()
    {
        Debug.Log("Fire");
        OnAnyShootShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = TargetUnit,
            shootingUnit = unit
        });// for camera shake and other shared properties:
        
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = TargetUnit,
            shootingUnit = unit
        }); // Invoking shoot animation:
        TargetUnit.Damage(40);
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override void TakeAction(GridPosition gridPosition, Action onComplete)
    {
        TargetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        // Debug.Log("Aiming");
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        CanShootBullet = true;
        
        ActionStart(onComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
    
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
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
                if (testDistance > maxShootDistance)
                {
                    continue;
                }

                // if (unitGridPosition == testGridPosition)
                // {
                //     // Same Grid position where unit is already at:
                //     continue;
                // }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position is Empty, on Enemy unit present:
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both are in same team:
                    continue;
                }
                
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        shootDir,
                        Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                        obstaclesLayerMask))
                {
                    // Blocked by an Obstacle
                    continue;
                }
                
                validGridPositions.Add(testGridPosition);
            }
        }
    
        return validGridPositions;
    }

    public override int GetActionPointsCost()   
    {
        return 2;
    }

    public Unit GetTargetUnit()
    {
        return TargetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }
    
    public override EnemyAIActions GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        return new EnemyAIActions
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized())* 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
