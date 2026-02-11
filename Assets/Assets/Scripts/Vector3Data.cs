using UnityEngine;

[CreateAssetMenu(
    fileName = "New Vector3 Data",
    menuName = "Data/Vector3 Data"
)]
public class Vector3Data : ScriptableObject
{
    public Vector3[] value;
}
