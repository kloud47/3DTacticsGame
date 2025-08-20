using System;
using Game.Grids;
using Game.Units.Actions;
using UnityEngine;

public class Unit : MonoBehaviour
{
   private GridPosition gridPosition;
   private MoveAction moveAction;
   private SpinAction spinAction;
   private BaseAction[] baseActionsArray;
   private int actionPoints = 2;

   private void Awake()
   {
      moveAction = GetComponent<MoveAction>();
      spinAction = GetComponent<SpinAction>();
      baseActionsArray = GetComponents<BaseAction>();
   }

   private void Start()
   {
      gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
      LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
   }

   private void Update()
   {
      
      
      GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
      if (newGridPosition != gridPosition)
      {
         // Unit GridPosition changed:
         LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
         gridPosition = newGridPosition;
      }
   }

   public MoveAction GetMoveAction()
   {
      return moveAction;
   }

   public SpinAction GetSpinAction()
   {
      return spinAction;
   }

   public GridPosition GetGridPosition()
   {
      return gridPosition;
   }

   public BaseAction[] GetBaseActionsArray()
   {
      return baseActionsArray;
   }

   public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
   {
      if (CanSpendActionPointsToTakeAction(baseAction))
      {
         SpendActionPoints(baseAction.GetActionPointsCost());
         return true;
      }
      else
      {
         return false;
      }
   }
   
   public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
   {
      return actionPoints >= baseAction.GetActionPointsCost();
   }

   private void SpendActionPoints(int amount)
   {
      actionPoints -= amount;
   }

   public int GetActionPoints()
   {
      return actionPoints;
   }
}
