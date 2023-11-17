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

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        GameObject startRoom = Instantiate(startRoomPrefab);
        currentRooms.Add(startRoom);

        int roomsCount = Random.Range(minRooms, maxRooms + 1);

        for (int i = 0; i < roomsCount; i++)
        {
            GameObject roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            GameObject room = Instantiate(roomPrefab);
            SetUpRoom(currentRooms.Last(), room);

            currentRooms.Add(room);
        }

        GameObject bossRoom = Instantiate(bossRoomPrefab);
        SetUpRoom(currentRooms.Last(), bossRoom);
        currentRooms.Add(bossRoom);
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