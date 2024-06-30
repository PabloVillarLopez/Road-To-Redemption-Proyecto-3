using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterMinigames : MonoBehaviour
{
    public int minigameID;
    public SpriteRenderer[] unlockedIcons;
    public Sprite[] lockedIcons;
    public MainMenu menuManager;

    // Start is called before the first frame update
    void Start()
    {
        LockMinigames();
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
                if (!MinigamesCompleted.minigame2Finished)
                {
                    menuManager.PlaySound(0);
                    SceneManager.LoadScene("miniJuego1");
                }
                else if (MinigamesCompleted.minigame2Finished)
                {
                    menuManager.PlaySound(2); //negative feedback
                }
                break;
            case 2: //Agua limpia
                if (!MinigamesCompleted.minigame3Finished)
                {
                    menuManager.PlaySound(0);
                    SceneManager.LoadScene("Minijuego3_AguaLimpia");
                }
                else if (MinigamesCompleted.minigame3Finished)
                {
                    menuManager.PlaySound(2); //negative feedback
                }
                break;
            case 3: //Infraestructrua
                if (!MinigamesCompleted.minigame4Finished)
                {
                    menuManager.PlaySound(0);
                    SceneManager.LoadScene("MinujuegoInfraestructura");
                }
                else if (MinigamesCompleted.minigame4Finished)
                {
                    menuManager.PlaySound(2); //negative feedback
                }
                break;
            case 4: //Energía asequible
                if (!MinigamesCompleted.minigame5Finished)
                {
                    menuManager.PlaySound(0);
                    SceneManager.LoadScene("Minijuego_Marc");
                }
                else if (MinigamesCompleted.minigame5Finished)
                {
                    menuManager.PlaySound(2); //negative feedback
                }
                break;
            case 5: //Vida submarina
                if (!MinigamesCompleted.minigame6Finished)
                {
                    menuManager.PlaySound(0);
                    SceneManager.LoadScene("Minijuego6_VidaSubmarina");
                }
                else if (MinigamesCompleted.minigame6Finished)
                {
                    menuManager.PlaySound(2); //negative feedback
                }
                break;
            case 6: //Vida de ecosistemas terrestres
                if (!MinigamesCompleted.minigame7Finished)
                {
                    menuManager.PlaySound(0);
                    SceneManager.LoadScene("Santuario_Dañado 1");
                }
                else if (MinigamesCompleted.minigame7Finished)
                {
                    menuManager.PlaySound(2); //negative feedback
                }
                break;
            case 7: //Acción por el clima
                if (!MinigamesCompleted.minigame8Finished)
                {
                    menuManager.PlaySound(0);
                    SceneManager.LoadScene("MiniGame_8");
                }
                else if (MinigamesCompleted.minigame8Finished)
                {
                    menuManager.PlaySound(2); //negative feedback
                }
                break;
            default:
                break;
        }
    }
    private void LockMinigames()
    {
        if (MinigamesCompleted.minigame2Finished)
        {
            unlockedIcons[0].sprite = lockedIcons[0];
        }

        if (MinigamesCompleted.minigame3Finished)
        {
            unlockedIcons[1].sprite = lockedIcons[1];
        }

        if (MinigamesCompleted.minigame4Finished)
        {
            unlockedIcons[2].sprite = lockedIcons[2];
        }

        if (MinigamesCompleted.minigame5Finished)
        {
            unlockedIcons[3].sprite = lockedIcons[3];
        }

        if (MinigamesCompleted.minigame6Finished)
        {
            unlockedIcons[4].sprite = lockedIcons[4];
        }

        if (MinigamesCompleted.minigame7Finished)
        {
            unlockedIcons[5].sprite = lockedIcons[5];
        }

        if (MinigamesCompleted.minigame8Finished)
        {
            unlockedIcons[6].sprite = lockedIcons[6];
        }
    }
}
