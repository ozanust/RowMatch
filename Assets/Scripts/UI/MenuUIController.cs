using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField]
    private DreamButton levelsButton;
    [SerializeField]
    private DreamScrollView levelsScrollPanel;
    [SerializeField]
    private LevelsContent levelsContent;
    [SerializeField]
    private DreamUI highScorePanel;
    [SerializeField]
    private ParticleSystem highScoreParticles;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        highScorePanel.onClick += OnClickHighScorePanel;

		if (gameManager.levelEndData != null)
		{
            if (gameManager.levelEndData.IsNewHighScore)
            {
                OnNewHighScore();
			}

            levelsScrollPanel.gameObject.SetActive(true);
            levelsButton.gameObject.SetActive(false);
        }
		else
		{
            levelsButton.AddListener(OnClickLevelsButton);
        }
    }

    private void OnClickLevelsButton()
	{
        levelsScrollPanel.gameObject.SetActive(true);
        levelsButton.gameObject.SetActive(false);
	}

    private void OnNewHighScore()
	{
        highScorePanel.gameObject.SetActive(true);
        highScoreParticles.Play();
	}

    private void OnClickHighScorePanel()
	{
        highScorePanel.gameObject.SetActive(false);
        levelsContent.UnlockLevelButton(gameManager.levelEndData.FinishedLevelNumber + 1);
	}
}
