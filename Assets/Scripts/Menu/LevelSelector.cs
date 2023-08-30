using BallCollector.CoreSystem;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using Zenject;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private SimpleScrollSnap _scrollSnap;

    public int GetSelectedLevel()
    {
        return _scrollSnap.SelectedPanel;
    }
}
