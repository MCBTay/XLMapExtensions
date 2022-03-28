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

            // replay doesn't work, needs `Playback Skater Root` in front of find path
            var transformToUse = GameStateMachine.Instance.CurrentState.GetType() == typeof(ReplayState)
                ? ReplayEditorController.Instance.transform
                : PlayerController.Instance.transform;

            gameObject.transform.position = GetReferencePosition(transformToUse);

            if (UpdateRotation)
            {
                gameObject.transform.rotation = GetReferenceRotation(transformToUse);
            }
        }

        private Vector3 GetReferencePosition(Transform topLevelTransform)
        {
            switch (Target)
            {
                case TargetType.SkaterHips: return topLevelTransform.Find(SkaterPelvis).position;
                case TargetType.SkaterChest: return topLevelTransform.Find(SkaterSpine2).position;
                case TargetType.SkaterHead: return topLevelTransform.Find(SkaterHead).position;
                case TargetType.Skateboard: return topLevelTransform.Find(Skateboard).position;
                case TargetType.BackTruck: return topLevelTransform.Find(BackTruckHanger).position;
                case TargetType.FrontTruck: return topLevelTransform.Find(FrontTruckHanger).position;
                case TargetType.BackTruckWheel1: return topLevelTransform.Find(Wheel1).position;
                case TargetType.BackTruckWheel2: return topLevelTransform.Find(Wheel2).position;
                case TargetType.FrontTruckWheel3: return topLevelTransform.Find(Wheel3).position;
                case TargetType.FrontTruckWheel4: return topLevelTransform.Find(Wheel4).position;
                default: return Vector3.up;
            }
        }

        
        private Quaternion GetReferenceRotation(Transform topLevelTransform)
        {
            switch (Target)
            {
                case TargetType.SkaterHips: return topLevelTransform.Find(SkaterPelvis).rotation;
                case TargetType.SkaterChest: return topLevelTransform.Find(SkaterSpine2).rotation;
                case TargetType.SkaterHead: return topLevelTransform.Find(SkaterHead).rotation;
                case TargetType.Skateboard: return topLevelTransform.Find(Skateboard).rotation;
                case TargetType.BackTruck: return topLevelTransform.Find(BackTruckHanger).rotation;
                case TargetType.FrontTruck: return topLevelTransform.Find(FrontTruckHanger).rotation;
                case TargetType.BackTruckWheel1: return topLevelTransform.Find(Wheel1).rotation;
                case TargetType.BackTruckWheel2: return topLevelTransform.Find(Wheel2).rotation;
                case TargetType.FrontTruckWheel3: return topLevelTransform.Find(Wheel3).rotation;
                case TargetType.FrontTruckWheel4: return topLevelTransform.Find(Wheel4).rotation;
                default: return Quaternion.identity;
            }
        }
    }
}
