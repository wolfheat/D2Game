using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIController : MonoBehaviour
{
    [SerializeField] Slider healthBarSlider;
    [SerializeField] Slider energyBarSlider;

    private void Update()
    {
        SetHealthBar(CharacterStats.Health/ (float)CharacterStats.HealthMax);
        SetEnergyBar(CharacterStats.Energy/ (float)CharacterStats.EnergyMax);   
    }

    public void SetHealthBar(float percent)
    {
        healthBarSlider.value = percent;
    }
    
	public void SetEnergyBar(float percent)
    {
        energyBarSlider.value = percent;
    }

}
