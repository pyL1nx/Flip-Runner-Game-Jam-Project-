using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject roadSectionPrefab;

    // Adjust this to where the new section should overlap the previous one.
    // The video first tries -57, then adjusts to -35 on Z after accounting for movement.
    // Example below uses -35 as demonstrated.
    [SerializeField] private Vector3 spawnPosition = new Vector3(0f, 0f, -35f);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            // Spawn with no rotation (Quaternion.identity) as in the video
            Instantiate(roadSectionPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
