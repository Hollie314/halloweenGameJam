using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{

    public GameObject MainPanel;

    private void Awake()
    {
        Cursor.visible = true;
    }

    public void Back(GameObject currentPannel)
    {
        currentPannel.SetActive(false);
        MainPanel.SetActive(true);
    }

    public void OpenPanel(GameObject panelToOpen)
    {
        MainPanel.SetActive(false);
        panelToOpen.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ProtoLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CreditsLola()
    {
        Application.OpenURL("https://www.artstation.com/aykoma");
    }
    public void CreditsLino()
    {
        Application.OpenURL("https://lil-demon-is-watching.itch.io/");
    }
    public void CreditsSalim()
    {
        Application.OpenURL("https://hollie31415.itch.io/");
    }
    public void CreditsRaph()
    {
        Application.OpenURL("https://jobless-slime.itch.io/");
    }
    public void CreditsChloe()
    {
        Application.OpenURL("https://www.artstation.com/adriaen_th");
    }
    public void CreditsShae()
    {
        Application.OpenURL("https://www.artstation.com/mythdraw");
    }
    public void CreditsAmanda()
    {
        Application.OpenURL("https://ivydrag0n.artstation.com/");
    }
    public void CreditsLois()
    {
        Application.OpenURL("https://yuefty.itch.io/");
    }
}
