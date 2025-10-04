using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private void Update()
    {
        // Move in the opposite Z direction
        transform.position += new Vector3(0f, 0f, -speed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("destroy"))
        {
            Destroy(gameObject);
        }
    }
}
