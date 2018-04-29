using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bresenham2D : MonoBehaviour {
    //Bresenham 算法绘制2d线段
    public class Node2D
    {
        public int x;
        public int y;

        public Node2D(int _x,int _y)
        {
            x = _x;
            y = _y;
        }     
        public Node2D(Node2D node2D)
        {
            x = node2D.x;
            y = node2D.y;
        }
    }

    public GameObject _cube;    //像素点

    public Node2D StartPos = new Node2D(0,0);    //起始点
    public Node2D EndPos = new Node2D(0,0);      //终点

    public Text startX;
    public Text startY;
    public Text endX;
    public Text endY;
    public Text Note;

    private float k;

    private int p0;             //第一个决策值
    private int pCurrent;
    private int pNext;

    private int dx;
    private int dy;

    private Node2D currentNode;
    private Node2D nextNode = new Node2D(0,0);

    #region 输入初始点x,y与终点x,y坐标
    public void EnterStartX()
    {
        try
        {
            Note.gameObject.SetActive(false);
            StartPos.x = int.Parse(startX.text);
        }
        catch(FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    public void EnterStartY()
    {
        try
        {
            Note.gameObject.SetActive(false);
            StartPos.y = int.Parse(startY.text);
        }
        catch (FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    public void EnterEndX()
    {
        try
        {
            Note.gameObject.SetActive(false);
            EndPos.x = int.Parse(endX.text);
        }
        catch (FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    public void EnterEndY()
    {
        try
        {
            Note.gameObject.SetActive(false);
            EndPos.y = int.Parse(endY.text);
        }
        catch (FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    #endregion

    #region 画线
    public void DrawLine () {
        dx = EndPos.x - StartPos.x;
        dy = EndPos.y - StartPos.y;
        k =  (float)dy / (float)dx;

        if(EndPos.x < StartPos.x)
        {
            Node2D temp = new Node2D(0, 0);
            temp = EndPos;
            EndPos = StartPos;
            StartPos = temp;
        }

        currentNode = new Node2D(StartPos.x, StartPos.y);
        Debug.Log("K = "+k);

        //斜率大于1
        if (k>=1)
        {
            p0 = 2 * dx - dy;
            pCurrent = p0;
            NextNode1();
        }
        //斜率大于0小于1
        else if(k>=0 && k<1)
        {
            p0 = 2 * dy - dx;
            pCurrent = p0;
            NextNode2();
        }
        //斜率大于-1小于0
        else if(k>=-1 && k<0)
        {
            p0 = dx - 2 * dy;
            pCurrent = p0;
            NextNode3();
        }
        //斜率小于-1
        else if(k < -1)
        {
            p0 = 2 * dx + dy;
            pCurrent = p0;
            NextNode4();
        }  
	}
    #endregion

    #region 斜率大于1
    private void NextNode1()
    {
        while(currentNode.x <= EndPos.x && currentNode.y <= EndPos.y)
        {
            if (pCurrent < 0)
            {
                nextNode.x = currentNode.x;
                nextNode.y = currentNode.y + 1;
                pNext = pCurrent + 2 * dx;
            }
            else
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y + 1;
                pNext = pCurrent + 2 * dx - 2 * dy;
            }
            pCurrent = pNext;
            currentNode.x = nextNode.x;
            currentNode.y = nextNode.y;
            Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        }    
    }
    #endregion

    #region 斜率大于0小于1
    private void NextNode2()
    {
        while (currentNode.x <= EndPos.x && currentNode.y <= EndPos.y)
        {
            if (pCurrent < 0)
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y;
                pNext = pCurrent + 2 * dy;
            }
            else
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y + 1;
                pNext = pCurrent + 2 * dy - 2 * dx;
            }
            pCurrent = pNext;
            currentNode.x = nextNode.x;
            currentNode.y = nextNode.y;
            Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
    #endregion

    #region 斜率大于-1小于0
    private void NextNode3()
    {
        while (currentNode.x <= EndPos.x && currentNode.y >= EndPos.y)
        {
            if (pCurrent < 0)
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y;
                pNext = pCurrent - 2 * dy;
            }
            else
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y - 1;
                pNext = pCurrent - 2 * dx - 2 * dy;
            }
            pCurrent = pNext;
            currentNode.x = nextNode.x;
            currentNode.y = nextNode.y;
            Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
    #endregion

    #region 斜率小于-1
    private void NextNode4()
    {
        while (currentNode.x <= EndPos.x && currentNode.y >= EndPos.y)
        {
            if (pCurrent < 0)
            {
                nextNode.x = currentNode.x;
                nextNode.y = currentNode.y - 1;
                pNext = pCurrent + 2 * dx;
            }
            else
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y - 1;
                pNext = pCurrent + 2 * dx + 2 * dy;
            }
            pCurrent = pNext;
            currentNode.x = nextNode.x;
            currentNode.y = nextNode.y;
            Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
    #endregion
}
