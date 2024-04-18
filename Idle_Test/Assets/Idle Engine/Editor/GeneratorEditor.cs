using IdleEngine.Generator;
using UnityEditor;

namespace IdleEngine.Editor
{
    [CustomEditor(typeof(Generator.Generator))]
    public class GeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var generator = (Generator.Generator)target;

            EditorGUILayout.LabelField(nameof(Generator.Generator.NextBuildingCostsForOne), generator.NextBuildingCostsForOne.ToString());
            EditorGUILayout.LabelField(nameof(Generator.Generator.Owned), generator.Owned.ToString());
            EditorGUILayout.LabelField(nameof(Generator.Generator.ProductionCycleInSeconds), generator.ProductionCycleInSeconds.ToString());
        }
    }
}