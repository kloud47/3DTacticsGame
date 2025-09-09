using System;
using Game.Units.Actions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Units.AnimControls
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] private Animator unitAnimator;
        [SerializeField] private Transform bulletProjectilePrefab;
        [SerializeField] private Transform shootPointReference;
        [SerializeField] private Transform rifleTransform;
        [SerializeField] private Transform swordTransform;

        private void Awake()
        {
            if (TryGetComponent<MoveAction>(out MoveAction moveAction))
            {
                moveAction.OnStartMoving += MoveAction_OnStartMoving;
                moveAction.OnStopMoving += MoveAction_OnStopMoving;
            }

            if (TryGetComponent<ShootAction>(out ShootAction shootAction))
            {
                shootAction.OnShoot += ShootAction_OnShoot;
            }
            
            if (TryGetComponent<SwordAction>(out SwordAction swordAction))
            {
                swordAction.OnSwordActionStarted += swordAction_OnSwordActionStarted;
                swordAction.OnSwordActionCompleted += swordAction_OnSwordActionCompleted;
            }
        }

        private void Start()
        {
            EquipRifle();
        }

        private void MoveAction_OnStartMoving(object sender, EventArgs e)
        {
            unitAnimator.SetBool("IsWalking", true);
        }
        
        private void MoveAction_OnStopMoving(object sender, EventArgs e)
        {
            unitAnimator.SetBool("IsWalking", false);
        }

        private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
        {
            unitAnimator.SetTrigger("Shoot");

            Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointReference.position, Quaternion.identity);
            BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

            Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
            targetUnitShootAtPosition.y = shootPointReference.position.y;
            bulletProjectile.Setup(targetUnitShootAtPosition);
        }

        private void swordAction_OnSwordActionStarted(object sender, EventArgs e)
        {
            EquipSword();
            unitAnimator.SetTrigger("SwordSlash");
        }

        private void swordAction_OnSwordActionCompleted(object sender, EventArgs e)
        {
            EquipRifle();   
        }

        private void EquipSword()
        {
            swordTransform.gameObject.SetActive(true);
            rifleTransform.gameObject.SetActive(false);
        }

        private void EquipRifle()
        {
            swordTransform.gameObject.SetActive(false);
            rifleTransform.gameObject.SetActive(true);
        }
    }
}
