using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerDeathOnHit : MonoBehaviour
{
    [SerializeField] private string obstacleTag = "obstacle";
    public GameObject gameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(obstacleTag))
        {
            if (GameManager.Instance != null) GameManager.Instance.GameOver();
            // Destroy player, play VFX/SFX, etc.
            Destroy(gameObject);
        }
    }
}
