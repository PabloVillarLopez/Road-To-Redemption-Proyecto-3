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
    [Tooltip("Tres fases: Montaje placa solar, Instalación placas y Cableado")]
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
                //Hacer lógica de que si presionas la pieza se inicie la animación de ponerse en su sitio

                //Comprobar si pieza1, pieza2, pieza3, pieza4, pieza5, pieza6 y pieza7 están bien colocada
                    //Pasar a siguiente fase

                break;
            case Phase.PHASE2:
                //Hacer lógica de sol y de porcentaje de luz natural solar según la luz que le da o el momento del día

                //Comprobar si pieza8, pieza9, pieza10 y piez11 están bien colocadas
                    //Pasar a siguiente fase

                break;
            case Phase.PHASE3:
                //Se muestra panel de cableado

                //Hacer lógica de cableado con drag and drop de los cables a su lugar correspondiente

                //Comprobar si todos los cables están bien puestos
                    //Mostrar panel de ahorro energético y como las casas empiezan a gastar menos energía
                    //Después mostrar el robot que agradece al jugador y le otorga uno de los sellos

                break;
            default:
                break;
        }
    }

    #endregion Handle MiniGame Phases
}
