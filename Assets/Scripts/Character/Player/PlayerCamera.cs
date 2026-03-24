using UnityEngine;

namespace FR
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance; //singleton 
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;
        // Change to tweak cam performance
        [Header("CAMERA SETTINGS")]
        private float cameraSmoothSpeed = 1;        // The bigger this number, the longer the camera will take to reach its desired position during movement
        [SerializeField] private float leftAndRightRotationSpeed = 180;  // tweak for testing feeling, try keeping left and right faster
        [SerializeField] private float upAndDownRotationSpeed = 160;
        [SerializeField] float minPivot = -50;  // So we don't fully rotate the camera vertically
        [SerializeField] float maxPivot = 60;
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("CAMERA VALUES")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition;   // used for camera collisions (moves the cam object to this pos when colliding)
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;        // Values used for camera collisions
        private float targetCameraZPosition;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        } 

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisions();           
            }

        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            // if locked on, force rotate towards target
            // else rotate regularly

            // Normal rotations

            // Rotate left and right based on horizontal movement of camera input
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            
            // Rotate up and down based on vertical movement of camera input
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;  //we need to clamp this so it never exceeds min max pivot
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minPivot, maxPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;
            // Rotate this gameobject left and right
            cameraRotation = Vector3.zero;
            cameraRotation.y = leftAndRightLookAngle;   // in terms of rotation y is left and right
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // Rotate the pivot gameobject up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
    
        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            // (Desired) Direction for collision check
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // Check if there is an object in front of our desired camera direction
            if (Physics.SphereCast(
                cameraPivotTransform.position,
                cameraCollisionRadius,
                direction,
                out hit,
                Mathf.Abs(targetCameraZPosition),
                collideWithLayers       // Layer (currently default layer of the game is 0)
            ))
            {
                // If there is, we get our distance from it
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // Then equate target Z position ot the following
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            // If target pos is less than collision radius, subtract collision radius (snapping it back as if "bouncing")
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            // Apply our final pos using a lerp over a time of 0.2f
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);   //0.2f is for time
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    
    }    
}
