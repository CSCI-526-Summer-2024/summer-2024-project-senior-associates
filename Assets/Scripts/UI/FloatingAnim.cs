using UnityEngine;

public class FloatingAnim : MonoBehaviour
{
    private const float FloatingInterval = 0.4f;
    private const float FloatingMagnitude = 0.2f;
    private float lowY = 0f;
    private float floatingCountdown = 0f;
    private bool isGoingUp = false;

    void Update()
    {
        if (floatingCountdown > 0f)
        {
            floatingCountdown -= Time.deltaTime;
            if (floatingCountdown <= 0f)
            {
                floatingCountdown = FloatingInterval;
                isGoingUp = !isGoingUp;
            }
        }
        var progress = (FloatingInterval - floatingCountdown) / FloatingInterval;
        var y = isGoingUp ? Mathf.Lerp(lowY, lowY + FloatingMagnitude, progress) : Mathf.Lerp(lowY + FloatingMagnitude, lowY, progress);
        gameObject.transform.localPosition = Util.ChangeY(gameObject.transform.localPosition, y);
    }

    public void Init(GameObject obj, Vector3 offset)
    {
        gameObject.transform.SetParent(obj.transform);
        gameObject.transform.localPosition = offset;
        lowY = gameObject.transform.localPosition.y;
        floatingCountdown = FloatingInterval;
    }
}
