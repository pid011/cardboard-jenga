
using Google.XR.Cardboard;

using UnityEngine;

namespace CardboardJenga
{
    public class CardboardController : MonoBehaviour
    {
        public void Start()
        {
            // Configures the app to not shut down the screen and sets the brightness to maximum.
            // Brightness control is expected to work only in iOS, see:
            // https://docs.unity3d.com/ScriptReference/Screen-brightness.html.
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Screen.brightness = 1.0f;

            // Checks if the device parameters are stored and scans them if not.
            if (!Api.HasDeviceParams())
            {
                Api.ScanDeviceParams();
            }
        }

        public void Update()
        {
            if (Api.IsGearButtonPressed)
            {
                Api.ScanDeviceParams();
            }

            if (Api.IsCloseButtonPressed)
            {
                Application.Quit();
            }

            if (Api.IsTriggerHeldPressed)
            {
                Api.Recenter();
            }

            if (Api.HasNewDeviceParams())
            {
                Api.ReloadDeviceParams();
            }

#if !UNITY_EDITOR
    Api.UpdateScreenParams();
#endif
        }
    }
}