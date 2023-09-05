using System.Collections.Generic;
using System.Linq;
using BallCollector.ScriptableObjects;
using UnityEngine;

namespace BallCollector.Gameplay
{
    public class CollectableItemsContainer : MonoBehaviour
    {
        [SerializeField] private List<CollectableItem> _allItems;
        [SerializeField] private List<float> _volumes;

        [SerializeField] private List<CollectableItem> _itemsOfUnavailableSize;

#if UNITY_EDITOR
		[SerializeField] private StartPositionData _startPositionDataEditorOnly;
		[SerializeField] private float _densityEditorOnly = 350;
#endif

		private Collector _collector; //TODO Change to Inject



        private void OnEnable()
        {
            for (var i = 0; i < _allItems.Count; i++)
            {
                _allItems[i].Collected += TryEnablingAppropriateSizeItems;
                _allItems[i].SetVolume(_volumes[i]);
            }

            _collector = FindObjectOfType<Collector>(); //TODO Change to Inject
            _itemsOfUnavailableSize = new List<CollectableItem>(_allItems);
           
        }

        private void Start()
        {
            TryEnablingAppropriateSizeItems();
        }

        private void TryEnablingAppropriateSizeItems()
        {
            int enabledItemsCount=0;
            foreach (var item in _itemsOfUnavailableSize)
            {
                if (item.Volume > _collector.CurrentVolume)
                    break;

                item.EnablePhysics();
                enabledItemsCount++;
            }
            
            _itemsOfUnavailableSize.RemoveRange(0, enabledItemsCount);
        }

#if UNITY_EDITOR

        public void GetCollectableItems()
        {
            _allItems = transform.GetComponentsInChildren<CollectableItem>().ToList();
            _volumes = new List<float>();
            foreach (var item in _allItems)
            {
                item.EnablePhysics();
                item.SetData(_densityEditorOnly);
                item.DisablePhysics();
                _volumes.Add(item.Volume);
            }

            _allItems = _allItems.OrderBy(item => item.Volume).ToList();
            _volumes.Sort();
        }


        public void GetStartPositions()
        {
            var positions = new List<Vector3>();
            var rotations=new List<Quaternion>();
            foreach(Transform child in transform)
            {
                positions.Add(child.position);
                rotations.Add(child.rotation);
            }

            _startPositionDataEditorOnly.StartPositions = positions;
            _startPositionDataEditorOnly.StartRotations = rotations;
        }
        public void SetStartPositions()
        {
           
            int i = 0;
            foreach(Transform child in transform)
            {
                child.position = _startPositionDataEditorOnly.StartPositions[i];
                child.rotation = _startPositionDataEditorOnly.StartRotations[i];

                i++;
            }
            
        }


    }

#endif
}