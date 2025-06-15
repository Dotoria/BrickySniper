using System.Collections;
using Common;
using UnityEngine;

namespace Lobby
{
    public class Capture : MonoBehaviour
    {
        [SerializeField] private GameObject _deniedUI;
        [SerializeField] private GameObject _failedUI;

        public void CaptureScreen()
        {
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string fileName = "CELLQUAD-SCREENSHOT-" + timestamp + ".png";

#if UNITY_IPHONE || UNITY_ANDROID
            StartCoroutine(CaptureScreenForMobile(fileName));
#else
        StartCoroutine(CaptureScreenForPC(fileName));
#endif
        }

        private IEnumerator CaptureScreenForPC(string fileName)
        {
            yield return new WaitForEndOfFrame();

            ScreenCapture.CaptureScreenshot("~/Downloads/" + fileName);
        }

        private IEnumerator CaptureScreenForMobile(string fileName)
        {
            yield return new WaitForEndOfFrame();

            Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

            NativeGallery.Permission permission =
                NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
            if (permission == NativeGallery.Permission.Denied)
            {
                UIManager.Instance.OpenUI(_deniedUI);
                yield break;
            }

            string albumName = "CELLQUAD";
            NativeGallery.SaveImageToGallery(texture, albumName, fileName);

            // cleanup
            Object.Destroy(texture);
        }

        public void CloseDeniedUI(bool setting)
        {
            UIManager.Instance.CloseUI(_deniedUI);

            if (setting && NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();
            else if (setting && !NativeGallery.CanOpenSettings()) UIManager.Instance.PopUp(_failedUI);
        }
    }
}