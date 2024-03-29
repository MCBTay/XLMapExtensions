﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class SetRandomObjectActive : BoardTriggerBase
    {
        [Tooltip("The list of game objects to potentially be set as active.")]
        public List<GameObject> GameObjects;

        [Tooltip("Check to set a random object active, uncheck to set a random object inactive.")]
        public bool Active = true;

        private void OnTriggerEnter(Collider collider)
        {
            if (!GameObjects.Any()) return;
            if (collider != _boardCollider) return;
            if (!CanBeFiredAgain()) return;

            var randomObjectIndex = Random.Range(0, GameObjects.Count);

            var randomObject = GameObjects[randomObjectIndex];

            randomObject.SetActive(Active);

            var audioSource = randomObject.GetComponent<AudioSource>();
            audioSource?.Play();
        }
    }
}
