using UnityEngine;

[CreateAssetMenu(fileName = "Prop", menuName = "Data/Prop")]
public class Prop : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public Sprite Icon;
    public int Point;
}
