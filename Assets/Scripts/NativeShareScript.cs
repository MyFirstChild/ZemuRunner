using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class NativeShareScript : MonoBehaviour
{
    public GameObject CanvasShareObj;
    public RawImage shareFrame;
    public Canvas canvas;

    private bool isProcessing = false;
    private bool isFocus = false;

    public void ShareBtnPress()
    {
        if (!isProcessing)
        {
            //CanvasShareObj.SetActive(true);
            
        }
        StartCoroutine(ShareScreenshot());
    }

    IEnumerator ShareScreenshot()
    {
        Debug.Log(canvas.scaleFactor);
        isProcessing = true;

        yield return new WaitForEndOfFrame();

        yield return new WaitForEndOfFrame();
        Texture2D texture = new Texture2D((int)(shareFrame.rectTransform.rect.width * canvas.scaleFactor), (int)(shareFrame.rectTransform.rect.height * canvas.scaleFactor), TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(shareFrame.rectTransform.position.x, shareFrame.rectTransform.position.y, (int)(shareFrame.rectTransform.rect.width * canvas.scaleFactor), (int)(shareFrame.rectTransform.rect.height * canvas.scaleFactor)), 0, 0);
        texture.Apply();
        //byte[] bytes = texture.EncodeToPNG();
        string destination = Path.Combine(Application.temporaryCachePath, "screenshot.png");
        //string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");
        File.WriteAllBytes(destination, texture.EncodeToPNG());
        

        //ScreenCapture.CaptureScreenshot("screenshot.png", 2);
        //string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");

        yield return new WaitForSecondsRealtime(0.3f);

        new NativeShare().AddFile(destination).SetText("https://play.google.com/store/apps/details?id=game.MyF.ZemuRunner").SetTitle("share score").Share();

        /*if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file:/" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"),
                uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"),
                "Can you beat my score?");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser",
                intentObject, "Share your new score");
            currentActivity.Call("startActivity", chooser);

            yield return new WaitForSecondsRealtime(1);
        }*/

        yield return new WaitUntil(() => isFocus);
        //CanvasShareObj.SetActive(false);
        isProcessing = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }
}