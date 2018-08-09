using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour {

    [Header("Potty Prefab, dupe in OnClick")]
    public GameObject pottyPrefab;
    [Header("Button Display Controls")]
    [SerializeField]
    private Button thisButton;
    [SerializeField]
    private Image buttonDisplayPottyImage;
    [SerializeField]
    private Sprite standardPottySprite;
    [SerializeField]
    private Sprite selectedPottySprite;
    [SerializeField]
    private Sprite highlightedPottySprite;
    [SerializeField]
    private Sprite disabledPottySprite;

    private BuildManager buildManager;
    private PottyData pottyData;

    private SpriteState standardSpriteState;
    private SpriteState selectedSpriteState;

    private void Start()
    {
        buildManager = BuildManager.instance;
        standardSpriteState = thisButton.spriteState;

        selectedSpriteState = new SpriteState
        {
            disabledSprite = disabledPottySprite,
            highlightedSprite = selectedPottySprite,
            pressedSprite = selectedPottySprite
        };

        InvokeRepeating("SetSpriteStateToStandard", 0f, 0.25f);
    }

    private void Update()
    {
        if (buildManager.pottyToBuild != null && pottyData == null)
        {
            pottyData = buildManager.pottyToBuild.GetComponent<PottyData>();
        }
        if (pottyData != null)
        {
            if (pottyData.pottyStyle != "StandardPotty")
            {
                buttonDisplayPottyImage.sprite = standardPottySprite;
                pottyData = null;
            }
            else if (pottyData.pottyStyle == "StandardPotty")
            {
                buttonDisplayPottyImage.sprite = selectedPottySprite;
                thisButton.spriteState = selectedSpriteState;
            }
        }
    }

    private void SetSpriteStateToStandard()
    {
        if (buildManager.pottyToBuild == null)
        {
            buttonDisplayPottyImage.sprite = standardPottySprite;
            thisButton.spriteState = standardSpriteState;
            pottyData = null;
        }
    }
}
