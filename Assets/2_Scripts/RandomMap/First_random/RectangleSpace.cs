using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSpace
{
    public Vector2Int leftDown;
    public int width;
    public int height;
    //직사각형을 표현하기 위해서 설정

    public RectangleSpace(Vector2Int leftDown, int width, int height)
    {
        this.leftDown = leftDown;
        this.width = width;
        this.height = height;
    }

    public Vector2Int Center()
    {
        return new Vector2Int(((leftDown.x * 2) + width - 1) / 2, ((leftDown.y * 2) + height - 1) / 2);
    } // 공간의 중심 좌표를 리턴해주는 함수

}
