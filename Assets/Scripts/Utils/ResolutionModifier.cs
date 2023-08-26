using UnityEngine;

namespace HNW.Utils
{
    public class ResolutionModifier : MonoBehaviour
    {
        [Tooltip("-1 la plataforma destino usa su default frame rate")]
        [SerializeField] Resolution[] resolutions;

        private void Awake()
        {
            resolutions = Screen.resolutions;
            SetFrameRate(Screen.currentResolution);
        }

        public void SetFrameRate(Resolution targetResolution)
        {
            Screen.SetResolution(targetResolution.width, targetResolution.height, FullScreenMode.ExclusiveFullScreen, targetResolution.refreshRateRatio);
            Debug.Log(targetResolution.ToString());
        }
    }
}