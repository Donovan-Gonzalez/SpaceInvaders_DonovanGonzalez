using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    public int maxHearts = 3;
    public AlienFleet fleet;   // auto-found if left empty

    int hearts;

    void Start()
    {
        hearts = maxHearts;
        if (fleet == null) fleet = FindFirstObjectByType<AlienFleet>();
    }

    public int GetCurrentHearts()
    {
        return hearts;
    }

    public void TakeDamage()
    {
        if (hearts <= 0) return;

        hearts--;
        Debug.Log("Ship hit! Hearts left: " + hearts);

        if (hearts <= 0 && fleet != null)
        {
            fleet.GameOver("Your ship was destroyed!");
        }
    }
}