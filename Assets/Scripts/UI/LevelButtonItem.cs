using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LevelButtonItem : DreamButton
{
    [SerializeField]
    private SpriteRenderer buttonRenderer;
    [SerializeField]
    private SpriteRenderer lockIconRenderer;
    [SerializeField]
    private TMP_Text levelNumberText;
    [SerializeField]
    private TMP_Text highestScoreText;
    [SerializeField]
    private GameObject playTextObject;

    private int levelNumber;
    private int highestLevelScore;
    private bool isLocked;

    public int LevelNumber => levelNumber;
    public int HighestLevelScore => highestLevelScore;

    public Action<int> OnClick;

    public void SetupButton(int levelNumber, int moveCount, int score, bool isLocked)
	{
        this.levelNumber = levelNumber;
        highestLevelScore = score;
        this.isLocked = isLocked;

        lockIconRenderer.gameObject.SetActive(isLocked);
        playTextObject.SetActive(!isLocked);
        buttonRenderer.color = isLocked ? Color.gray : Color.green;

        levelNumberText.text = string.Format("{0} {1} - {2} {3}", "Level", levelNumber, moveCount, "moves");
        highestScoreText.text = string.Format("{0}: {1}", "Highest Score", score);
    }

	private void Start()
	{
        Debug.Log("start");
        AddListener(OnClickButton);
	}

    private void OnClickButton()
	{
        Debug.Log(isLocked);
        if (!isLocked)
        {
            OnClick?.Invoke(levelNumber);
        }
	}

    public void Unlock()
	{
        isLocked = false;
        lockIconRenderer.gameObject.SetActive(isLocked);
        playTextObject.SetActive(!isLocked);
        buttonRenderer.color = isLocked ? Color.gray : Color.green;
    }
}
