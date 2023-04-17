using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpreadTilemap : MonoBehaviour
{
    [SerializeField]
    private Tilemap floor; // Tilemap Object 바닥을 세우는 용도
    [SerializeField]
    private Tilemap wall; // Tilemap Object  벽을 세우는 용도
    [SerializeField]
    private TileBase floorTile; // 사용할 Asset이며 floor 타일맵에 까는 용도
    [SerializeField]
    private TileBase wallTile; // 사용할 Asset이며 Wall 타일맵에 까는 용도

    public void SpreadFloorTilemap(HashSet<Vector2Int> positions)
    {
        SpreadTile(positions, floor, floorTile);
    }// 파라미터로 받은 좌표에 세우는 함수
    //HashSet은 컨테이너로써 리스트와 비교하면 엘리먼트가 중복 될수 없음.
    //순서가 없고 집합과 관련된 유용한 함수가 있음.
    //TileMap에 적합

    public void SpreadWallTilemap(HashSet<Vector2Int> positions)
    {
        SpreadTile(positions, wall, wallTile);
    }// 파라미터로 받은 좌표에 세우는 함수
    //SpereadFloorTilemap과 동일

    private void SpreadTile(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(var position in positions)
        {
            tilemap.SetTile((Vector3Int)position, tile);
        }//Settile은 Vector3를 적용받기 때문에 변환해주어야함.
    }

    public void ClearAllTiles()
    {
        floor.ClearAllTiles();
        wall.ClearAllTiles();
    }//이미 깔려있는 타일맵을 모두 제거하는 역할을 함
    void Start()
    {
    }
}
