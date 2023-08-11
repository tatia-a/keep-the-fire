using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject defaultPanel;

    [SerializeField] private GameObject rules;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameoverPanel;

    private Image[] fireImages;
    [SerializeField] private Sprite emptyFireSprite;
    [SerializeField] private Sprite fullFireSprite;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameoverScoreText;

    public static UI instance;
    private void Start()
    {
        instance = this;

        CountFireSprites();
        UpdateFireAmountUI();

        menuPanel.SetActive(true);
        defaultPanel.SetActive(true);
        rules.SetActive(false);
        gamePanel.SetActive(false);
        gameoverPanel.SetActive(false);
    }
    private void Update()
    {
        if (LevelController.GameStarted)
        {
            gamePanel.SetActive(true);
            scoreText.text = Mathf.Round(LevelController.instance.Score).ToString();
        }
        if(LevelController.instance.GameOver)
        {
            gamePanel.SetActive(false);
            gameoverPanel.SetActive(true);
            gameoverScoreText.text = Mathf.Round(LevelController.instance.Score).ToString();
        }

    }

    public void ShowRules()
    {
        defaultPanel.SetActive(false);
        rules.SetActive(true);
    }
    public void HideRules()
    {
        rules.SetActive(false);
        defaultPanel.SetActive(true);
    }
    public void StartGame()
    {

        LevelController.instance.StartLevel();
        menuPanel.SetActive(false);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void UpdateFireAmountUI()
    {
        foreach (var item in fireImages)
        {
            item.sprite = emptyFireSprite;
        }
        for (int i = 0; i < LevelController.instance.FireAmount; i++)
        {
            fireImages[i].sprite = fullFireSprite;
        }
    }    

    private void CountFireSprites()
    {
        var fireImagesObj = GameObject.FindGameObjectsWithTag("FireSprite");
        fireImages = new Image[fireImagesObj.Length];
        for (int i = 0; i < fireImagesObj.Length; i++)
        {
            fireImages[i] = fireImagesObj[i].GetComponent<Image>();
        }
    }
}
