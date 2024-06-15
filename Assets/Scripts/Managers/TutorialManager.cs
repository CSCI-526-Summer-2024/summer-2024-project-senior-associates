using UnityEngine;

public abstract class TutorialManager : MonoBehaviour
{
    public abstract void OnCircularProgressDone();

    public abstract void OnRequestSatisfied();

    public abstract void UpdatePhase();


}
