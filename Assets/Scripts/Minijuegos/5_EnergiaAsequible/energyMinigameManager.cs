using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyMinigameManager : MonoBehaviour
{
    #region Minigame Phases


    public enum Phase
    {
        PHASE1, //Mount solar plaques
        PHASE2, //Install solar plaques
        PHASE3  //Install cables
    }

    [Header("Minigame Phase")]
    [Tooltip("Tres fases: Montaje placa solar, Instalaci�n placas y Cableado")]
    public Phase phase;

    #endregion Minigame Phases

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

    #region Handle MiniGame Phases

    private void HandleMinigamePhase()
    {
        switch (phase)
        {
            case Phase.PHASE1:
                //Hacer l�gica de que si presionas la pieza se inicie la animaci�n de ponerse en su sitio

                //Comprobar si pieza1, pieza2, pieza3, pieza4, pieza5, pieza6 y pieza7 est�n bien colocada
                    //Pasar a siguiente fase

                break;
            case Phase.PHASE2:
                //Hacer l�gica de sol y de porcentaje de luz natural solar seg�n la luz que le da o el momento del d�a

                //Comprobar si pieza8, pieza9, pieza10 y piez11 est�n bien colocadas
                    //Pasar a siguiente fase

                break;
            case Phase.PHASE3:
                //Se muestra panel de cableado

                //Hacer l�gica de cableado con drag and drop de los cables a su lugar correspondiente

                //Comprobar si todos los cables est�n bien puestos
                    //Mostrar panel de ahorro energ�tico y como las casas empiezan a gastar menos energ�a
                    //Despu�s mostrar el robot que agradece al jugador y le otorga uno de los sellos

                break;
            default:
                break;
        }
    }

    #endregion Handle MiniGame Phases
}
