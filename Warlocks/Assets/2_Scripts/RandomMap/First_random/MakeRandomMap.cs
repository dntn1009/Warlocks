using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRandomMap : MonoBehaviour
{
    [SerializeField]
    private int distance;
    [SerializeField]
    private int minRoomWidth;
    [SerializeField]
    private int minRoomHeight;
    //방을 만들때 쓰는 변수
    [SerializeField]
    private DivideSpace divideSpace;
    //나누어진 공간(spcaeList)를 가져오기위해 쓰임.
    [SerializeField]
    private SpreadTilemap spreadTilemap;
    //방과복도에 벽타일을 깔기위해 사용.
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject entrance;

    private HashSet<Vector2Int> floor; // 방의 좌표
    private HashSet<Vector2Int> wall; // 벽의 좌표

    // Start is called before the first frame update
    void Start()
    {
        StartRandomMap();
    }

    public void StartRandomMap() // 랜덤맵을 만드는것을 실행하는 함수.
    {
        spreadTilemap.ClearAllTiles(); // 깔려있는 모든타일 삭제
        divideSpace.totalSpace = new RectangleSpace(new Vector2Int(0, 0), divideSpace.totalWidth, divideSpace.totalHeight);
        divideSpace.spaceList = new List<RectangleSpace>();
        floor = new HashSet<Vector2Int>();
        wall = new HashSet<Vector2Int>();
        divideSpace.DivideRoom(divideSpace.totalSpace);
        //SpaceList 생성
        MakeRandomRooms();
        //방
        MakeCorridors();
        //복도
        MakeWall();
        //벽
        spreadTilemap.SpreadFloorTilemap(floor);
        spreadTilemap.SpreadWallTilemap(wall);
        // 벽과복드에 타일맵을 깔아줌.

        //player.transform.position = (Vector2)divideSpace.spaceList[0].Center();
        //entrance.transform.position = (Vector2)divideSpace.spaceList[divideSpace.spaceList.Count - 1].Center();
    }

    private void MakeRandomRooms() // 방을 만드는 함수
    {
        foreach(var space in divideSpace.spaceList)
        {
            HashSet<Vector2Int> positions = MakeARandomRectangleRoom(space);
            floor.UnionWith(positions);
        }// 모든 space를 가져오고 floor에 리턴되는 모든 좌표들을 floor에 넣어줌.
        //UnionWith = HashSet에 쓰이며 합집합 용도.
    }

    private HashSet<Vector2Int> MakeARandomRectangleRoom(RectangleSpace space) // MakeRandomRooms에 참조되어있음
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
        int width = Random.Range(minRoomWidth, space.width + 1 - distance * 2); // 최소길이보다 길지만 distance를 뺸 거리의 방
        int height = Random.Range(minRoomHeight, space.height + 1 - distance * 2);
        for(int i = space.Center().x - width / 2; i <= space.Center().x + width / 2; i++)
        {
            for(int j = space.Center().y - height / 2; j < space.Center().y + height / 2; j++)
            {
                positions.Add(new Vector2Int(i, j));
            }
        }// 방의 중심을 구하기위해 쓰임.
        return positions;
    }

    private void MakeCorridors() // 복도를 만드는 함수
    {
        List<Vector2Int> tempCenters = new List<Vector2Int>();
        foreach(var space in divideSpace.spaceList)
        {
            tempCenters.Add(new Vector2Int(space.Center().x, space.Center().y));
        }
        Vector2Int nextCenter;
        Vector2Int currentCenter = tempCenters[0];
        tempCenters.Remove(currentCenter);
        while(tempCenters.Count != 0)
        {
            nextCenter = ChooseShortestNextCorridor(tempCenters, currentCenter);
            MakeOneCorridor(currentCenter, nextCenter);
            currentCenter = nextCenter;
            tempCenters.Remove(currentCenter);
        }
    }

    private Vector2Int ChooseShortestNextCorridor(List<Vector2Int> tempCenters, Vector2Int previousCenter)
    { // MakeCorridors에 참조되어있음
        int n = 0;
        float minLength = float.MaxValue;
        for(int i = 0; i < tempCenters.Count; i++)
        {
            if(Vector2Int.Distance(previousCenter, tempCenters[i]) < minLength)
            {
                minLength = Vector2.Distance(previousCenter, tempCenters[i]);
                n = i;
            }
        }
        return tempCenters[n];
    }
            
    private void MakeOneCorridor(Vector2Int currentCenter, Vector2Int nextCenter)
    { // MakeCorridors에 참조되어있음
        Vector2Int current = new Vector2Int(currentCenter.x, currentCenter.y);
        Vector2Int next = new Vector2Int(nextCenter.x, nextCenter.y);
        floor.Add(current);
        while(current.x != next.x)
        {
            if(current.x < next.x)
            {
                current.x += 1;
                floor.Add(current);
            }
            else
            {
                current.x -= 1;
                floor.Add(current);
            }
        }
        while(current.y != next.y)
        {
            if(current.y < next.y)
            {
                current.y += 1;
                floor.Add(current);
            }
            else
            {
                current.y -= 1;
                floor.Add(current);
            }
        }
    }

    private void MakeWall() // 벽을 만드는 함수
    {
        foreach(Vector2Int tile in floor)
        {
            HashSet<Vector2Int> boundary = Make3X3Square(tile);
            boundary.ExceptWith(floor);
            if(boundary.Count != 0)
            {
                wall.UnionWith(boundary);
            }
        }
    }

    private HashSet<Vector2Int> Make3X3Square(Vector2Int tile)
    { // MakeWall에 참조되어있음
        HashSet<Vector2Int> boundary = new HashSet<Vector2Int>();
        for(int i = tile.x - 1; i <= tile.x + 1; i++)
        {
            for(int j = tile.y - 1; j <= tile.y + 1; j++)
            {
                boundary.Add(new Vector2Int(i, j));
            }
        }
        return boundary;
    }
}
