using UnityEngine;

public class Main_menu : MonoBehaviour
{
    public static Main_menu Instance { get; set; }

    private void Start()
    {
        Instance = this;
    }
    public void Main_menu_ON()
    {
        this.gameObject.SetActive(true);
    }
    public void Main_menu_OFF()
    {
        this.gameObject.SetActive(false);
    }
}
