using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage_R_Direction
{
    なし,
    上,
    下,
    右,
    左
}

public class StageInfScript : MonoBehaviour
{
    public int stage_ID;
    public int dungeonID;

    public int stage_Range_Infvertical;     // ステージの縦タイルマップ数
    public int stage_Range_Infhorizontal;   // 横タイルマップ数

    public int stage_Entrance;      // 入り口のマス数
    public int stage_Entrance_Tile; // 入り口の下から数えて1マス目

    public int stage_Exit;          // 出口のマス数
    public int stage_Exit_Tile;     // 出口の下から数えて1マス目
}
