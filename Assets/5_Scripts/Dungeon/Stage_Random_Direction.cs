using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage_Direction
{
    なし,
    上,
    下,
    右,
    左
}

public class Stage_Random_Direction : MonoBehaviour
{
    public Stage_Direction direction_new;
    public int direction_new_Blocks;

    public Stage_Direction direction_last;
    public int direction_last_Blocks;
}
