﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class GameObjectUnityEvent : UnityEvent<GameObject> { }

    [Serializable]
    public class CollectorManager : MonoBehaviour
    {
        [Tooltip("Name for the collector manager.  Helpful if you're using multiple of them.")]
        public string Name;

        [Tooltip("The number of items needed to be collected before firing the ItemsCollectedEvent.")]
        public int NumberOfItemsToCollect;

        [Tooltip("Fired when NumberOfItemsToCollect has been met.")]
        public UnityEvent CompletedEvent;

        [Tooltip("Fired each time an item is collected.")]
        public GameObjectUnityEvent CollectedEvent;

        [Tooltip("Use this to control the format of the status text.")]
        public string StatusTextFormat = "{0} of {1} collected...";

        [Tooltip("A reference to the TMP Text object you'd like to have updated.")]
        public TMP_Text TextToUpdate;

        [Tooltip("Toggle this on if you'd like to have the TextToUpdate get updated after collecting all items.")]
        public bool UpdateStatusTextOnCompletion;

        private int _itemsCollected;

        private List<GameObject> _collectedItems;
        private string CurrentStatusText => string.Format(StatusTextFormat, _itemsCollected, NumberOfItemsToCollect);

        private void Start()
        {
            _itemsCollected = 0;

            _collectedItems = new List<GameObject>();

            if (CompletedEvent == null)
            {
                CompletedEvent = new UnityEvent();
            }

            if (CollectedEvent == null)
            {
                CollectedEvent = new GameObjectUnityEvent();
            }

            if (TextToUpdate != null)
            {
                TextToUpdate.SetText(CurrentStatusText);
            }
        }

        public void ItemCollected(GameObject item)
        {
            _itemsCollected++;

            CollectedEvent.Invoke(item);

            _collectedItems.Add(item);

            if (_itemsCollected == NumberOfItemsToCollect)
            {
                CompletedEvent.Invoke();

                if (UpdateStatusTextOnCompletion)
                {
                    TextToUpdate?.SetText(CurrentStatusText);
                }
            }
            else
            {
                TextToUpdate?.SetText(CurrentStatusText);
            }
        }
    }

    [Serializable]
    public class ItemCollector : BoardTriggerBase
    {
        [Tooltip("A reference to the manager managing these items.")]
        public CollectorManager Manager;

        [Tooltip("Fired when item is collected but after we've fired event to manager.")]
        public GameObjectUnityEvent CollectedEvent;

        private GameObjectUnityEvent _itemCollectedManagerEvent;

        protected override void Start()
        {
            base.Start();

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
            if (!CanBeFiredAgain()) return;

            _itemCollectedManagerEvent?.Invoke(collider.gameObject);
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider != _boardCollider) return;
            if (!CanBeFiredAgain()) return;

            CollectedEvent.Invoke(collider.gameObject);
        }
    }
}
