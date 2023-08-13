using BallCollector.Gameplay;
using UnityEditor;
using UnityEngine;

namespace BallCollector.EditorExtantions
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
        }
    }
}

