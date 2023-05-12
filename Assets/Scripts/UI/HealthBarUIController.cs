using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIController : MonoBehaviour
{
    [SerializeField] Slider healthBarSlider;
    [SerializeField] Slider energyBarSlider;
    [SerializeField] Slider XPBarSlider;

    private void Update()
    {
        SetHealthBar(SavingUtility.Instance.playerInventory.Health/ (float)SavingUtility.Instance.playerInventory.MaxHealth);
        SetEnergyBar(SavingUtility.Instance.playerInventory.Energy / (float)SavingUtility.Instance.playerInventory.MaxEnergy);
        //Debug.Log("Setting XP "+ SavingUtility.Instance.playerInventory.XP+" of "+ SavingUtility.Instance.playerInventory.MaxXP);
        SetXPBar(SavingUtility.Instance.playerInventory.XP / (float)SavingUtility.Instance.playerInventory.MaxXP);
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
