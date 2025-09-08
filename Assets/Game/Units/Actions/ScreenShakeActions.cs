using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyShootShoot += ShootAction_OnAnyShootShoot;
    }

    private void ShootAction_OnAnyShootShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        Debug.Log("reached Shaky reached");
        ScreenShake.Instance.Shake();   
    }
}
