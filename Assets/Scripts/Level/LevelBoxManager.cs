using UnityEngine;

public class SceneController : MonoBehaviour
{
    public int levelCount;
    public GameObject levelBoxPrefab;
    private const float BackgroundXOffset = -1.51f;
    private const float TitleHeight = 2.2f;
    private const float BackgroundYOffset = -.2f - TitleHeight / 2; // -1.3
    private const float BackgroundWidth = 17.76f;
    private const float BackgroundHeight = 10f - TitleHeight; // 7.8
    private const float LevelBoxWidth = 2.66f;
    private const float LevelBoxHeight = 1.5f;
    private const int NumLevelBoxPerRow = 4;
    private const int NumLevelBoxPerCol = 3;
    private const float GapWidth = (BackgroundWidth - NumLevelBoxPerRow * LevelBoxWidth) / (NumLevelBoxPerRow + 1);
    private const float GapHeight = (BackgroundHeight - NumLevelBoxPerCol * LevelBoxHeight) / (NumLevelBoxPerCol + 1); // 0.825
    private const float XOffsetStart = BackgroundXOffset - BackgroundWidth / 2 + GapWidth + LevelBoxWidth / 2;
    private const float XOffsetStep = GapWidth + LevelBoxWidth;
    private const float YOffsetStart = BackgroundYOffset + BackgroundHeight / 2 - GapHeight - LevelBoxHeight / 2;
    private const float YOffsetStep = GapHeight + LevelBoxHeight;
    private PlayerData playerData;

    void Start()
    {
        playerData = PlayerData.LoadPlayerData(levelCount);
        CreateLevelBoxes(levelCount);
    }

    public void CreateLevelBoxes(int levelCount)
    {
        int row = 0;
        int col = 0;
        for (int i = 0; i < levelCount; i++)
        {
            var levelBox = Instantiate(levelBoxPrefab, GetPosition(row, col), Quaternion.identity);
            levelBox.GetComponent<LevelBox>().SetLevelNumAndBestScore(i + 1, playerData.bestScores[i]);
            col++;
            if (col >= NumLevelBoxPerRow)
            {
                col -= NumLevelBoxPerRow;
                row++;
            }
        }
    }

    private Vector3 GetPosition(int row, int col)
    {
        return new(XOffsetStart + col * XOffsetStep, YOffsetStart - row * YOffsetStep, 0f);
    }
}
