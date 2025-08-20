using System;
using System.Collections.Generic;
using Game.Grids;
using UnityEngine;

namespace Game.Units.Actions
{
    public class MoveAction : BaseAction
    {
        private Vector3 targetPosition;
        
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] private int maxMoveDistance = 4; // this is the amount of distance a Unit can move:
        [SerializeField] private Animator unitAnimator;
        
        protected override void Awake()
        {
            base.Awake();
            targetPosition = transform.position;
        }

        void Update()
        {
            if (!isActive) return;
            
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float stoppingDistance = 0.1f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                transform.position += moveDirection * (Time.deltaTime * moveSpeed);
                unitAnimator.SetBool("IsWalking", true);
            }
            else
            {
                onActionComplete?.Invoke(); // Invoke the function to clear the Busy action:
                unitAnimator.SetBool("IsWalking", false);
                this.isActive = false;
            }
            float rotationSpeed = 15f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }
    
        public override void TakeAction(GridPosition gridPosition, Action onComplete)
        {
            onActionComplete = onComplete;
            isActive = true;
            targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
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
                    validGridPositions.Add(testGridPosition);
                }
            }
        
            return validGridPositions;
        }

        public override string GetActionName() => "Move";
    }
}
