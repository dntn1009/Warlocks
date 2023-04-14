using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 cameraPosition = new Vector3(0, 0, -10);

    float height;
    float width;

    GameObject player;
    bool tag_check = false;

    [SerializeField]
    Vector2 center; // 카메라가 비출 센터 조정
    [SerializeField]
    Vector2 mapSize; // 맵사이즈 조정

    [SerializeField]
    float cameraMoveSpeed;

    //맵 이동 시 화면 이동
    public bool Move_check = false;
    public float Move_speed = 1f;
    public int NextDirection;
    Vector2 Init_camera;
    
    private void Start()
    {
        Init_camera = center;
        Invoke("player_tag", 0.1f);
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }
    private void Update()
    {
        Next_move();
    }

    private void FixedUpdate()
    {
        LimitCameraArea();
    }

    void player_tag()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tag_check = true;
    }

    void camera_move()
    {
        if (tag_check)
            transform.position = player.transform.position + cameraPosition;
    }
    void LimitCameraArea()
    {
        if (tag_check)
        {
            transform.position = Vector3.Lerp(transform.position,
                                              player.transform.position + cameraPosition,
                                              Time.deltaTime * cameraMoveSpeed);
            float lx = mapSize.x - width;
            float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

            float ly = mapSize.y - height;
            float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }

    void Next_move() // 맵 이동시 카메라 이동
    {
        if (Move_check)
        {
            switch(NextDirection)
            {
                case 0:
                    center += Vector2.up * 40;
                    player.transform.position = Vector2.up * 32.5f;
                    NextDirection = -1;
                    Move_check = false;
                    player.GetComponent<PlayerController>().Next_move = false;
                    break;
                case -1:
                    center -= Vector2.up * 40;
                    player.transform.position = Vector2.up * 7.5f;
                    NextDirection = 0;
                    Move_check = false;
                    player.GetComponent<PlayerController>().Next_move = false;
                    break;
                case 1:
                    center.y += Move_speed * Time.deltaTime;
                    if (center.y >= Init_camera.y + 16f)
                    {
                        center.y = Init_camera.y + 16f;
                        Init_camera = center;
                        Move_check = false;
                    }
                    break;
                case 2:
                    center.y -= Move_speed * Time.deltaTime;
                    if (center.y <= Init_camera.y - 16f)
                    {
                        center.y = Init_camera.y - 16f;
                        Init_camera = center;
                        Move_check = false;
                    }
                    break;
                case 3:
                    center.x += Move_speed * Time.deltaTime;
                    if (center.x >= Init_camera.x + 16f)
                    {
                        center.x = Init_camera.x + 16f;
                        Init_camera = center;
                        Move_check = false;
                    }
                    break;
                case 4:
                    center.x -= Move_speed * Time.deltaTime;
                    if (center.x <= Init_camera.x - 16f)
                    {
                        center.x = Init_camera.x - 16f;
                        Init_camera = center;
                        Move_check = false;
                    }
                    break;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}
