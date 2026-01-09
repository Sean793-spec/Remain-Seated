using UnityEngine;

public class DetectionRrangeTrigger : MonoBehaviour
{
    [Tooltip("This if for the Script Attached to the monster")]
    public Monster_Controller monsterController;
    [Tooltip("This is the same Scriptable object that the monster is using")]
    public Vector3Data monsterMovePoints;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterController.currentState = Monster_Controller.MonsterState.Chasing;
            monsterMovePoints.value[1] = monsterMovePoints.value[0];
            monsterMovePoints.value[0] = other.transform.position;
            Debug.Log("Player detected — chasing!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterController.agent.ResetPath();
            monsterMovePoints.value[0] = monsterMovePoints.value[1];
            monsterController.currentState = Monster_Controller.MonsterState.Roaming;
            Debug.Log("Player left range — roaming again.");
        }
    }
    
}

      