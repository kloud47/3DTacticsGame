using System;
using Game.Units.Actions;
using UnityEngine;

namespace Game.Units.AnimControls
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] private Animator unitAnimator;
        [SerializeField] private Transform bulletProjectilePrefab;
        [SerializeField] private Transform shootPointReference;

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
    }
}
