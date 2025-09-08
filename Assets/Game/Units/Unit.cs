using System;
using Game.Grids;
using Game.Units;
using Game.Units.Actions;
using UnityEngine;

public class Unit : MonoBehaviour
{
   private const int ACTION_POINTS_MAX = 20;
   
   public static event EventHandler OnAnyActionPointsChanged;
   public static event EventHandler OnAnyUnitSpawned;
   public static event EventHandler OnAnyUnitDead;
   
   [SerializeField] private bool isEnemy;   

   private GridPosition gridPosition;
   private HealthSysem healthSystem;
   private BaseAction[] baseActionsArray;
   private int actionPoints = ACTION_POINTS_MAX;
   
   private void Awake()
   {
      healthSystem = GetComponent<HealthSysem>();
      baseActionsArray = GetComponents<BaseAction>();
   }

   private void Start()
   {
      gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
      LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
      TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
      healthSystem.OnDead += HealthSystem_OnDead;
      
      
      
      OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty); // Add the spawned Unit to the list:
   }

   private void Update()
   {
      GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
      if (newGridPosition != gridPosition)
      {
         // Unit GridPosition changed:
         GridPosition oldGridPostion = gridPosition;
         gridPosition = newGridPosition;
         LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPostion, newGridPosition);
      }
   }
   
   public T GetAction<T>() where T : BaseAction
   {
      foreach (BaseAction baseAction in baseActionsArray)
      {
         if (baseAction is T)
         {
            return (T)baseAction;
         }
      }
      return null;
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

   public void Damage(int damageAmount)
   {
      healthSystem.TakeDamage(damageAmount);
   }

   private void HealthSystem_OnDead(object sender, EventArgs e)
   {
      LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
      Destroy(gameObject);
      
      OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
   }

   public float GetHealthNormalized()
   {
      return healthSystem.GetHealthNormalized();
   }
}
