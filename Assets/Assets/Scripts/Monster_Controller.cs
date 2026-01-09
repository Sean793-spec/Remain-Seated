using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Monster_Controller : MonoBehaviour
{
    [Header("Navmesh Target")]
    [Tooltip("This is the Scriptable Object that contains the target positions for the monster to move to.")]
    public Vector3Data target;
    // this is the path that the monster will follow when moive can be seen with the debug line the monster is selected in the inspector
    private NavMeshPath path;
    [Tooltip("This is the NavMeshAgent component that allows the monster to move using the NavMesh system.")]
    public NavMeshAgent agent;
   
    [Header("Roaming Settings")]
    [Tooltip("The range of Detection is the scale of the Detection range sphere set on the monster")]
    public Transform detectionRange;

    [Header("Animation")]
    [Tooltip("Assin the Animator component to control the monster animations.")]
    public Animator animator;
    [Tooltip("This is the animation state for roaming movement.")]
    public string roamingAnimationParameter = "isWalking";
    [Tooltip("This is the animation state for chasing movement.")]
    public string chasingAnimationParameter = "isChasing";
    
    [Tooltip("This is the event that is invoked when the monster is roaming.")]
    public UnityEvent onRoaming;
    [Tooltip("This is the event that is invoked when the monster is chasing.")]
    public UnityEvent onChasing;
    [Tooltip("This is the event that is invoked when the monster is tracking an item.")]
    public UnityEvent onTrackingItem;
    
    [Header("Monster State")]
    [Tooltip("This is the current state of the monster. can be changed for debug but is change by self")]
    public MonsterState currentState;
    
    public enum MonsterState
    {
        Roaming,
        Chasing,
        TrackingItem
    }

    public void Awake()
    {
        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
        
        RoamingPositionSet();
    }

    public void Update()
    {
        switch (currentState)
        {
            case MonsterState.Chasing:
                FollowPlayer();
                break;
            
            case MonsterState.Roaming:
                RoamingStart();
                break;
            
            case MonsterState.TrackingItem:
                TrackItem();
                break;
        }
    }

    private void RoamingPositionSet()
    { 
        
        Vector3 randomDirection = Random.insideUnitSphere * (detectionRange.localScale.x * 0.5f);
        randomDirection.y = 0f;
        Vector3 roamingPosition = detectionRange.position + randomDirection;
        
        target.value[0] = roamingPosition;
        
        agent.SetDestination(target.value[0]);
        
    }

    private void TrackItem()
    {
     
        onTrackingItem.Invoke();
        animator.Play(chasingAnimationParameter);
        
        NavMesh.CalculatePath(transform.position, target.value[0], NavMesh.AllAreas, path);
        agent.SetDestination(target.value[0]);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = MonsterState.Roaming;
        }
            
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.blue);
        
    }
    
    private void FollowPlayer()
    {
        onChasing.Invoke();
        animator.Play(chasingAnimationParameter);
        
        NavMesh.CalculatePath(transform.position, target.value[0], NavMesh.AllAreas, path);
        agent.SetDestination(target.value[0]);
            
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green);
        
    }
    
    private void RoamingStart()
    {
        onRoaming.Invoke();
        animator.Play(roamingAnimationParameter);
        
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (MonsterState.Roaming == currentState)
            {
                RoamingPositionSet();
            }
        }
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }
}
