using UnityEngine;
using Assets.Scripts.Data.Player;
using Photon.Pun;

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
        private PhotonView _photonView;
        private Vector2 m_moveInput = new Vector2(0f, 0f);
        private Camera _playerCamera;

        /* ================================================================================================================
        ---------------------------------------------------- UNITY LIFECYCLE METHODS -----------------------------------------------------
        ================================================================================================================= */
        private void Awake()
        {
            m_inputSystemAction = new InputSystem_Actions();
            m_characterController = GetComponent<CharacterController>();
            _photonView = GetComponent<PhotonView>();
            _playerCamera = GetComponentInChildren<Camera>(true);
            
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
            if (!_photonView.IsMine) return;
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
            var cam = _playerCamera != null ? _playerCamera : UnityEngine.Camera.main;
            if (cam == null) return;
            Vector3 camForward = cam.transform.forward;
            Vector3 camRight = cam.transform.right;
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