using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public CharacterController controller;
    public float speed;
    public float gravity;
    Vector3 moveDirection;
    float scaleFactor;

    void Update()
    {
        scaleFactor = transform.localScale.x;
        float adjustedSpeed = speed * scaleFactor;

        Vector2 direction = joystick.direction;

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(direction.x, 0, direction.y);

            Quaternion targetRotation = moveDirection != Vector3.zero ? Quaternion.LookRotation(moveDirection).normalized : transform.rotation;
            transform.rotation = targetRotation;

            moveDirection = moveDirection * adjustedSpeed;
        }

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
    }
}