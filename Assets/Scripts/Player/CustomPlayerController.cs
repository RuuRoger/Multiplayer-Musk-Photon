using UnityEngine;
using Assets.Scripts.Data.Player;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]

    public class CustomPlayerController : MonoBehaviour
    {
        /* ================================================================================================================
        ---------------------------------------------------- MEMBERS -----------------------------------------------------
        ================================================================================================================= */
        [SerializeField] private CustomPlayerControllerSettings _settings;
        private InputSystem_Actions m_inputSystemAction;
        private CharacterController m_characterController;
        private Vector2 m_moveInput = new Vector2(0f, 0f);

        /* ================================================================================================================
        ---------------------------------------------------- UNITY LIFECYCLE METHODS -----------------------------------------------------
        ================================================================================================================= */
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

        /* ================================================================================================================
        ---------------------------------------------------- HORIZONTAL MOVEMENT -----------------------------------------------------
        ================================================================================================================= */
        private void ReadInput()
        {
            m_moveInput = m_inputSystemAction.Player.Move.ReadValue<Vector2>();
        }

        private void Move()
        {
            Vector3 camForward = UnityEngine.Camera.main.transform.forward;
            Vector3 camRight = UnityEngine.Camera.main.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 direction = (camForward * m_moveInput.y) + (camRight * m_moveInput.x);
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _settings.RotationSpeed * Time.deltaTime);
            }
            m_characterController.Move(direction * _settings.MovementSpeed * Time.deltaTime);
        }
    }
}