using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage_R_Direction
{
    �Ȃ�,
    ��,
    ��,
    �E,
    ��
}

public class StageInfScript : MonoBehaviour
{
    public int stage_ID;
    public int dungeonID;

    public int stage_Range_Infvertical;     // �X�e�[�W�̏c�^�C���}�b�v��
    public int stage_Range_Infhorizontal;   // ���^�C���}�b�v��

    public int stage_Entrance;      // ������̃}�X��
    public int stage_Entrance_Tile; // ������̉����琔����1�}�X��

    public int stage_Exit;          // �o���̃}�X��
    public int stage_Exit_Tile;     // �o���̉����琔����1�}�X��
}
