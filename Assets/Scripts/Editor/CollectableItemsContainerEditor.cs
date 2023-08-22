using BallCollector.Gameplay;
using UnityEditor;
using UnityEngine;

namespace BallCollector.EditorExtensions
{
    [CustomEditor(typeof(CollectableItemsContainer))]
    public class CollectableItemsContainerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CollectableItemsContainer container = (CollectableItemsContainer)target;

            if (GUILayout.Button("Get Collectable Items"))
            {
                container.GetCollectableItems();
            }
            GUILayout.Space(50);
            if (GUILayout.Button("Get Start Positions"))
            {
                container.GetStartPositions();
            }
            
            if (GUILayout.Button("Set Start Positions"))
            {
                container.SetStartPositions();
            }
        }
    }
}

