using UnityEngine;

public class MenuButtonManager : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.LoadGame();
    }

    public void setLinear()
    {
        GameManager.instance.setIsLinear(true);
    }

    public void setQuadratic()
    {
        GameManager.instance.setIsLinear(false);
    }

    public void ShowInfo()
    {
        GameManager.instance.LoadInfoScene();
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
