public class LevelEndData
{
    public int FinishedLevelNumber;
    public bool IsNewHighScore;

    public LevelEndData(int levelNumber, bool isHighscore)
	{
        FinishedLevelNumber = levelNumber;
        IsNewHighScore = isHighscore;
	}
}
