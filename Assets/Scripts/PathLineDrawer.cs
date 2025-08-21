using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class DotPathRenderer : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform startPoint;
    public Transform endPoint; // Bu değişebilir!
    public GameObject dotPrefab;

    [Header("Ayarlar")]
    public float dotSpacing = 0.5f;
    [SerializeField] private float transitionDistance = 10f;

    private Seeker seeker;
    private List<GameObject> dots = new List<GameObject>();

    private Vector3 lastStartPos;
    private float updateThreshold = 0.1f;

    private bool hasStartedPath = false; // Daha önce path başlatıldı mı?

    
    void Start()
    {
        
        seeker = GetComponent<Seeker>();

        if (startPoint == null)
        {
            Debug.LogWarning("startPoint atanmadı.");
            enabled = false;
            return;
        }
        
        lastStartPos = startPoint.position;
    }

    void Update()
    {
        // Hedef atanmadıysa hiçbir şey yapma
        if (endPoint == null) return;

        // Daha önce hiç path başlatılmadıysa, şimdi başlat
        if (!hasStartedPath)
        {
            seeker.StartPath(startPoint.position, endPoint.position, OnPathComplete);
            hasStartedPath = true;
            return;
        }

        // Hareket edildiyse yeni yol çiz
        if (Vector3.Distance(startPoint.position, lastStartPos) > updateThreshold)
        {
            lastStartPos = startPoint.position;
            seeker.StartPath(startPoint.position, endPoint.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path path)
    {
        if (endPoint == null || path.error || path.vectorPath == null || path.vectorPath.Count < 2)
            return;

        // Önceki noktaları sil
        foreach (var dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();

        float distanceAccumulator = 0f;
        Vector3 previousPoint = path.vectorPath[0];

        for (int i = 4; i < path.vectorPath.Count; i++)
        {
            Vector3 currentPoint = path.vectorPath[i];
            float segmentDistance = Vector3.Distance(previousPoint, currentPoint);
            distanceAccumulator += segmentDistance;

            if (distanceAccumulator >= dotSpacing)
            {
                float distanceToEnd = Vector3.Distance(currentPoint, endPoint.position);
                float t = Mathf.InverseLerp(transitionDistance, 0f, distanceToEnd);
                t = Mathf.Clamp01(t);

                Color dotColor = Color.Lerp(Color.red, Color.green, t);
                InstantiateDot(currentPoint, dotColor);
                distanceAccumulator = 0f;
            }

            previousPoint = currentPoint;
        }
    }

    void InstantiateDot(Vector3 position, Color color)
    {
        position.z = 0;
        GameObject dot = Instantiate(dotPrefab, position, Quaternion.identity);

        var sr = dot.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = color;
        }

        dots.Add(dot);
    }
}
