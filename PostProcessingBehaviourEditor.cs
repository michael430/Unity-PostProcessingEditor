using System;
using System.Linq.Expressions;
using UnityEngine.PostProcessing;

namespace UnityEditor.PostProcessing
{
    [CustomEditor(typeof(PostProcessingBehaviour))]
    public class PostProcessingBehaviourEditor : Editor
    {
        SerializedProperty m_Profile;
        PostProcessingInspector profileEditor;
        bool m_Foldout = true;
        
        public void OnEnable()
        {
            m_Profile = FindSetting((PostProcessingBehaviour x) => x.profile);
        }

        public override void OnInspectorGUI()
        {
            PostProcessingProfile profile = (PostProcessingProfile)m_Profile.objectReferenceValue;
            EditorGUI.BeginChangeCheck ();
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Profile);
            serializedObject.ApplyModifiedProperties();

            if(m_Profile.objectReferenceValue == null)
            {
				EditorGUILayout.HelpBox ("Please assign a PostProcessing profile here.", MessageType.Warning);
                profileEditor = null;
            }
            else
                m_Foldout = EditorGUILayout.Foldout(m_Foldout, "Show Profile Settings", true);
            if( m_Foldout)
            {
                if(!profileEditor && profile)
                    profileEditor = (PostProcessingInspector) CreateEditor (profile);
                if( profileEditor && profile)
                    profileEditor.OnInspectorGUI();
            }

            if (EditorGUI.EndChangeCheck ())
            {
                // Reset editor if new profile assigned
                profileEditor = null;
            }
        }

        SerializedProperty FindSetting<T, TValue>(Expression<Func<T, TValue>> expr)
        {
            return serializedObject.FindProperty(ReflectionUtils.GetFieldPath(expr));
        }
    }
}
