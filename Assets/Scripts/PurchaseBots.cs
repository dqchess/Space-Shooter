using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseBots : MonoBehaviour {


    [SerializeField] Button bot1Button;
    [SerializeField] Button bot2Button;

    public void EnableBot1Button(bool state) {
        bot1Button.interactable = state;
    }

    public void EnableBot2Button(bool state) {
        bot2Button.interactable = state;
    }
    
}
