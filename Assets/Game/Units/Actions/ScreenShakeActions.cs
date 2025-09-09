using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyShootShoot += ShootAction_OnAnyShootShoot;
        GrenadeProjectile.OnAnyGrenadeExplode += GrenadeProjectile_OnAnyGrenadeExplode;
    }

    private void ShootAction_OnAnyShootShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        // Debug.Log("reached Shaky reached");
        ScreenShake.Instance.Shake();   
    }

    private void GrenadeProjectile_OnAnyGrenadeExplode(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(4f);
    }
}
