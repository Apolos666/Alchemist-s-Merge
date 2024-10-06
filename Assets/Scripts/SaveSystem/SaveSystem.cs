using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveSystem : GenericPersistentSingleton<SaveSystem>
{
    private string _savePath;
    private const string PrefabPath = "Props/";

    protected override void Awake()
    {
        base.Awake();
        _savePath = Path.Combine(Application.persistentDataPath, "gamesave.json");
    }

    public void SaveGame()
    {
        var saveData = new SaveData();

        foreach (var prop in PropManager.Instance.GetAllPropsData())
        {
            saveData.PropDatas.Add(new PropData
            {
                PropName = RemoveCloneFromName(prop.name),
                Position = prop.transform.position,
                Rotation = prop.transform.rotation
            });
        }
    
        var json = JsonUtility.ToJson(saveData);
        File.WriteAllText(_savePath, json);
    }
    
    private static string RemoveCloneFromName(string name)
    {
        const string cloneSuffix = "(Clone)";
        if (name.EndsWith(cloneSuffix))
        {
            return name.Substring(0, name.Length - cloneSuffix.Length);
        }
        return name;
    }

    public void LoadGame()
    {
        if (!File.Exists(_savePath)) return;
        
        var json = File.ReadAllText(_savePath);
        var saveData = JsonUtility.FromJson<SaveData>(json);
            
        PropManager.Instance.ClearAllProps();

        foreach (var data in saveData.PropDatas)
        {
            var prefabPath = Path.Combine(PrefabPath, data.PropName);
            var propPrefab = Resources.Load<GameObject>(prefabPath);
            
            if (propPrefab != null)
            {
                var newProp = Instantiate(propPrefab, data.Position, data.Rotation);
                newProp.layer = LayerMask.NameToLayer("Item");
                PropManager.Instance.AddPropFromSaveData(newProp);
            }
            else
            {
                Debug.LogWarning("Prefab not found: " + prefabPath);
            }
        }
    }

    public bool HasSaveData()
    {
        if (!File.Exists(_savePath)) return false;

        var json = File.ReadAllText(_savePath);
        var saveData = JsonUtility.FromJson<SaveData>(json);

        return saveData.PropDatas.Count > 0;
    }
    
    public void ClearSaveData()
    {
        if (File.Exists(_savePath))
        {
            File.Delete(_savePath);
        }
    }
}