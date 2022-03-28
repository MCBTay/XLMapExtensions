using System;
using GameManagement;
using ReplayEditor;
using UnityEngine;

namespace XLMapExtensions
{
    public enum TargetType
    {
        SkaterHead,
        SkaterChest,
        SkaterHips,
        Skateboard,
        BackTruck,
        FrontTruck,
        BackTruckWheel1,
        BackTruckWheel2,
        FrontTruckWheel3,
        FrontTruckWheel4
    }

    [Serializable]
    public class PlayerReference : MonoBehaviour
    {
        public TargetType Target = TargetType.SkaterHips;

        [Tooltip("Check this to update the target game object's rotation as well as position.")]
        public bool UpdateRotation;

        private static string PlaybackSkaterRoot = "Playback Skater Root";
        private static string SkaterRoot = "NewSkater/Skater_Joints/Skater_root";
        private static string SkaterPelvis = $"{SkaterRoot}/Skater_pelvis";
        private static string SkaterSpine2 = $"{SkaterPelvis}/Skater_Spine/Skater_Spine1/Skater_Spine2";
        private static string SkaterHead = $"{SkaterSpine2}/Skater_Neck/Skater_Head";
        private static string Skateboard = "Skateboard/Deck";
        private static string BackTruckHanger = $"{Skateboard}/Back Truck/Hanger";
        private static string FrontTruckHanger = $"{Skateboard}/Front Truck/Hanger";
        private static string Wheel1 = $"{BackTruckHanger}/Wheel1";
        private static string Wheel2 = $"{BackTruckHanger}/Wheel2";
        private static string Wheel3 = $"{FrontTruckHanger}/Wheel3";
        private static string Wheel4 = $"{FrontTruckHanger}/Wheel4";

        private void Update()
        {
            gameObject.transform.position = GetReferencePosition();

            if (UpdateRotation)
            {
                gameObject.transform.rotation = GetReferenceRotation();
            }
        }

        private Vector3 GetReferencePosition()
        {
            return GetTransform()?.position ?? Vector3.up;
        }

        
        private Quaternion GetReferenceRotation()
        {
            return GetTransform()?.rotation ?? Quaternion.identity;
        }

        private Transform GetTransform()
        {
            var transformToUse = GameStateMachine.Instance.CurrentState.GetType() == typeof(ReplayState)
                ? ReplayEditorController.Instance.transform
                : PlayerController.Instance.transform;

            var searchPath = GetSearchPath();
            
            return string.IsNullOrEmpty(searchPath) ? null : transformToUse.Find(searchPath);
        }

        private string GetSearchPath()
        {
            var searchPath = string.Empty;

            switch (Target)
            {
                case TargetType.SkaterHips: searchPath = SkaterPelvis; break;
                case TargetType.SkaterChest: searchPath = SkaterSpine2; break;
                case TargetType.SkaterHead: searchPath = SkaterHead; break;
                case TargetType.Skateboard: searchPath = Skateboard; break;
                case TargetType.BackTruck: searchPath = BackTruckHanger; break;
                case TargetType.FrontTruck: searchPath = FrontTruckHanger; break;
                case TargetType.BackTruckWheel1: searchPath = Wheel1; break;
                case TargetType.BackTruckWheel2: searchPath = Wheel2; break;
                case TargetType.FrontTruckWheel3: searchPath = Wheel3; break;
                case TargetType.FrontTruckWheel4: searchPath = Wheel4; break;
                default: searchPath = string.Empty; break;
            }

            if (GameStateMachine.Instance.CurrentState.GetType() == typeof(ReplayState))
            {
                searchPath = $"{PlaybackSkaterRoot}/{searchPath}";
            }

            return searchPath;
        }
    }
}
