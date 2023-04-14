using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideSpace : MonoBehaviour
{//전체 공간 토탈스페이스를 나누고 리스트에 저장하는 역할

    public int totalWidth;
    public int totalHeight;
    //전체 공간의 너비와 높이


    [SerializeField]
    private int minWidth; 
    [SerializeField]
    private int minHeight;
    //공간이 가지고있는 최소한의 너비와 높이

    public RectangleSpace totalSpace;
    // (0,0) ~ (totalWidth, totalHeight) 까지 가진 RectangleSpace

    public List<RectangleSpace> spaceList;
    //나누어진 공간을 저장해야하기 때문에 List 설정

    public void DivideRoom(RectangleSpace space)
    {// 나누지 못할때까지 나눈후 스페이스 리스트에 저장하기 위한 함수
        // 공간이 최소치의 2배이상이면 공간을 나눔.
        if(space.height >= minHeight * 2 && space.width >= minWidth * 2)
        {
            if(Random.Range(0, 2) < 1)
            {
                RectangleSpace[] spaces = DivideHorizontal(space);

                DivideRoom(spaces[0]);
                DivideRoom(spaces[1]);
            }
            else
            {
                RectangleSpace[] spaces = DivideVertical(space);

                DivideRoom(spaces[0]);
                DivideRoom(spaces[1]);
            }
        }// 높이가 최소높이의 2배보다 크거나같거나 너비가 최소 너비의 2배보다 크거나 같으면 가로 또는 세로로 자르도록 함.
        else if(space.height < minHeight * 2 && space.width >= minWidth * 2)
        {
            RectangleSpace[] spaces = DivideVertical(space);

            DivideRoom(spaces[0]);
            DivideRoom(spaces[1]);
        }// 높이가 최소높이의 2배보다 작거나 너비가 최소너비의 2배보다 크거나 같으면 세로로 자르도록 함.
        else if(space.height >= minHeight * 2 && space.width < minWidth * 2)
        {
            RectangleSpace[] spaces = DivideHorizontal(space);

            DivideRoom(spaces[0]);
            DivideRoom(spaces[1]);
        }// 높이가 최소높이보다 크거나 같고 너비가 최소너비의 2배보다 작으면 가로로 자르도록 함.
        else
        {
            spaceList.Add(space);
        }// 높이와 너비가 최소의 2배보다 작으면 멈추고 리스트에 저장함.
    }

    private RectangleSpace[] DivideHorizontal(RectangleSpace space)
    {// 공간을 가로로 자르는 함수
        int newSpace1Height = minHeight + Random.Range(0, space.height - minHeight * 2 + 1);
        RectangleSpace newSpace1 = new RectangleSpace(space.leftDown, space.width, newSpace1Height);

        int newSpace2Height = space.height - newSpace1Height;
        Vector2Int newSpace2LeftDown = new Vector2Int(space.leftDown.x, space.leftDown.y + newSpace1Height);
        RectangleSpace newSpace2 = new RectangleSpace(newSpace2LeftDown, space.width, newSpace2Height);

        RectangleSpace[] spaces = new RectangleSpace[2];
        spaces[0] = newSpace1;
        spaces[1] = newSpace2;
        return spaces;
    }

    private RectangleSpace[] DivideVertical(RectangleSpace space)
    {// 공간을 세로로 자르는 함수
        int newSpace1Wdith = minWidth + Random.Range(0, space.width - minWidth * 2 + 1);
        RectangleSpace newSpace1 = new RectangleSpace(space.leftDown, newSpace1Wdith, space.height);

        int newSpace2Width = space.width - newSpace1Wdith;
        Vector2Int newSpace2LeftDown = new Vector2Int(space.leftDown.x + newSpace1Wdith, space.leftDown.y);
        RectangleSpace newSpace2 = new RectangleSpace(newSpace2LeftDown, newSpace2Width, space.height);

        RectangleSpace[] spaces = new RectangleSpace[2];
        spaces[0] = newSpace1;
        spaces[1] = newSpace2;
        return spaces;
    }
    void Start()
    {
    }

   }
