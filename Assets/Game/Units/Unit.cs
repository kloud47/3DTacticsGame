using System;
using Game.Grids;
using Game.Units.Actions;
using UnityEngine;

public class Unit : MonoBehaviour
{
   private const int ACTION_POINTS_MAX = 2;
   
   public static event EventHandler OnAnyActionPointsChanged; 
   
   [SerializeField] private bool isEnemy;   

   private GridPosition gridPosition;
   private MoveAction moveAction;
   private SpinAction spinAction;
   private BaseAction[] baseActionsArray;
   private int actionPoints = ACTION_POINTS_MAX;
   
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
      TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
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

   public Vector3 GetWorldPosition()
   {
      return transform.position;
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
      
      OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
   }

   public int GetActionPoints()
   {
      return actionPoints;  
   }

   private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
   {
      if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
          (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
      {
         actionPoints = ACTION_POINTS_MAX;
         
         OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);  
      }
   }

   public bool IsEnemy()
   {
      return isEnemy;
   }

   public void Damage()
   {
      Debug.Log(transform + " damage!");
   }
}
