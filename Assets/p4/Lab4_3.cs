using UnityEngine;
using UnityEngine.UIElements;
public class Lab4_3 : VisualElement
{
    VisualElement[] items = new VisualElement[5];
    string currentItemType;
    public string ItemType
    {
        get => currentItemType;
        set
        {
            currentItemType = value;
            UpdateItems();
        }
    }

    int nItems;
    public int Items
    {
        get => nItems;
        set
        {
            nItems = value;
            UpdateItems();
        }
    }

    public Lab4_3()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("plantilla_p4"));
        for (int i = 0; i < 5; i++)
        {
            items[i] = new VisualElement();
            items[i].style.width = 100;
            items[i].style.height = 100;
            items[i].style.backgroundImage = Resources.Load<Texture2D>(currentItemType);

            items[i].AddToClassList("panel_round");

            hierarchy.Add(items[i]);
        }
    }

    void UpdateItems()
    {
        for (int i = 0; i < 5; i++)
        {
            if (currentItemType == "sword"){
                items[i].style.backgroundColor = new StyleColor(new Color(1.0f, 0.0f, 0.0f, 0.75f));
            }
            else if (currentItemType == "shield"){
                items[i].style.backgroundColor = new StyleColor(new Color(0.0f, 1.0f, 0.0f, 0.75f));
            }
            else{
                items[i].style.backgroundColor = new StyleColor(new Color(0.5f, 0.5f, 0.5f, 0.65f));
            }

            if (i < nItems)
            {
                items[i].style.opacity = 1.0f;
            }
            else
            {
                items[i].style.opacity = 0.5f;
            }
        }
    }

    public new class UxmlFactory : UxmlFactory<Lab4_3, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription myItemType = new UxmlStringAttributeDescription { name = "item-type", defaultValue = "sword" };
        UxmlIntAttributeDescription myItemCount = new UxmlIntAttributeDescription { name = "item-count", defaultValue = 3 };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var element = ve as Lab4_3;
            element.ItemType = myItemType.GetValueFromBag(bag, cc);
            element.Items = myItemCount.GetValueFromBag(bag, cc);
        }
    }
}
