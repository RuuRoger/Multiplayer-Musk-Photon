using UnityEngine;

namespace Assets.Scripts.Data.Player
{
    [CreateAssetMenu(fileName = "PlayerControllerSettings", menuName = "Player/Controller Settings")]
    public class CustomPlayerControllerSettings : ScriptableObject
    {
        /* ================================================================================================================
        ---------------------------------------------------- MEMBERS -----------------------------------------------------
        ================================================================================================================= */
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 1f;

        /* ================================================================================================================
        ---------------------------------------------------- PROPERTIES -----------------------------------------------------
        ================================================================================================================= */
        public float MovementSpeed
        {
            get
            {
                return _movementSpeed;
            }
        }

        public float RotationSpeed
        {
            get
            {
                return _rotationSpeed;
            }
        }
    }    
}
