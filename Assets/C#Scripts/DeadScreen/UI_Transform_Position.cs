using UnityEngine;
using UnityEngine.SceneManagement; //для управления сценами

public class UI_Transform_Position : MonoBehaviour
{
    public float xPercent = 0.5f; // X position as a percentage of screen width (0-1)
    public float yPercent = 0.5f; // Y position as a percentage of screen height (0-1)
    public float zPosition = 0f; // Z position in world space
    private void Start()
    {
        // Get the size of the screen in pixels
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        // Convert the percentage positions to pixel positions
        float xPos = xPercent * screenSize.x;
        float yPos = yPercent * screenSize.y;

        // Convert the pixel positions to world positions
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(xPos, yPos, zPosition));

        // Set the object's position to the calculated world position
        transform.position = worldPosition;
    }
    public void ContinueAD()
    {
        SaveSerial.Instance.LoadlLastGame();
        SaveSerial.Instance.SaveGame();
        SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
    }
    public void RestartLVL()
    {
        LvLGeneration.Instance.Restart();
    }
    public void Collectables()
    {
        SceneManager.LoadScene("Collectables", LoadSceneMode.Single);
    }
    public void JoystickSettings()
    {
        //Pause.Instance.ChangeJoystickSetting();
    }
    public void MusicSettings()
    {

    }
    public void SoundSettings()
    {

    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
