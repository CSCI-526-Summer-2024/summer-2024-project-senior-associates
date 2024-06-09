using UnityEngine;
using TMPro
;
public class CurrentScoreText : MonoBehaviour
{
    private readonly float Duration = 0.5f;
    private readonly float OriginalSize = 0.4f;
    private readonly float SizeMultiplier = 2f;
    private readonly Color OriginalColor = Color.white;
    private readonly Color TargetColor = Color.green;
    private TMP_Text tmp;
    private int currentScore = 0;
    private float elapsedTime;


    private enum AnimationState { None, Growing, Shrinking }
    private AnimationState animationState = AnimationState.None;

    private void Start()
    {
        tmp = GetComponent<TMP_Text>();
        UpdateText();
    }

    private void Update()
    {
        if (animationState != AnimationState.None)
        {
            elapsedTime += Time.deltaTime;
            if (animationState == AnimationState.Growing)
            {
                if (elapsedTime < Duration)
                {
                    UpdateTextWithAnimation(elapsedTime / Duration, OriginalColor, TargetColor, OriginalSize, OriginalSize * SizeMultiplier);
                }
                else
                {
                    elapsedTime = 0f;
                    animationState = AnimationState.Shrinking;
                }
            }
            else
            {
                if (elapsedTime < Duration)
                {
                    UpdateTextWithAnimation(elapsedTime / Duration, TargetColor, OriginalColor, OriginalSize * SizeMultiplier, OriginalSize);
                }
                else
                {
                    animationState = AnimationState.None;
                    UpdateText();
                }
            }
        }
    }

    public void AddScore(int score)
    {
        currentScore += score;
        elapsedTime = 0f;
        animationState = AnimationState.Growing;
        UpdateText();
    }

    public Vector3 GetBottomRightPoint()
    {
        TMP_TextInfo textInfo = tmp.textInfo;
        TMP_CharacterInfo lastCharacter = textInfo.characterInfo[textInfo.characterCount - 1];
        return tmp.transform.TransformPoint(lastCharacter.bottomRight);
    }

    private void UpdateText()
    {
        tmp.text = $"<size={OriginalSize}>{currentScore}</size>";
    }

    private void UpdateTextWithAnimation(float f, Color startColor, Color endColor, float startSize, float EndSize)
    {
        Color color = Color.Lerp(startColor, endColor, f);
        float size = Mathf.Lerp(startSize, EndSize, f);
        tmp.text = $"<size={size}><color=#{ColorUtility.ToHtmlStringRGB(color)}>{currentScore}</color></size>";
    }
}