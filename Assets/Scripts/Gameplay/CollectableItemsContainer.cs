using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BallCollector.Gameplay
{
    public class CollectableItemsContainer : MonoBehaviour
    {
        [SerializeField] private List<CollectableItem> _allItems;
        [SerializeField] private List<float> _volumes;

        [SerializeField] private List<CollectableItem> _itemsOfUnavailableSize;

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
                item.SetVolume();
                item.DisablePhysics();
                _volumes.Add(item.Volume);
            }

            _allItems = _allItems.OrderBy(item => item.Volume).ToList();
            _volumes.Sort();
        }

#endif
    }
}