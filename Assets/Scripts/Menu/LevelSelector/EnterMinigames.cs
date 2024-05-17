using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterMinigames : MonoBehaviour
{
    public int minigameID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        switch (minigameID)
        {
            case 1: //Hambre cero
                Debug.Log("Minijuego bloqueado por ahora.");
                break;
            case 2: //Agua limpia
                Debug.Log("Minijuego bloqueado por ahora.");
                break;
            case 3: //Infraestructrua
                SceneManager.LoadScene("MiniJuego4");
                break;
            case 4: //Energía asequible
                Debug.Log("Minijuego bloqueado por ahora.");
                break;
            case 5: //Vida submarina
                SceneManager.LoadScene("Minijuego6_VidaSubmarina");
                break;
            case 6: //Vida de ecosistemas terrestres
                SceneManager.LoadScene("Santuario_Dañado 1");
                break;
            case 7: //Acción por el clima
                SceneManager.LoadScene("MiniJuego8");
                break;
            default:
                break;
        }
    }
}
