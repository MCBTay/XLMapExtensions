using System.Collections.Generic;
using System.Linq;
using GameManagement;
using ReplayEditor;
using UnityEngine;

namespace XLMapExtensions
{
    public class Frame
    {
        public float time;
        public Vector3 position;
        public Quaternion rotation;

        public Frame(Transform transform)
        {
            position = transform.localPosition;
            rotation = transform.localRotation;
            time = PlayTime.time;
        }
    }

    public class TrackRigidbodyInReplay : MonoBehaviour
    {
        private List<Frame> frames;
        private Rigidbody rigidbody;

        private int numFramesToBuffer;

        private float nextRecordTime;
        
        private Animation animation;
        private AnimationClip animationClip;

        AnimationCurve xPosCurve;
        AnimationCurve yPosCurve;
        AnimationCurve zPosCurve;
        
        AnimationCurve xRotCurve;
        AnimationCurve yRotCurve;
        AnimationCurve zRotCurve;
        AnimationCurve wRotCurve;

        /// <summary>
        /// Used to place the rigidbody back to it's original position when transitioning back to PlayState from ReplayState.
        /// </summary>
        private Vector3 originalLocalPosition;
        /// <summary>
        /// Used to place the rigidbody back to it's original rotation when transitioning back to PlayState from ReplayState.
        /// </summary>
        private Quaternion originalLocalRotation;

        private void Awake()
        {
            frames = new List<Frame>();

            rigidbody = gameObject.GetComponent<Rigidbody>();

            numFramesToBuffer = Mathf.RoundToInt(ReplaySettings.Instance.FPS * ReplaySettings.Instance.MaxRecordedTime);

            animation = gameObject.AddComponent<Animation>();
            animation.animatePhysics = true;

            CreateAnimationCurves();
            CreateAnimationClip();
        }

        private void CreateAnimationCurves()
        {
            xPosCurve = new AnimationCurve();
            yPosCurve = new AnimationCurve();
            zPosCurve = new AnimationCurve();

            xRotCurve = new AnimationCurve();
            yRotCurve = new AnimationCurve();
            zRotCurve = new AnimationCurve();
            wRotCurve = new AnimationCurve();
        }

        private void CreateAnimationClip()
        {
            animationClip = new AnimationClip
            {
                legacy = true,
                name = gameObject.name
            };

            animationClip.SetCurve(string.Empty, typeof(Transform), "localPosition.x", xPosCurve);
            animationClip.SetCurve(string.Empty, typeof(Transform), "localPosition.y", yPosCurve);
            animationClip.SetCurve(string.Empty, typeof(Transform), "localPosition.z", zPosCurve);

            animationClip.SetCurve(string.Empty, typeof(Transform), "localRotation.x", xRotCurve);
            animationClip.SetCurve(string.Empty, typeof(Transform), "localRotation.y", yRotCurve);
            animationClip.SetCurve(string.Empty, typeof(Transform), "localRotation.z", zRotCurve);
            animationClip.SetCurve(string.Empty, typeof(Transform), "localRotation.w", wRotCurve);
        }

        private void ResetRigidbody()
        {
            if (!rigidbody.isKinematic) return;

            if (animation.isPlaying) animation.Stop();

            transform.localPosition = originalLocalPosition;
            transform.localRotation = originalLocalRotation;

            rigidbody.isKinematic = false;
        }

        private void Update()
        {
            if (GameStateMachine.Instance.CurrentState.GetType() == typeof(PlayState))
            {
                ResetRigidbody();
                RecordFrame();
                return;
            }

            if (GameStateMachine.Instance.CurrentState.GetType() != typeof(ReplayState)) return;


            if (!rigidbody.isKinematic)
            {
                originalLocalPosition = transform.localPosition;
                originalLocalRotation = transform.localRotation;
                rigidbody.isKinematic = true;
            }

            animation.AddClip(animationClip, animationClip.name);

            var animationState = animation[animationClip.name];
            if (!animation.isPlaying && ReplayEditorController.Instance.playbackController.TimeScale != 0.0)
                animation.Play(animationClip.name);

            animationState.time = ReplayEditorController.Instance.playbackController.CurrentTime;
            animationState.speed = ReplayEditorController.Instance.playbackController.TimeScale;
        }

        private void RecordFrame()
        {
            if (nextRecordTime > PlayTime.time) return;

            if (nextRecordTime < PlayTime.time - 1.0)
                nextRecordTime = PlayTime.time + 0.03333334f;
            else
                nextRecordTime += 0.03333334f;
            
            if (frames.Count >= numFramesToBuffer)
            {
                var framesToRemove = frames.Count - numFramesToBuffer;
                RemovePrunedFramesFromAnimationCurves(framesToRemove);
                frames.RemoveRange(0, framesToRemove);
            }

            var frame = new Frame(transform);
            frames.Add(frame);

            UpdateAnimationCurves(frame);
        }

        private void RemovePrunedFramesFromAnimationCurves(int numKeysToRemove)
        {
            for (int i = 0; i < numKeysToRemove; i++)
            {
                xPosCurve.RemoveKey(i);
                yPosCurve.RemoveKey(i);
                zPosCurve.RemoveKey(i);

                xRotCurve.RemoveKey(i);
                yRotCurve.RemoveKey(i);
                zRotCurve.RemoveKey(i);
                wRotCurve.RemoveKey(i);
            }
        }

        private void UpdateAnimationCurves(Frame frame)
        {
            xPosCurve.AddKey(frame.time, frame.position.x);
            yPosCurve.AddKey(frame.time, frame.position.y);
            zPosCurve.AddKey(frame.time, frame.position.z);

            xRotCurve.AddKey(frame.time, frame.rotation.x);
            yRotCurve.AddKey(frame.time, frame.rotation.y);
            zRotCurve.AddKey(frame.time, frame.rotation.z);
            wRotCurve.AddKey(frame.time, frame.rotation.w);
        }
    }
}
