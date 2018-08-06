using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonStartUp : MonoBehaviour {

    [SerializeField]
    private Text thisButtonsText;

    public void SetShopButtonText(string shopButtonText)
    {
        thisButtonsText.text = shopButtonText;
    }
}
