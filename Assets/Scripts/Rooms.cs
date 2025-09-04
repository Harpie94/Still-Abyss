using System;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    [SerializeField] private string roomName;
    [SerializeField] private int thresholdForCriticalDamage = 30; // pourcentage de points de vie pour le seuil de dommage critique

    public static event Action<Rooms> OnCriticalDamageReached; 
    
    // Propriétés de points de vie
    private int maxHealthPoints;
    private int currentHealthPoints;
    private bool isCriticallyDamaged = false;

    public string RoomName => string.IsNullOrEmpty(roomName) ? gameObject.name : roomName;

    // Propriétés pour les points de vie
    public int MaxHealthPoints => maxHealthPoints;
    public int CurrentHealthPoints => currentHealthPoints;
    public float HealthPercentage => maxHealthPoints > 0 ? (float)currentHealthPoints / maxHealthPoints : 0f;

    // Initialisation des points de vie
    public void InitializeHealth(int maxHealth)
    {
        maxHealthPoints = maxHealth;
        currentHealthPoints = maxHealth;
        isCriticallyDamaged = false;
    }

    // Mise à jour des points de vie
    public void UpdateCurrentHealth(int health)
    {
        currentHealthPoints = Mathf.Clamp(health, 0, maxHealthPoints);
        CheckCriticalDamage();
    }
    
    private void CheckCriticalDamage()
    {
        if (!isCriticallyDamaged && HealthPercentage * 100 <= thresholdForCriticalDamage)
        {
            isCriticallyDamaged = true;
            OnCriticalDamageReached?.Invoke(this);
            Debug.Log($"Room {RoomName} has reached critical damage!");
        }
    }
}
        