using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopListControl : MonoBehaviour {

    public float collapsedMenuBufferSize = 50f;
    private float hieghtBuffer;
    private Vector2 collapseDestination;

    public GameObject standardPottyButtonTemplate;

    private RectTransform myRectTransform;
    private bool resizingWindow = false;
    private bool isCollapsed = false;

    private void Start()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        hieghtBuffer = myRectTransform.rect.height - collapsedMenuBufferSize;
        collapseDestination = new Vector2(0, hieghtBuffer);

        for (int i = 0; i < 8; i++)
        {
            GenerateShopButtons(standardPottyButtonTemplate);
        }     
    }

    private void Update()
    {
        if (resizingWindow) { PopMenu(); }
    }

    private void GenerateShopButtons(GameObject buttonTemplate)
    {
        GameObject button = Instantiate(buttonTemplate);
        button.SetActive(true);

        button.GetComponent<ShopButtonStartUp>().SetShopButtonText("$120");

        button.transform.SetParent(buttonTemplate.transform.parent, false);
    }

    public void PopMenu()
    {
        resizingWindow = true;

        if (isCollapsed)
        {
            myRectTransform.offsetMin = new Vector2(0f, 0f);
            isCollapsed = false;
            resizingWindow = false;
        }
        else
        {
            //myRectTransform.offsetMin = new Vector2(0f, hieghtBuffer);
            Vector2 smoothTransition = Vector2.Lerp(myRectTransform.offsetMin, collapseDestination, 1f);
            myRectTransform.offsetMin = smoothTransition;

            if ((hieghtBuffer - myRectTransform.offsetMin.y) < 1)
            {
                isCollapsed = true;
                resizingWindow = false;
            }           
        }
    }
}
