using System;
using UnityEngine;

public class TownRegenerator : MonoBehaviour
{

    private float regenerationTimer = 0f;
    private float regenerationTime = 1f;
    private int healthRegen = 5;
    private int energyRegen = 5;    

	private void Update()
    {
        regenerationTimer -= Time.deltaTime;
        if(regenerationTimer <= 0)
        {
            Regenerate();
            regenerationTimer = regenerationTime;
        }
    }

    private void Regenerate()
    {
        if (SavingUtility.Instance.playerInventory.Health != SavingUtility.Instance.playerInventory.MaxHealth)
            SavingUtility.Instance.playerInventory.Health = Mathf.Clamp(SavingUtility.Instance.playerInventory.Health + healthRegen, 0, SavingUtility.Instance.playerInventory.MaxHealth);
        if (SavingUtility.Instance.playerInventory.Energy != SavingUtility.Instance.playerInventory.MaxEnergy)
            SavingUtility.Instance.playerInventory.Energy = Mathf.Clamp(SavingUtility.Instance.playerInventory.Energy+energyRegen,0, SavingUtility.Instance.playerInventory.MaxEnergy);
    }
}
