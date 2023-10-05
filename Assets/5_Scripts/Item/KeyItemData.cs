using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemData
{
    [LabelText("キーアイテムの画像"), HideLabel]
    [PreviewField(40)]
    public Sprite keyItemImage;

    public int keyItemID;
    public string keyItemName;
}
