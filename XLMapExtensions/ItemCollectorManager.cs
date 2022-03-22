using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace XLMapExtensions
{
    [Serializable]
    public class GameObjectUnityEvent : UnityEvent<GameObject> { }

    [Serializable]
    public class ItemCollectorManager : MonoBehaviour
    {
        [Tooltip("Name for the collector manager.  Helpful if you're using multiple of them.")]
        public string Name;

        [Tooltip("The number of items needed to be collected before firing the ItemsCollectedEvent.")]
        public int NumberOfItemsToCollect;

        [Tooltip("Fired when NumberOfItemsToCollect has been met.")]
        public UnityEvent ItemsCollectedEvent;

        [Tooltip("Fired each time an item is collected.")]
        public GameObjectUnityEvent ItemCollectedEvent;

        [Tooltip("Use this to control the format of the status text.")]
        public string StatusTextFormat = "{0} of {1} collected...";

        public TMP_Text TextToUpdate;

        private int _itemsCollected;

        private List<GameObject> _collectedItems;
        private string CurrentStatusText => string.Format(StatusTextFormat, _itemsCollected, NumberOfItemsToCollect);

        private void Start()
        {
            _itemsCollected = 0;

            _collectedItems = new List<GameObject>();

            if (ItemsCollectedEvent == null)
            {
                ItemsCollectedEvent = new UnityEvent();
            }

            if (ItemCollectedEvent == null)
            {
                ItemCollectedEvent = new GameObjectUnityEvent();
            }

            if (TextToUpdate != null)
            {
                TextToUpdate.SetText(CurrentStatusText);
            }
        }

        public void ItemCollected(GameObject item)
        {
            _itemsCollected++;

            ItemCollectedEvent.Invoke(item);

            _collectedItems.Add(item);

            if (TextToUpdate != null)
            {
                TextToUpdate.SetText(CurrentStatusText);
            }

            if (_itemsCollected >= NumberOfItemsToCollect)
            {
                ItemsCollectedEvent.Invoke();
            }
        }
    }

    [Serializable]
    public class ItemCollectedEvent : MonoBehaviour
    {
        [Tooltip("A reference to the manager managing these items.")]
        public ItemCollectorManager Manager;

        [Tooltip("Fired when item is collected but after we've fired event to manager.")]
        public GameObjectUnityEvent CollectedEvent;

        private GameObjectUnityEvent _itemCollectedManagerEvent;

        private Collider _boardCollider;

        private void Start()
        {
            _boardCollider = PlayerController.Instance.boardController.boardColliders[0];

            if (_itemCollectedManagerEvent == null)
            {
                _itemCollectedManagerEvent = new GameObjectUnityEvent();
            }

            if (CollectedEvent == null)
            {
                CollectedEvent = new GameObjectUnityEvent();
            }

            _itemCollectedManagerEvent.AddListener(Manager.ItemCollected);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider != _boardCollider) return;

            _itemCollectedManagerEvent?.Invoke(collider.gameObject);
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider != _boardCollider) return;
            
            CollectedEvent.Invoke(collider.gameObject);
        }
    }
}
