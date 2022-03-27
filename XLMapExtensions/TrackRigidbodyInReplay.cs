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

        private Vector3 originalVelocity;

        private Vector3 originalAngularVelocity;

        private void Awake()
        {
            frames = new List<Frame>();

            rigidbody = gameObject.GetComponent<Rigidbody>();

            numFramesToBuffer = Mathf.RoundToInt(ReplaySettings.Instance.FPS * ReplaySettings.Instance.MaxRecordedTime);

            animation = gameObject.AddComponent<Animation>();
            animation.animatePhysics = true;

            CreateAnimationCurves();
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

        private void Update()
        {
            switch (GameStateMachine.Instance.CurrentState)
            {
                case PlayState playState:
                    ResetRigidbody();
                    RecordFrame();
                    break;
                case ReplayState replayState:
                    StoreRigidbodyState();
                    SetAnimationState();
                    break;
            }
        }

        /// <summary>
        /// Checks for the rigidbody being kinematic.  If it is, we're transitioning from ReplayState and will
        /// stop the animation from playing, reset the transform to it's original location recorded when transitioning
        /// to ReplayState, and set the rigidbody to no longer be kinematic.
        /// </summary>
        private void ResetRigidbody()
        {
            if (!rigidbody.isKinematic) return;

            if (animation.isPlaying) animation.Stop();

            transform.localPosition = originalLocalPosition;
            transform.localRotation = originalLocalRotation;

            rigidbody.velocity = originalVelocity;
            rigidbody.angularVelocity = originalAngularVelocity;

            rigidbody.isKinematic = false;
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

        /// <summary>
        /// Checks for the rigidbody being kinematic.  If it is not, we're transitioning to ReplayState and will
        /// record the position/rotation of the rididbody such that we can reset it once we leave ReplayState.  Then,
        /// sets the rigidbody to be kinematic.
        /// </summary>
        private void StoreRigidbodyState()
        {
            if (rigidbody.isKinematic) return;

            originalLocalPosition = transform.localPosition;
            originalLocalRotation = transform.localRotation;

            originalVelocity = rigidbody.velocity;
            originalAngularVelocity = rigidbody.angularVelocity;
            
            rigidbody.isKinematic = true;
        }

        private void SetAnimationState()
        {
            CreateAnimationClip();

            var playbackController = ReplayEditorController.Instance.playbackController;

            var animationState = animation[animationClip.name];
            if (!animation.isPlaying && playbackController.TimeScale != 0.0)
                animation.Play(animationClip.name);

            animationState.time = playbackController.CurrentTime;
            animationState.speed = playbackController.TimeScale;
        }

        /// <summary>
        /// Creates the initial animation clip, assigning the 7 curves to it.
        /// </summary>
        private void CreateAnimationClip()
        {
            if (animationClip != null)
                animation.RemoveClip(animationClip);

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

            animation.AddClip(animationClip, animationClip.name);
        }
    }
}
