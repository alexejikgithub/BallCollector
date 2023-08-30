using System;
using BallCollector.CoreSystem;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class PlayButton : MonoBehaviour
{
   public event Action LevelChosen;
   [SerializeField] private Button _button;



   private void OnEnable()
   {
      _button.onClick.AddListener(ChoseLevel);
   }

   private void OnDisable()
   {
      _button.onClick.RemoveListener(ChoseLevel);
   }

   private void ChoseLevel()
   {
      LevelChosen?.Invoke();
   }
}

