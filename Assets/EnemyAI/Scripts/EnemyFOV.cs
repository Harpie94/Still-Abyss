using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRadius = 10f; // The radius of the enemy's field of view
    public float viewAngle = 120f; // The angle of the enemy's field of view
    public GameObject Player;
    public bool isChasingPlayer = false;
    public bool isPlayerInSight = false;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayerInFOV();
    }

    void DetectPlayerInFOV()
    {
        if (Player == null) return;

        Vector3 dirToPlayer = (Player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

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
                    PlayerDetected();
                    return;
                }
            }
        }
        isPlayerInSight = false;
    }

    void PlayerDetected()
    {
        isPlayerInSight = true;
    }
}
