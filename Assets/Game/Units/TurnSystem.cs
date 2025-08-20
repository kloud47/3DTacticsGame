using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber = 0;

    public void NextTurn()
    {
        turnNumber++;
    }
}
