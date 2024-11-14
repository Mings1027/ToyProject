using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public InputActionReference movement, jump;

        public CharacterController characterController;
        public Transform cam;
        public Transform playerFeet;
        //Move
        public float moveSpeed;
        public float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;
        private Vector3 moveVec;
        //Jump
        private const float Gravity = -9.8f;
        public float jumpForce;
        private float ySpeed;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }
        private void Update()
        {
            Move();
            Jump();
        }
        private void LateUpdate()
        {
            characterController.Move(moveVec * (moveSpeed * Time.deltaTime));
        }
        private void Move()
        {
            moveVec = movement.action.ReadValue<Vector3>().normalized;
            if (moveVec.magnitude > 0f)
            {
                var targetAngle = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                moveVec = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            }
            playerFeet.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        private void Jump()
        {
            ySpeed += Gravity * Time.deltaTime;
            if (characterController.isGrounded)
            {
                ySpeed = -0.5f;
                if (jump.action.triggered)
                {
                    ySpeed = jumpForce;
                }
            }
            moveVec.y = ySpeed;
        }

    }
}