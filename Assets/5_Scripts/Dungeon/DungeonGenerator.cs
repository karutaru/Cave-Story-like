using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject startRoomPrefab;
    public GameObject bossRoomPrefab;
    public int minRooms;
    public int maxRooms;

    private List<GameObject> currentRooms = new List<GameObject>();
    private int roomsCount; // �����_���Ō��߂�ꂽ������
    private GameObject lastRoomPrefab;
    private int randomValue;
    private int lastrandomValue;


    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        GameObject startRoom = Instantiate(startRoomPrefab); // �ŏ��̕����𐶐�
        currentRooms.Add(startRoom); // �������������̃��X�g�ɓ����

        roomsCount = Random.Range(minRooms, maxRooms + 1);�@// �������������_���Ɍ��߂�
        Stage_Direction lastExitDirection = startRoom.GetComponent<Stage_Random_Direction>().direction_last; // �ŏ��̕����̏o���̕���
        int lastExitBlocksDirection = startRoom.GetComponent<Stage_Random_Direction>().direction_last_Blocks; // �ŏ��̕����̏o���̃}�X��

        for (int i = 0; i < roomsCount; i++)
        {
            
            GameObject roomPrefab = GetNextRoomPrefab(lastExitDirection, lastExitBlocksDirection);
            if (lastRoomPrefab != roomPrefab && roomPrefab != null)
            {
                GameObject room = Instantiate(roomPrefab);
                SetUpRoom(currentRooms.Last(), room);

                lastExitDirection = room.GetComponent<Stage_Random_Direction>().direction_last;
                currentRooms.Add(room);

                lastRoomPrefab = roomPrefab;
            }
        }

        GameObject bossRoom = Instantiate(bossRoomPrefab);
        SetUpRoom(currentRooms.Last(), bossRoom);
        currentRooms.Add(bossRoom);
    }

    // ���ɐ�������X�e�[�W�����߂�
    GameObject GetNextRoomPrefab(Stage_Direction lastExitDirection, int lastExitBlocksDirection)
    {
        if (lastrandomValue == 1)
        {
            randomValue = Random.Range(1, 3 + 1); // 1����3�̃����_���l���擾
        }
        else if(lastrandomValue== 4)
        {
            randomValue = Random.Range(2, 4 + 1); // 2����4�̃����_���l���擾
        }
        else
        {
            randomValue = Random.Range(1, 4 + 1); // 1����4�̃����_���l���擾
        }
        //Debug.Log(randomValue);

        List<GameObject> rightRooms = new List<GameObject>();   // �o�����E�̃��X�g�����
        List<GameObject> upRooms = new List<GameObject>();      // �o������̃��X�g�����
        List<GameObject> underRooms = new List<GameObject>();   // �o�������̃��X�g�����


        foreach (var prefab in roomPrefabs) // ������X�e�[�W��var prefab�ɓ���v�Z
        {
            
            Stage_Random_Direction direction = prefab.GetComponent<Stage_Random_Direction>();

            if (IsDirectionCompatible(lastExitDirection, lastExitBlocksDirection, direction.direction_new, direction.direction_new_Blocks)) // �o���Ɠ�����̕�������v���Ă��邩
            {
                if (direction.direction_last == Stage_Direction.�E) // �m���Ɋ�Â��ďo�����E�̃X�e�[�W��I��
                {
                    rightRooms.Add(prefab);
                }
                else if (direction.direction_last == Stage_Direction.��) // �m���Ɋ�Â��ďo�������̃X�e�[�W��I��
                {
                    underRooms.Add(prefab);
                }
                else if (direction.direction_last == Stage_Direction.��) // �m���Ɋ�Â��ďo������̃X�e�[�W��I��
                {
                    upRooms.Add(prefab);
                }
            }
        }
        if (randomValue == 2 || randomValue == 3)
        {
            lastrandomValue = randomValue;
            return rightRooms[Random.Range(0, rightRooms.Count)];
        } 
        else if (randomValue == 1) 
        {
            lastrandomValue = randomValue;
            return underRooms[Random.Range(0, underRooms.Count)];
        }
        else if (randomValue == 4)
        {
            lastrandomValue = randomValue;
            return upRooms[Random.Range(0, upRooms.Count)];
        }
        //if (validRooms.Count == 0)
        //{
        //    roomsCount++;
        //    Debug.Log("���g���Ȃ�");
        //}
        // �����ɍ��v���镔����������Ȃ��ꍇ
        //Debug.LogError(roomsCount);
        roomsCount++;
        return null; // �܂��̓f�t�H���g�̕�����Ԃ�
    }

    bool IsDirectionCompatible(Stage_Direction lastExit, int lastExitBlocksDirection, Stage_Direction newEntrance, int direction_new_Blocks)
    {
        // �o���Ɠ�����̕����̌݊������`�F�b�N
        return lastExit == newEntrance && lastExitBlocksDirection == direction_new_Blocks;
    }

    void SetUpRoom(GameObject lastRoom, GameObject newRoom)
    {
        var lastExit = FindChildObjects(lastRoom, "Random_Exit").FirstOrDefault();
        var newEntrance = FindChildObjects(newRoom, "Random_Entrance").FirstOrDefault();

        if (lastExit != null && newEntrance != null)
        {
            CompositeCollider2D lastExitCollider = lastExit.GetComponent<CompositeCollider2D>();
            CompositeCollider2D newEntranceCollider = newEntrance.GetComponent<CompositeCollider2D>();

            if (lastExitCollider != null && newEntranceCollider != null)
            {
                Vector3 lastExitBoundsCenter = lastExitCollider.bounds.center;
                Vector3 newEntranceBoundsCenter = newEntranceCollider.bounds.center;

                // Bounds�����g�p���ĐV���������̈ʒu�𒲐�
                Vector3 positionOffset = lastExitBoundsCenter - newEntranceBoundsCenter;
                newRoom.transform.position = lastRoom.transform.position + positionOffset;
            }
        }
    }

    IEnumerable<GameObject> FindChildObjects(GameObject parent, string name)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == name)
            {
                yield return child.gameObject;
            }
            foreach (var grandChild in FindChildObjects(child.gameObject, name))
            {
                yield return grandChild;
            }
        }
    }
}