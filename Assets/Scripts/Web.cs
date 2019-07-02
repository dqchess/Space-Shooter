using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour {
    public void OpenPrivacyPolicy() {
        Application.OpenURL("https://space-shooter-0.flycricket.io/privacy.html");
    }
}
