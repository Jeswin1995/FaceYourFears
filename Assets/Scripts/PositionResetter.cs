using UnityEngine;

public class PositionResetter : MonoBehaviour
{
    // The Y position threshold to trigger the reset
    [SerializeField] private float yThreshold = -10f;

    // The reset position
    [SerializeField] private Vector3 resetPosition = new Vector3(0, -1, 0);

    void Update()
    {
        // Check if the object's Y position is below the threshold
        if (transform.position.y < yThreshold)
        {
            // Reset the position
            transform.position = resetPosition;
        }
    }
}