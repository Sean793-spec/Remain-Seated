using UnityEngine;
public class SoundItem : MonoBehaviour
{
    [Tooltip("This if for the Script Attached to the monster")]
    public Monster_Controller monsterController;
    [Tooltip("This is the same Scriptable object that the monster is using")]
    public Vector3Data monsterMovePoints;

    public float spawnscale = 1f;

    public void Awake()
    {
        ChangeScale(spawnscale);
    }

    public void MakeSound()
    {
        monsterMovePoints.value[1] = monsterMovePoints.value[0];
        monsterMovePoints.value[0] = transform.position;
        monsterController.currentState = Monster_Controller.MonsterState.TrackingItem;
        Debug.Log("Sound made — tracking item!");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            MakeSound();
            monsterController.currentState = Monster_Controller.MonsterState.TrackingItem;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            monsterController.currentState = Monster_Controller.MonsterState.Roaming;
        }
    }

    public void ChangeScale(float scale)
    {

        transform.localScale = new Vector3(1,1,1) * scale;

    }
    
}
