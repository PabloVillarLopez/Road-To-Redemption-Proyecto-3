using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMountingManager : MonoBehaviour
{
    #region Piece Variables

    private float pressingTimer;
    public static bool piece1WellMounted, piece2WellMounted, piece3WellMounted, piece4WellMounted, piece5WellMounted, piece6WellMounted, piece7WellMounted;
    public static bool piece8WellMounted, piece9WellMounted, piece10WellMounted, piece11WellMounted;

    #endregion Piece Variables

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion Update

    #region OnMouse Methods

    private void OnMouseDrag()
    {
        pressingTimer++;
    }

    private void OnMouseUp()
    {
        if (pressingTimer < 5)
        {
            pressingTimer = 0;
        } 
    }

    #endregion OnMouse Methods

    #region CheckTimer Method

    private void CheckTimer()
    {
        if (pressingTimer >= 5)
        {
            //Hacer la animación o mover a algún sitio
        }
    }

    #endregion CheckTimer Method
}
