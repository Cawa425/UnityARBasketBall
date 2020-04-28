using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Scenes.UX.MyScene
{
    [RequireComponent(typeof(ARCameraManager))]
    public class myUIManager : MonoBehaviour
    {
        [SerializeField] [Tooltip("The ARCameraManager which will produce frame events.")]
        private ARCameraManager mCameraManager;

        /// <summary>
        /// Get or set the <c>ARCameraManager</c>.
        /// </summary>
        public ARCameraManager cameraManager
        {
            get => mCameraManager;
            set
            {
                if (mCameraManager == value)
                    return;

                if (mCameraManager != null)
                    mCameraManager.frameReceived -= FrameChanged;

                mCameraManager = value;

                if (mCameraManager != null & enabled)
                    mCameraManager.frameReceived += FrameChanged;
            }
        }

        private const string KFadeOffAnim = "FadeOff";
        private const string KFadeOnAnim = "FadeOn";

        [SerializeField] private ARPlaneManager mPlaneManager;

        public ARPlaneManager planeManager
        {
            get { return mPlaneManager; }
            set { mPlaneManager = value; }
        }

        [SerializeField] private Animator mMoveDeviceAnimation;

        public Animator moveDeviceAnimation
        {
            get => mMoveDeviceAnimation;
            set => mMoveDeviceAnimation = value;
        }

        [SerializeField] private Animator mTapToPlaceAnimation;

        public Animator tapToPlaceAnimation
        {
            get => mTapToPlaceAnimation;
            set => mTapToPlaceAnimation = value;
        }

        static List<ARPlane> s_Planes = new List<ARPlane>();

        private bool _mShowingTapToPlace = false;

        private bool _mShowingMoveDevice = true;

        void OnEnable()
        {
            if (mCameraManager != null)
                mCameraManager.frameReceived += FrameChanged;

            PlaceHoop.onPlacedObject += PlacedObject;
        }

        void OnDisable()
        {
            if (mCameraManager != null)
                mCameraManager.frameReceived -= FrameChanged;

            PlaceHoop.onPlacedObject -= PlacedObject;
        }

        void FrameChanged(ARCameraFrameEventArgs args)
        {
            if (PlanesFound() && _mShowingMoveDevice)
            {
                if (moveDeviceAnimation)
                    moveDeviceAnimation.SetTrigger(KFadeOffAnim);

                if (tapToPlaceAnimation)
                    tapToPlaceAnimation.SetTrigger(KFadeOnAnim);

                _mShowingTapToPlace = true;
                _mShowingMoveDevice = false;
            }
        }

        bool PlanesFound()
        {
            if (planeManager == null)
                return false;

            return planeManager.trackables.count > 0;
        }

        void PlacedObject()
        {
            if (_mShowingTapToPlace)
            {
                if (tapToPlaceAnimation)
                    tapToPlaceAnimation.SetTrigger(KFadeOffAnim);

                _mShowingTapToPlace = false;
            }
        }
    }
}