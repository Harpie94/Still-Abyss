using System;
using UnityEngine;
using System.Collections;

public class RepairableObject : MonoBehaviour
{
    [Header("Configuration des dégâts")]
    [SerializeField] private int maxHealthPoints = 100;
    [SerializeField] private int currentHealthPoints;
    
    [Header("Configuration de la réparation")]
    [SerializeField] private int repairAmountPerSecond = 30;
    [SerializeField] private float repairRange = 3f;
    
    [Header("Meshes par niveau de dégât")]
    [SerializeField] private Mesh[] damageLevelMeshes;
    
    [Header("Salle")]
    [SerializeField] private Rooms assignedRoom;
    public Rooms AssignedRoom => assignedRoom;
    
    [Header("Composants")]
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    
    [Header("Effets visuels")]
    [SerializeField] private ParticleSystem damageEffect;
    [SerializeField] private ParticleSystem repairEffect;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip repairSound;
    
    public bool IsFullyRepaired => currentHealthPoints >= maxHealthPoints;
    public bool IsDestroyed => currentHealthPoints <= 0;
    public float HealthPercentage => (float)currentHealthPoints / maxHealthPoints;
    public bool CanBeRepaired => !IsFullyRepaired && !IsDestroyed;
    
    private bool isBeingRepaired = false;
    private Coroutine repairCoroutine;

    private void Awake()
    {
        currentHealthPoints = maxHealthPoints;
        UpdateVisualState();
    }

    private void Start()
    {
        //UpdateVisualState();
    }
    
    public void TakeDamage(int damage)
    {
        if (IsDestroyed) return;
        
        currentHealthPoints = Mathf.Max(0, currentHealthPoints - damage);
        UpdateVisualState();
        PlayDamageEffects();
        
        if (IsDestroyed)
        {
            StopRepair();
            OnObjectDestroyed();
        }
    }
    
    public bool Repair(int repairAmount)
    {
        if (IsFullyRepaired) return false;
        
        currentHealthPoints = Mathf.Min(maxHealthPoints, currentHealthPoints + repairAmount);
        UpdateVisualState();
        PlayRepairEffects();
        
        return true;
    }
    
    public bool IsInRepairRange(Vector3 playerPosition)
    {
        return Vector3.Distance(transform.position, playerPosition) <= repairRange;
    }
    
    public void StartRepair()
    {
        if (!CanBeRepaired || isBeingRepaired) return;
        
        isBeingRepaired = true;
        repairCoroutine = StartCoroutine(RepairRoutine());
        
        if (repairEffect != null)
            repairEffect.Play();
    }
    
    public void StopRepair()
    {
        if (!isBeingRepaired) return;
        
        isBeingRepaired = false;
        
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
            repairCoroutine = null;
        }
        
        if (repairEffect != null)
            repairEffect.Stop();
    }
    
    private IEnumerator RepairRoutine()
    {
        while (isBeingRepaired && CanBeRepaired)
        {
            yield return new WaitForSeconds(1f);
            
            if (isBeingRepaired && CanBeRepaired)
            {
                Repair(repairAmountPerSecond);
                
                if (IsFullyRepaired)
                {
                    StopRepair();
                    Debug.Log($"{gameObject.name} a été entièrement réparé !");
                }
            }
        }
    }
    
    // Getter pour la santé actuelle
    public int GetCurrentHealth()
    {
        return currentHealthPoints;
    }
    
    // Getter pour la santé maximale
    public int GetMaxHealth()
    { 
        return maxHealthPoints;
    }
    
    private void UpdateVisualState()
    {
        if (damageLevelMeshes.Length == 0 || meshFilter == null) return;
        
        int damageLevel = CalculateDamageLevel();
        meshFilter.mesh = damageLevelMeshes[damageLevel];
    }
    
    private int CalculateDamageLevel()
    {
        float damagePercentage = 1f - HealthPercentage;
        int maxLevel = damageLevelMeshes.Length - 1;
        return Mathf.RoundToInt(damagePercentage * maxLevel);
    }
    
    private void PlayDamageEffects()
    {
        if (damageEffect != null)
            damageEffect.Play();
            
        if (audioSource != null && damageSound != null)
            audioSource.PlayOneShot(damageSound);
    }
    
    private void PlayRepairEffects()
    {
        if (audioSource != null && repairSound != null)
            audioSource.PlayOneShot(repairSound);
    }
    
    private void OnObjectDestroyed()
    {
        Debug.Log($"{gameObject.name} a été détruit !");
    }
}