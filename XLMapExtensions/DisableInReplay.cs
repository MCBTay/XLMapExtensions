using System;
using GameManagement;
using UnityEngine;

namespace XLMapExtensions
{
[Serializable]
public class DisableInReplay : MonoBehaviour
{
    private void Update()
    {
        if (gameObject == null) return;

        switch (GameStateMachine.Instance.CurrentState)
        {
            case PlayState playState:
                if (!gameObject.activeSelf) gameObject.SetActive(true);
                break;
            case ReplayState replayState:
                if (gameObject.activeSelf) gameObject.SetActive(false);
                break;
        }
    }
}
}
