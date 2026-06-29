using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Dungeon Settings")]
    public Slider widthSlider;
    public Slider heightSlider;
    public Slider minRoomSizeSlider;

    public GameObject visualizerButton;
    public GameObject playButton;

    /// <summary>
    /// Initialisiert die UI-Elemente basierend auf dem aktuellen Zustand des Dungeons.
    /// </summary>
    void Start()
    {
        bool hasDungeon = DungeonManager.Instance?.DungeonData != null;
        if (visualizerButton != null) visualizerButton.SetActive(hasDungeon);
        if (playButton != null) playButton.SetActive(hasDungeon);
    }

    /// <summary>
    /// Generiert einen neuen Dungeon basierend auf den Werten der Slider und aktiviert die entsprechenden Buttons,
    /// wenn der Dungeon erfolgreich erstellt wurde.
    /// </summary>
    public void GenerateDungeon()
    {
        int width = Mathf.RoundToInt(widthSlider.value);
        int height = Mathf.RoundToInt(heightSlider.value);
        int minRoom = Mathf.RoundToInt(minRoomSizeSlider.value);

        DungeonManager.Instance.GenerateDungeon(width, height, minRoom);
        if (visualizerButton != null) visualizerButton.SetActive(true);
        if (playButton != null) playButton.SetActive(true);
    }
    public void LoadVisualizer()
    {
        SceneManager.LoadScene("Visualizer");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
