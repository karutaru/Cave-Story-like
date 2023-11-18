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
    private int roomsCount; // ランダムで決められた部屋数
    private GameObject lastRoomPrefab;
    private int randomValue;
    private int lastrandomValue;


    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        GameObject startRoom = Instantiate(startRoomPrefab); // 最初の部屋を生成
        currentRooms.Add(startRoom); // 生成した部屋のリストに入れる

        roomsCount = Random.Range(minRooms, maxRooms + 1);　// 部屋数をランダムに決める
        Stage_Direction lastExitDirection = startRoom.GetComponent<Stage_Random_Direction>().direction_last; // 最初の部屋の出口の方向
        int lastExitBlocksDirection = startRoom.GetComponent<Stage_Random_Direction>().direction_last_Blocks; // 最初の部屋の出口のマス数

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

    // 次に生成するステージを決める
    GameObject GetNextRoomPrefab(Stage_Direction lastExitDirection, int lastExitBlocksDirection)
    {
        if (lastrandomValue == 1)
        {
            randomValue = Random.Range(1, 3 + 1); // 1から3のランダム値を取得
        }
        else if(lastrandomValue== 4)
        {
            randomValue = Random.Range(2, 4 + 1); // 2から4のランダム値を取得
        }
        else
        {
            randomValue = Random.Range(1, 4 + 1); // 1から4のランダム値を取得
        }
        //Debug.Log(randomValue);

        List<GameObject> rightRooms = new List<GameObject>();   // 出口が右のリストを作る
        List<GameObject> upRooms = new List<GameObject>();      // 出口が上のリストを作る
        List<GameObject> underRooms = new List<GameObject>();   // 出口が下のリストを作る


        foreach (var prefab in roomPrefabs) // 作ったステージをvar prefabに入れ計算
        {
            
            Stage_Random_Direction direction = prefab.GetComponent<Stage_Random_Direction>();

            if (IsDirectionCompatible(lastExitDirection, lastExitBlocksDirection, direction.direction_new, direction.direction_new_Blocks)) // 出口と入り口の方向が一致しているか
            {
                if (direction.direction_last == Stage_Direction.右) // 確率に基づいて出口が右のステージを選択
                {
                    rightRooms.Add(prefab);
                }
                else if (direction.direction_last == Stage_Direction.下) // 確率に基づいて出口が下のステージを選択
                {
                    underRooms.Add(prefab);
                }
                else if (direction.direction_last == Stage_Direction.上) // 確率に基づいて出口が上のステージを選択
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
        //    Debug.Log("中身がない");
        //}
        // 条件に合致する部屋が見つからない場合
        //Debug.LogError(roomsCount);
        roomsCount++;
        return null; // またはデフォルトの部屋を返す
    }

    bool IsDirectionCompatible(Stage_Direction lastExit, int lastExitBlocksDirection, Stage_Direction newEntrance, int direction_new_Blocks)
    {
        // 出口と入り口の方向の互換性をチェック
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

                // Bounds情報を使用して新しい部屋の位置を調整
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