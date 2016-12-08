using UnityEngine;

namespace ImprovedContentManager.Util
{
    public class UpdateHook : MonoBehaviour
    {
        public delegate void OnUnityUpdate();

        public delegate void OnUnityDestroy();

        public OnUnityUpdate onUnityUpdate = null;
        public OnUnityDestroy onUnityDestroy = null;
        public bool once = true;

        void Update()
        {
            if (onUnityUpdate != null)
            {
                onUnityUpdate();
                if (once)
                {
                    Destroy(this);
                }
            }
        }

        void OnDestroy()
        {
            if (onUnityDestroy != null)
            {
                onUnityDestroy();
            }
        }

    }
}
