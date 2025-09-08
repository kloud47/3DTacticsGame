using System;
using System.Collections.Generic;
using Game.Grids;
using UnityEngine;

namespace Game.Units.Actions
{
    public class MoveAction : BaseAction
    {
        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;
        
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] private int maxMoveDistance = 4; // this is the amount of distance a Unit can move:
        // [SerializeField] private Animator unitAnimator;

        private List<Vector3> positionList;
        private int currentPositionIndex;

        void Update()
        {
            if (!isActive) return;
            
            Vector3 targetPosition = positionList[currentPositionIndex];
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            
            float rotationSpeed = 15f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
            
            float stoppingDistance = 0.1f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                transform.position += moveDirection * (Time.deltaTime * moveSpeed);
            }
            else
            {
                currentPositionIndex++;
                if (currentPositionIndex >= positionList.Count)
                {
                    OnStopMoving?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
                }
            }
        }
    
        public override void TakeAction(GridPosition gridPosition, Action onComplete)
        {
            List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);
            
            currentPositionIndex = 0;
            positionList = new List<Vector3>(pathGridPositionList.Count);

            foreach (GridPosition pathGridPosition in pathGridPositionList)
            {
                positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));   
            }
            
            OnStartMoving?.Invoke(this, EventArgs.Empty);// alerts the animation controller:
            
            ActionStart(onComplete);
        }
    
        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositions = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPosition();
        
            for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
            {
                for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    if (unitGridPosition == testGridPosition)
                    {
                        // Same Grid position where unit is already at:
                        continue;
                    }

                    if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        // Grid position already occupied with another unit:
                        continue;
                    }

                    if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                    {
                          continue;
                    }
                    
                    // int pathfindingDistanceMultiplier = 10;
                    // if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
                    // {
                    //     // Path length is too long
                    //     continue;
                    // }

                    
                    validGridPositions.Add(testGridPosition);
                }
            }
        
            return validGridPositions;
        }

        public override string GetActionName() => "Move";
        
        public override EnemyAIActions GetEnemyAIAction(GridPosition gridPosition)
        {
            int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

            return new EnemyAIActions
            {
                gridPosition = gridPosition,
                actionValue = targetCountAtGridPosition * 10,
            };

        }
    }
}
