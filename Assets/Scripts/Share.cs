using System.Collections;
using System.IO;
using UnityEngine;

public class Share : MonoBehaviour {

    public void ShareScreenshot() {
        Debug.Log("ShareScreenshot() method");
        StartCoroutine(TakeSSAndShare());
    }

    private IEnumerator TakeSSAndShare() {
        yield return new WaitForEndOfFrame();
        Debug.Log("Share Coroutine");
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());
        Debug.Log("Screenshot Saved");
        // To avoid memory leaks
        Destroy(ss);

        float score = PlayerPrefs.GetFloat("ScoreTempStore", 0);
        string shareText = "I just scored " + score + " in the game Space Shooter. I challenge you to beat it.";
        new NativeShare().AddFile(filePath).SetSubject("Space Shooter - My Score").SetText(shareText).Share();
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventShare);

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).SetText( "Hello world!" ).SetTarget( "com.whatsapp" ).Share();
    }
}
