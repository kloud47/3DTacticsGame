using UnityEngine;

public class HealthSysem : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            health = 0;
        }
    }
}
