using System;
using UnityEngine;

public class TownRegenerator : MonoBehaviour
{

    private float regenerationTimer = 0f;
    private float regenerationTime = 1f;
    private int healthRegen = 8;
    private int energyRegen = 10;    

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
        if (CharacterStats.Health != CharacterStats.HealthMax)
            CharacterStats.Health = Mathf.Clamp(CharacterStats.Health+healthRegen,0, CharacterStats.HealthMax);
        if (CharacterStats.Energy != CharacterStats.EnergyMax)
            CharacterStats.Energy = Mathf.Clamp(CharacterStats.Energy+energyRegen,0, CharacterStats.EnergyMax);
    }
}
