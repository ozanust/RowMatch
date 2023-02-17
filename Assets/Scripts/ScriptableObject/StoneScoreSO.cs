using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoneScoreLib", order = 2)]
public class StoneScoreSO : ScriptableObject
{
    public StoneScore[] stoneScores;

	public int GetScoreOfType(StoneType type)
	{
		foreach (StoneScore ss in stoneScores)
		{
			if (ss.Type == type)
			{
				return ss.Score;
			}
		}

		return 0;
	}
}