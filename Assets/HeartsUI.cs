using UnityEngine;
using TMPro;

public class HeartsUI : MonoBehaviour
{
    public ShipHealth shipHealth;   // drag the Spaceship here
    public TMP_Text heartsText;     // leave empty to auto-find on this object

    void Start()
    {
        if (heartsText == null) heartsText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (shipHealth == null || heartsText == null) return;

        string display = "";
        for (int i = 0; i < shipHealth.GetCurrentHearts(); i++)
        {
            display += "♥ ";
        }
        heartsText.text = display;
    }
}
