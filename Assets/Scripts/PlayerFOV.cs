using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    public float viewRadius = 10f; // The radius of the enemy's field of view
    public float viewAngle = 120f; // The angle of the enemy's field of view
    public GameObject Enemy;
    public bool isEnemyInSight = false;

    public LayerMask targetMask; // Layer mask for targets (e.g., player)
    public LayerMask obstacleMask; // Layer mask for obstacles (e.g., walls)


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    { 
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Enemy == null)
        {
            Debug.LogError("Enemy GameObject is not assigned in PlayerFOV script.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayerInFOV();
    }

    void DetectPlayerInFOV()
    {
        if (Enemy == null) return;

        Vector3 dirToPlayer = (Enemy.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, Enemy.transform.position);

        // Vérifie si le joueur est dans le rayon de vision
        if (distanceToPlayer <= viewRadius)
        {
            // Vérifie si le joueur est dans l'angle de vision
            float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleToPlayer < viewAngle / 2f)
            {
                // Vérifie s'il n'y a pas d'obstacle entre l'ennemi et le joueur
                if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
                {
                    EnemyDetected();
                    return;
                }
            
                
            }
        }

        isEnemyInSight = false;
    }

    public void EnemyDetected()
    {
        isEnemyInSight = true;
    }
}


