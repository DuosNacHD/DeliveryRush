using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
[RequireComponent(typeof(Tilemap))]
public class IsoZFixer : MonoBehaviour
{
    void OnEnable()
    {
        FixZOrdering();
    }

    void OnValidate()
    {
        FixZOrdering();
    }

    void FixZOrdering()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        if (tilemap == null) return;

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            // X + Y'ye göre Z ofset ver
            float zOffset = (pos.x + pos.y) * 0.001f;
            tilemap.SetTransformMatrix(pos, Matrix4x4.TRS(new Vector3(0, tilemap.GetTransformMatrix(pos).GetPosition().y, zOffset), Quaternion.identity, Vector3.one));
            
        }
    }
}