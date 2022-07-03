using UnityEngine;

public class Border : MonoBehaviour
{
    private LineRenderer lineRendererComponent;

    private void Awake()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
    }

    public void SetBorder(float width, float height)
    {
        Vector3[] points = new Vector3[4];
        float lineWidth = lineRendererComponent.startWidth;
        float halfWidthXPos = width / 2 + transform.position.x + lineWidth;
        float halfHeightZPos = height / 2 + transform.position.z + lineWidth;
        points[0] = new Vector3(-halfWidthXPos, 0, halfHeightZPos);
        points[1] = new Vector3(halfWidthXPos, 0, halfHeightZPos);
        points[2] = new Vector3(halfWidthXPos, 0, -halfHeightZPos);
        points[3] = new Vector3(-halfWidthXPos, 0, -halfHeightZPos);
        
        lineRendererComponent.SetPositions(points);
    }

    public void SetEnable(bool enable)
    {
        lineRendererComponent.enabled = enable;
    }
}
