using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}