using UnityEngine;
using Unity.Netcode;

namespace FR{
    public class CharacterManager : NetworkBehaviour
    {

        public CharacterController characterController;

        CharacterNetworkManager characterNetworkManager;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);   

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            // If the character is controlled from one's side, assign its network position to one's transform
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            // If it is controlled else where, assign position locally depending on its network transform
            else //donottouchunlessitisstrictlynecessaryplease,yeahIamtalkingtoyoumyfutureself
            {
                // Position Section //
                transform.position = Vector3.SmoothDamp(
                    transform.position, 
                    characterNetworkManager.
                    networkPosition.Value, 
                    ref characterNetworkManager.networkPositionVelocity, 
                    characterNetworkManager.networkPositionSmoothTime
                );
                
                // Rotation Section //
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    characterNetworkManager.networkRotation.Value, 
                    characterNetworkManager.networkRotationSmoothTime
                );

            }
        }
    }
}
