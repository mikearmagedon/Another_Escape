using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class CharacterMovement : MonoBehaviour
    {
        ThirdPersonCharacter character;
        NavMeshAgent agent;

        private void Start()
        {
            character = GetComponent<ThirdPersonCharacter>();
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;
        }

        private void Update()
        {
            //if (target != null)
            //    agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false);
            else
                character.Move(Vector3.zero, false);
        }
    }
}
