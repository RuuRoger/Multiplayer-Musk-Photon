using UnityEngine;

namespace Assets.Scripts.Test
{
    [RequireComponent(typeof(CharacterController))]
    public class TestCustomPlayerCOntroller : MonoBehaviour
    {
        // ================================================== MEMBERS ==================================================
        [SerializeField]
        private float m_speed = 5f;

        [SerializeField]
        private float m_rotationSpeed = 1f;

        private InputSystem_Actions m_inputSystemAction;
        private CharacterController m_characterController;
        private Vector2 m_inputMove = new Vector2(0f, 0f);

        // ================================================== UNITY LIFECYCLE METHODS ==================================================
        private void Awake()
        {
            m_inputSystemAction = new InputSystem_Actions();
            m_characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            m_inputSystemAction.Player.Move.Enable();
        }

        private void OnDisable()
        {
            m_inputSystemAction.Player.Move.Disable();
        }

        private void Update()
        {
            ReadInput();
            Move();           
        }

        // ================================================== MOVEMENT ==================================================
        private void ReadInput()
        {
            m_inputMove = m_inputSystemAction.Player.Move.ReadValue<Vector2>();
        }

        private void Move()
        {
            Vector3 cameraForward = UnityEngine.Camera.main.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = UnityEngine.Camera.main.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 direction = (cameraForward * m_inputMove.y) + (cameraRight * m_inputMove.x);
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    m_rotationSpeed * Time.deltaTime
                );
            }

            m_characterController.Move(m_speed * Time.deltaTime * direction);
        }
    }
}