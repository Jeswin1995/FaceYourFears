using UnityEngine;

public class PositionResetter : MonoBehaviour
{
    // The Y position threshold to trigger the reset
    [SerializeField] private float yThreshold = -10f;

    // The reset Y position
    [SerializeField] private float resetYPosition = -1f;

    void Update()
    {
        // Check if the object's Y position is below the threshold
        if (transform.position.y < yThreshold)
        {
            // Reset only the Y position, keeping X and Z unchanged
            transform.position = new Vector3(transform.position.x, resetYPosition, transform.position.z);
        }
    }
}