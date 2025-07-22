using UnityEngine;

public class DebugGameState : MonoBehaviour
{
    public GameManager.GameState stateToApply = GameManager.GameState.Paused;

    private void OnTriggerEnter(Collider other)
    {
        GameManager gm = GameManager.Instance;
        if (gm != null && gm.IsPlayer(other.gameObject))
        {
            gm.SetGameState(stateToApply);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
