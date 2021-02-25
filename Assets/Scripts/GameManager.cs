using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool visibleMouse = false; 
    void Start()
    {
        Cursor.visible = visibleMouse;
    }
}
