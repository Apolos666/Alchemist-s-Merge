using System.Collections.Generic;
using UnityEngine;

public class PropManager : GenericSingleton<PropManager>
{
    private readonly HashSet<GameObject> activePropsData = new HashSet<GameObject>();

    public void RegisterProp(GameObject prop) => activePropsData.Add(prop);

    public void UnregisterProp(GameObject prop) => activePropsData.Remove(prop);

    public IEnumerable<GameObject> GetAllPropsData() => activePropsData;

    public void ClearAllProps() => activePropsData.Clear();
    
    public void AddPropFromSaveData(GameObject propDatas)
    {
        activePropsData.Add(propDatas);
    }

    private static string RemoveCloneFromName(string name)
    {
        const string cloneSuffix = "(Clone)";
        return name.EndsWith(cloneSuffix) ? name.Substring(0, name.Length - cloneSuffix.Length) : name;
    }
}