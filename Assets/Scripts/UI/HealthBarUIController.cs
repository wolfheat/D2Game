using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIController : MonoBehaviour
{
    [SerializeField] Slider healthBarSlider;
    [SerializeField] Slider energyBarSlider;
    [SerializeField] Slider XPBarSlider;

    private void Update()
    {
        SetHealthBar(CharacterStats.Health/ (float)CharacterStats.HealthMax);
        SetEnergyBar(CharacterStats.Energy/ (float)CharacterStats.EnergyMax);   
        SetXPBar(CharacterStats.XP/ (float)CharacterStats.XPMax);   
    }

    public void SetXPBar(float percent)
    {
        XPBarSlider.value = percent;
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
