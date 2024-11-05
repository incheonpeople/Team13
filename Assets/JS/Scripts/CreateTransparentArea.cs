using UnityEngine;

public class CreateTransparentArea : MonoBehaviour
{
    public float radius = 5f; 
    public Material transparentMaterial; 

    private void Start()
    {
        CreateCircle();  
    }

    private void CreateCircle()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>(); 
        }

        lineRenderer.positionCount = 50;  
        lineRenderer.useWorldSpace = false;  
        lineRenderer.material = transparentMaterial;  
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        float angleStep = 360f / lineRenderer.positionCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            lineRenderer.SetPosition(i, point);
        }
    }
}
