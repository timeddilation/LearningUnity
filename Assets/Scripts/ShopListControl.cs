using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopListControl : MonoBehaviour {

    [Header("Menu Open and Close")]
    public float menuCollapseSpeed = 5f;
    public float collapsedMenuBufferSize = 50f;

    [Header("Static Unity Objects")]
    public ScrollRect scrollbarRect;
    public Scrollbar scrollbar;
    public Sprite popMenuCaretUp;
    public Sprite popMenuCaretDown;
    public Button popMenuButton;
    private float hieghtBuffer;
    private Vector2 collapseDestination;

    [Header("Button Templates")]
    public GameObject standardPottyButtonTemplate;
    public GameObject standardPottyHandiButtonTemplate;

    private RectTransform myRectTransform;
    private bool resizingWindow = false;
    private bool isCollapsed = false;
    private bool pottyScrollMenuHidden = false;

    private void Start()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        hieghtBuffer = myRectTransform.rect.height - collapsedMenuBufferSize;
        collapseDestination = new Vector2(0, hieghtBuffer);

        //for (int i = 0; i < 8; i++)
        //{
        //    GenerateShopButtons(standardPottyButtonTemplate);
        //}

        GenerateShopButtons(standardPottyButtonTemplate);
        GenerateShopButtons(standardPottyHandiButtonTemplate);
    }

    private void Update()
    {
        if (resizingWindow) { PopMenu(); }
    }

    private void GenerateShopButtons(GameObject buttonTemplate)
    {
        GameObject button = Instantiate(buttonTemplate);
        PottyData buttonData = button.GetComponent<ButtonSelected>().pottyPrefab.GetComponent<PottyData>();
        button.SetActive(true);

        string shopButtonText = "$" + buttonData.cost.ToString();
        button.GetComponent<ShopButtonStartUp>().SetShopButtonText(shopButtonText);

        button.transform.SetParent(buttonTemplate.transform.parent, false);
    }

    public void PopMenu()
    {
        resizingWindow = true;

        if (isCollapsed)
        {
            popMenuButton.image.sprite = popMenuCaretUp;
            pottyScrollMenuHidden = false;
            TogglePottyScrollMenu();
            //myRectTransform.offsetMin = new Vector2(0f, 0f);
            Vector2 smoothTransition = Vector2.Lerp(myRectTransform.offsetMin, new Vector2(0f, 0f), Time.deltaTime * menuCollapseSpeed);
            myRectTransform.offsetMin = smoothTransition;

            if (myRectTransform.offsetMin.y < 0.15f)
            {
                isCollapsed = false;
                resizingWindow = false;
            }
        }
        else
        {
            popMenuButton.image.sprite = popMenuCaretDown;
            //myRectTransform.offsetMin = new Vector2(0f, hieghtBuffer);
            Vector2 smoothTransition = Vector2.Lerp(myRectTransform.offsetMin, collapseDestination, Time.deltaTime * menuCollapseSpeed);
            myRectTransform.offsetMin = smoothTransition;

            if ((hieghtBuffer - myRectTransform.offsetMin.y) < 0.15f)
            {
                isCollapsed = true;
                resizingWindow = false;                
            }
            //hide menu and scroll bar slightly before menu has finished collapsing
            else if ((hieghtBuffer - myRectTransform.offsetMin.y) < 5f)
            {
                pottyScrollMenuHidden = true;
                TogglePottyScrollMenu();
            }
        }
    }

    public void TogglePottyScrollMenu()
    {
        if (!pottyScrollMenuHidden)
        {
            scrollbarRect.enabled = true;
            scrollbar.gameObject.SetActive(true);
        }
        else
        {
            scrollbarRect.enabled = false;
            scrollbar.gameObject.SetActive(false);
        }
    }
}
