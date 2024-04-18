using System;
using IdleEngine.Generator;
using IdleEngine.Sessions;
using UnityEditor;
using UnityEngine;

namespace IdleEngine.Editor
{
    [CustomEditor(typeof(Session))]
    public class SessionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var session = (Session)target;

            AddProgressionButtons(session);
            AddGeneratorBuildButtons(session);
        }

        private void AddGeneratorBuildButtons(Session session)
        {
            if (session.Generator == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();

            foreach (var generator in session.Generator)
                if (generator.CanBeBuild(session))
                {
                    if (GUILayout.Button(generator.name))
                    {
                        generator.Build(session);
                    }
                }

            EditorGUILayout.EndHorizontal();
        }

        private void AddProgressionButtons(Session session)
        {
            if (GUILayout.Button("+ 1 Tag"))
            {
                session.Tick((float)TimeSpan.FromDays(1).TotalSeconds);
            }
        }
    }
}