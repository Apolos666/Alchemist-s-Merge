using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundEffectManager))]
public class SoundEffectManagerEditor : Editor
{
    private SerializedProperty _soundEffects;
    private SerializedProperty _poolSize;

    private void OnEnable()
    {
        _soundEffects = serializedObject.FindProperty("_soundEffects");
        _poolSize = serializedObject.FindProperty("_poolSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var poolSizeLabel = new GUIContent("Pool Size", "The number of audio sources to create.");
        EditorGUILayout.PropertyField(_poolSize, poolSizeLabel);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sound Effects", EditorStyles.boldLabel);

        for (var i = 0; i < _soundEffects.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            var soundEffect = _soundEffects.GetArrayElementAtIndex(i);
            var name = soundEffect.FindPropertyRelative("name");
            var clips = soundEffect.FindPropertyRelative("clips");
            var volume = soundEffect.FindPropertyRelative("volume");
            var pitchVariance = soundEffect.FindPropertyRelative("pitchVariance");
            var basePitch = soundEffect.FindPropertyRelative("basePitch");
            
            EditorGUILayout.PropertyField(name);
            EditorGUILayout.PropertyField(clips, true);
            EditorGUILayout.Slider(volume, 0f ,1f, "Volume");
            EditorGUILayout.Slider(pitchVariance, 0f, 2f, "Pitch Variance");
            EditorGUILayout.Slider(basePitch, 0f, 1f, "Base Pitch");
            
            if (GUILayout.Button("Remove Sound Effect"))
            {
                _soundEffects.DeleteArrayElementAtIndex(i);
                break;
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
        
        if (GUILayout.Button("Add Sound Effect"))
        {
            _soundEffects.InsertArrayElementAtIndex(_soundEffects.arraySize);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
