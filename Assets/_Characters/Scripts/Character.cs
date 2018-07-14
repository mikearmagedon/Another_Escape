using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    public class Character : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;
        [SerializeField] [Range(0.1f, 1f)] float animatorForwardCap = 1f;

        [Header("Audio")]
        [SerializeField][Range(0, 1f)] float audioSourceSpatialBlend = 0.5f;
        [SerializeField][Range(0, 1f)] float audioSourceVolume = 1f;

        [Header("Capsule Collider")]
        [SerializeField] PhysicMaterial physicMaterial;
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 0.9f, 0);
        [SerializeField] float colliderRadius = 0.3f;
        [SerializeField] float colliderHeight = 1.8f;

        [Header("Movement")]
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float jumpPower = 12f;
        [Range(1f, 4f)] [SerializeField] float gravityMultiplier = 2f;
        [SerializeField] float runCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField] float moveSpeedMultiplier = 1f;
        [SerializeField] float animSpeedMultiplier = 1f;
        [SerializeField] float groundCheckDistance = 0.1f;
        [SerializeField] AudioClip[] footstepSoundsGrass;
        [SerializeField] AudioClip[] footstepSoundsStone;


        [Header("Nav Mesh Agent")]
        [Tooltip("If false, the parameters below are ignored")]
        [SerializeField] bool createNavMeshAgent = true;
        [SerializeField] float navMeshAgentSteeringSpeed = 3.5f;
        [SerializeField] float navMeshAgentStoppingDistance = 1.3f;

        AudioManager audioManager;
        string type;
        Rigidbody rigidBody;
        Animator animator;
        AudioSource audioSource;
        NavMeshAgent navMeshAgent;
        bool isGrounded;
        float origGroundCheckDistance;
        const float half = 0.5f;
        float turnAmount;
        float forwardAmount;
        Vector3 groundNormal;
        float moveThreshold = 1f;
        bool isAlive = true;


        void Awake()
        {
            AddRequiredComponents();
        }

        void AddRequiredComponents()
        {
            // Animator
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;
            animator.applyRootMotion = true;

            // AudioSource
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;
            audioSource.volume = audioSourceVolume;

            // Capsule Collider
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.material = physicMaterial;
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;

            // Rigidbody
            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            // Nav Mesh Agent
            if (createNavMeshAgent)
            {
                navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
                navMeshAgent.speed = navMeshAgentSteeringSpeed;
                navMeshAgent.stoppingDistance = navMeshAgentStoppingDistance;
                navMeshAgent.updateRotation = false;
                navMeshAgent.updatePosition = true;
                navMeshAgent.autoBraking = false;
            }
        }

        void Start()
        {
            audioManager = AudioManager.instance;
            origGroundCheckDistance = groundCheckDistance;
        }

        void Update()
        {
            // For player movement which doesn't have navMeshAgent
            if (!createNavMeshAgent) { return; }

            if ((navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) && isAlive)
            {
                Move(navMeshAgent.desiredVelocity, false);
            }
            else
            {
                navMeshAgent.velocity = Vector3.zero;
                Move(Vector3.zero, false);
            }
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        public void Kill()
        {
            isAlive = false;
        }

        public void SetDestination(Vector3 worldPosition)
        {
            navMeshAgent.destination = (worldPosition); // TODO consider using SetDestination() instead
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        public float GetAnimSpeedMultiplier()
        {
            return animator.speed;
        }

        public void Move(Vector3 movement, bool jump)
        {
            Vector3 localMovement = SetForwardAndTurn(movement);

            ApplyExtraTurnRotation();

            // control and velocity handling is different when grounded and airborne:
            if (isGrounded)
            {
                HandleGroundedMovement(jump);
            }
            else
            {
                HandleAirborneMovement();
            }

            // send input and other state parameters to the animator
            UpdateAnimator(localMovement);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Stone"))
            {
                type = "stone";
            }
            else
            {
                type = "grass";
            }
        }

        // Left footstep SFX animation callback
        void FootL()
        {
            if (type == "stone")
            {
                AudioClip clip = footstepSoundsStone[Random.Range(0, footstepSoundsStone.Length)];
                audioManager.PlayMisc(clip);
            }
            else if (type == "grass")
            {
                AudioClip clip = footstepSoundsGrass[Random.Range(0, footstepSoundsGrass.Length)];
                audioManager.PlayMisc(clip);
            }
        }

        // Right footstep SFX animation callback
        void FootR()
        {
            if (type == "stone")
            {
                AudioClip clip = footstepSoundsStone[Random.Range(0, footstepSoundsStone.Length)];
                audioManager.PlayMisc(clip);
            }
            else if (type == "grass")
            {
                AudioClip clip = footstepSoundsGrass[Random.Range(0, footstepSoundsGrass.Length)];
                audioManager.PlayMisc(clip);
            }
        }

        Vector3 SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localMovement = transform.InverseTransformDirection(movement);
            CheckGroundStatus();
            localMovement = Vector3.ProjectOnPlane(localMovement, groundNormal);
            turnAmount = Mathf.Atan2(localMovement.x, localMovement.z);
            forwardAmount = localMovement.z;
            return localMovement;
        }

        void UpdateAnimator(Vector3 movement)
        {
            // update the animator parameters
            animator.SetFloat("Forward", forwardAmount * animatorForwardCap, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            if (!isGrounded)
            {
                animator.SetFloat("Jump", rigidBody.velocity.y);
            }

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle =
                Mathf.Repeat(
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
            float jumpLeg = (runCycle < half ? 1 : -1) * forwardAmount;
            if (isGrounded)
            {
                animator.SetFloat("JumpLeg", jumpLeg);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (isGrounded && movement.magnitude > 0)
            {
                animator.speed = animSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                animator.speed = 1;
            }
        }


        void HandleAirborneMovement()
        {
            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
            rigidBody.AddForce(extraGravityForce);

            groundCheckDistance = rigidBody.velocity.y < 0 ? origGroundCheckDistance : 0.01f;
        }


        void HandleGroundedMovement(bool jump)
        {
            // check whether conditions are right to allow a jump:
            if (jump && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // jump!
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpPower, rigidBody.velocity.z);
                isGrounded = false;
                animator.applyRootMotion = false;
                groundCheckDistance = 0.1f;
            }
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }


        void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (isGrounded && Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = rigidBody.velocity.y;
                rigidBody.velocity = v;
            }
        }


        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
            {
                groundNormal = hitInfo.normal;
                isGrounded = true;
                animator.applyRootMotion = true;
            }
            else
            {
                isGrounded = false;
                groundNormal = Vector3.up;
                animator.applyRootMotion = false;
            }
        }
    }
}
