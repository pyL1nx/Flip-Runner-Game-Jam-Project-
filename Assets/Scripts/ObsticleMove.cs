using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private void Update()
    {
        float speed = (GameManager.Instance != null) ? GameManager.Instance.CurrentObstacleSpeed : 2f;
        transform.position += Vector3.back * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy this obstacle when it overlaps a trigger tagged "destroy"
        if (other.CompareTag("destroyobsticle"))
        {
            Destroy(gameObject);
        }
    }
}
