using UnityEditor;

[CustomEditor(typeof(SettingsPanelManager))]
public class SettingsPanelManagerEditor : Editor
{
    SerializedProperty settingsPanelProp;
    SerializedProperty overlayProp;
    SerializedProperty settingsButtonProp;

    SerializedProperty animationDurationProp;
    SerializedProperty showEaseProp;
    SerializedProperty hideEaseProp;
    SerializedProperty hiddenPositionProp;
    SerializedProperty shownPositionProp;

    SerializedProperty useButtonAnimationProp;
    SerializedProperty rotationDurationProp;
    SerializedProperty rotationAngleProp;
    SerializedProperty rotationEaseProp;

    SerializedProperty onShowSettingsProp;
    SerializedProperty onHideSettingsProp;

    private void OnEnable()
    {
        // Lấy các SerializedProperty từ SettingsPanelManager
        settingsPanelProp = serializedObject.FindProperty("_settingsPanel");
        overlayProp = serializedObject.FindProperty("_overlay");
        settingsButtonProp = serializedObject.FindProperty("_settingsButton");

        animationDurationProp = serializedObject.FindProperty("_animationDuration");
        showEaseProp = serializedObject.FindProperty("showEase");
        hideEaseProp = serializedObject.FindProperty("hideEase");
        hiddenPositionProp = serializedObject.FindProperty("hiddenPosition");
        shownPositionProp = serializedObject.FindProperty("shownPosition");

        useButtonAnimationProp = serializedObject.FindProperty("useButtonAnimation");
        rotationDurationProp = serializedObject.FindProperty("rotationDuration");
        rotationAngleProp = serializedObject.FindProperty("rotationAngle");
        rotationEaseProp = serializedObject.FindProperty("rotationEase");

        onShowSettingsProp = serializedObject.FindProperty("OnShowSettings");
        onHideSettingsProp = serializedObject.FindProperty("OnHideSettings");
    }

    public override void OnInspectorGUI()
    {
        // Cập nhật đối tượng được serialized
        serializedObject.Update();

        // Hiển thị các thuộc tính đúng thứ tự như trong mã gốc

        // Các thuộc tính SettingsPanel, Overlay, và SettingsButton
        EditorGUILayout.PropertyField(settingsPanelProp);
        EditorGUILayout.PropertyField(overlayProp);
        EditorGUILayout.PropertyField(settingsButtonProp);

        // Phần Animation Settings
        EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(animationDurationProp);
        EditorGUILayout.PropertyField(showEaseProp);
        EditorGUILayout.PropertyField(hideEaseProp);
        EditorGUILayout.PropertyField(hiddenPositionProp);
        EditorGUILayout.PropertyField(shownPositionProp);

        // Phần Button Animation Settings
        EditorGUILayout.PropertyField(useButtonAnimationProp);
        if (useButtonAnimationProp.boolValue)
        {
            EditorGUILayout.PropertyField(rotationDurationProp);
            EditorGUILayout.PropertyField(rotationAngleProp);
            EditorGUILayout.PropertyField(rotationEaseProp);
        }

        // Phần Events
        EditorGUILayout.PropertyField(onShowSettingsProp);
        EditorGUILayout.PropertyField(onHideSettingsProp);

        // Áp dụng thay đổi
        serializedObject.ApplyModifiedProperties();
    }
}