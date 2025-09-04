using System;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    [SerializeField] private int roomsID;
    [SerializeField] private string roomName;
    public static event Action<Rooms> OnRoomAsTakenDamage; 
    
    // Propriétés de points de vie
    private int maxHealthPoints;
    private int currentHealthPoints;
    
    private int currentDamageTier = 0; // 0 = intact, 1 = minor damage, 2 = moderate damage, 3 = severe damage , 4 = destroyed
    private int currentHealthPercentage = 100;

    public string RoomName => string.IsNullOrEmpty(roomName) ? gameObject.name : roomName; // Si roomName est vide, utiliser le nom de l'objet
    public int RoomsID => roomsID;

    // Propriétés pour les points de vie
    public int MaxHealthPoints => maxHealthPoints;
    public int CurrentHealthPoints => currentHealthPoints;
    public float HealthPercentage => maxHealthPoints > 0 ? (float)currentHealthPoints / maxHealthPoints : 0f;

    // Initialisation des points de vie
    public void InitializeHealth(int maxHealth)
    {
        maxHealthPoints = maxHealth;
        currentHealthPoints = maxHealth;
    }

    // Mise à jour des points de vie
    public void UpdateCurrentHealth(int health)
    {
        currentHealthPoints = Mathf.Clamp(health, 0, maxHealthPoints);
        RoomAsTakenDamage();
    }
    
    public int GetCurrentDamageTier()
    {
        return currentDamageTier;
    }
    
    private void RoomAsTakenDamage()
    {
        // if (!isCriticallyDamaged && HealthPercentage * 100 <= thresholdForCriticalDamage)
        // {
        //     isCriticallyDamaged = true;
        //     OnRoomAsTakenDamage?.Invoke(this);
        //     Debug.Log($"Room {RoomName} has reached critical damage!");
        // }
        
        currentHealthPercentage = Mathf.RoundToInt(HealthPercentage * 100);
        // for health % 100-75 = tier 0
        // for health % 74-50 = tier 1
        // for health % 49-25 = tier 2
        // for health % 24-1 = tier 3
        // for health % 0 = tier 4
        int newDamageTier = currentHealthPercentage switch 
        {
            >= 75 => 0,
            >= 50 => 1,
            >= 25 => 2,
            >= 1 => 3,
            _ => 4
        };
        if (newDamageTier != currentDamageTier)
        {
            currentDamageTier = newDamageTier;
            OnRoomAsTakenDamage?.Invoke(this);
            Debug.Log($"Room {RoomName} has reached damage tier {currentDamageTier} ({currentHealthPercentage}%)");
        }
        
    }
}
        