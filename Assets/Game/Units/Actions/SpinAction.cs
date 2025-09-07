using System;
using System.Collections.Generic;
using Game.Grids;
using UnityEngine;

namespace Game.Units.Actions
{
    public class SpinAction : BaseAction
    {
        // public delegate void SpinCompleteDelegate();
        private float totalSpinAmt;
    
        private void Update()
        {
            if (!isActive) return;
        
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
            totalSpinAmt += spinAddAmount;
            if (totalSpinAmt >= 360f)
            {
                ActionComplete();
            }
        }

        public override void TakeAction(GridPosition gridPosition,Action onComplete)
        {
            totalSpinAmt = 0f;
            ActionStart(onComplete);
        }
    
        public override string GetActionName() => "Spin";

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            GridPosition unitGridPostion = unit.GetGridPosition();
            
            return new List<GridPosition> { unitGridPostion };
        }
        
        public override int GetActionPointsCost()
        {
            return 1;
        }

        public override EnemyAIActions GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIActions
            {
                gridPosition = gridPosition,
                actionValue = 0,
            };
        }
    }
}
