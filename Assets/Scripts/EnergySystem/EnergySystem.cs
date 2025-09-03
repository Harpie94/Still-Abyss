using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    [Header("Energy Settings")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float energyDrainRate = 5f;
    [SerializeField] private float extraEnergyDrain = 0f;
    [SerializeField] private float dayRechargeRate = 10f;
    [SerializeField] private float nightRechargeRate = 0f;
    [SerializeField] private float lowEnergyThreshold = 30f;
    [SerializeField] private float criticalEnergyThreshold = 10f;
    [SerializeField] private bool useMaxEnergyAtStart = true;
    [SerializeField] private bool infiniteEnergy = false;
    [SerializeField] private bool isRecharging = false;
    [SerializeField] private float startingEnergy = 0f;
    public float CurrentEnergy { get; private set; }

    private void Start()
    {
        if (useMaxEnergyAtStart)
        {
            CurrentEnergy = maxEnergy;
        }
        else
        {
            CurrentEnergy = Mathf.Clamp(startingEnergy, 0f, maxEnergy);
        }
    }

    private void Update()
    {
        if (infiniteEnergy)
        {
            CurrentEnergy = maxEnergy;
            return;
        }

        RechargeEnergy();
        DrainEnergy();
    }
    //permet de rajouter du drain pour la conso energy
    public void AddExtraDrain(float amount)
    {
        extraEnergyDrain += amount;
    }

    public void RemoveExtraDrain(float amount)
    {
        extraEnergyDrain -= amount;
        extraEnergyDrain = Mathf.Max(0f, extraEnergyDrain);
    }

    private void DrainEnergy()
    {
        float totalDrain = energyDrainRate + extraEnergyDrain;
        CurrentEnergy -= totalDrain * Time.deltaTime;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0f, maxEnergy);
    }

    //les fonctions qu'on va utiliser pour la recharge le jour
    private void RechargeEnergy()
    {
        if (CurrentEnergy < maxEnergy)
        {
            if (DayNightScript.isDay)
            {
                CurrentEnergy += dayRechargeRate * Time.deltaTime;
            }
            else
            {
                CurrentEnergy += nightRechargeRate * Time.deltaTime;
            }
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0f, maxEnergy);
        }
    }
    public void SetRecharging(bool value)
    {
        isRecharging = value;
    }

    public bool HasEnoughEnergy(float amount)
    {
        return CurrentEnergy >= amount;
    }

    public void UseEnergy(float amount)
    {
        if (infiniteEnergy) return;

        CurrentEnergy = Mathf.Max(0f, CurrentEnergy - amount);
    }

    public void SetEnergy(float value)
    {
        CurrentEnergy = Mathf.Clamp(value, 0f, maxEnergy);
    }

    public void AddEnergy(float amount)
    {
        if (infiniteEnergy) return;

        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, maxEnergy);
    }

    public float GetEnergyPercentage()
    {
        return CurrentEnergy / maxEnergy;
    }

    public float GetMaxEnergy()
    {
        return maxEnergy;
    }

    public bool IsLowEnergy()
    {
        return CurrentEnergy <= lowEnergyThreshold;
    }

    public bool IsCriticalEnergy()
    {
        return CurrentEnergy <= criticalEnergyThreshold;
    }

    //les fonctions pour upgrade ce qui est lié à l'énergie
    void UpgradeMaxEnergy(float upgrade)
    {
        maxEnergy = upgrade;
    }

    void UpgradeDayRecharge(float upgrade)
    {
        dayRechargeRate = upgrade;
    }

    void UpgradeNightRecharge(float upgrade)
    {
        nightRechargeRate = upgrade;
    }
}
