using UnityEngine;

public class OutdoorEnemyClearer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private OutdoorEnemySpawner spawner;

    [Header("Reglages")]
    [Min(0f)] public float clearRange = 2.5f;

    [Header("Interaction")]
    public bool requireKeyPress = true;
    public KeyCode interactKey = KeyCode.E;

    private void Update()
    {
        if (player == null || spawner == null) return;
        if (spawner.CurrentEnemy == null) return;

        float dist = Vector3.Distance(player.position, spawner.CurrentEnemy.transform.position);

        if (!requireKeyPress)
        {
            if (dist <= clearRange)
                spawner.ClearCurrentEnemy();
        }
        else
        {
            if (dist <= clearRange && Input.GetKeyDown(interactKey))
                spawner.ClearCurrentEnemy();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;
        Gizmos.color = new Color(0f, 1f, 1f, 0.35f);
        Gizmos.DrawWireSphere(player.position, clearRange);
    }
}
