using UnityEngine;

public class WallMover : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 direction;

    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }
}
