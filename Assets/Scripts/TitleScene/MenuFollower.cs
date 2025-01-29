using UnityEngine;

public class MenuFollower : MonoBehaviour
{
    public Transform playerHead; // Reference to the player's head (Main Camera)
    public Vector3 offset = new Vector3(0, 0.5f, 1f); // Offset from the player's head
    public float positionThreshold = 0.1f; // Minimum distance to trigger menu movement

    void Update()
    {
        if (playerHead != null)
        {
            // Calculate the target position based on the player's head position and offset
            Vector3 targetPosition = playerHead.position + playerHead.forward * offset.z +
                                     playerHead.right * offset.x +
                                     Vector3.up * offset.y;

            // Check the distance between the menu's current position and the target position
            if (Vector3.Distance(transform.position, targetPosition) > positionThreshold)
            {
                // Smoothly move the menu to the target position
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);

                // Make the menu face the player
                transform.rotation = Quaternion.LookRotation(transform.position - playerHead.position);
            }
        }
    }
}