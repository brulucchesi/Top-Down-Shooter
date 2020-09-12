using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField]
    private TextAsset _mapJson = null;

    [SerializeField]
    private Sprite[] _tiles = null;

    private MapJson _map;

    public void GenerateMap()
    {
        _map = JsonUtility.FromJson<MapJson>(_mapJson.ToString());

        DestroyMap();

        CreateLayer();
    }

    private void DestroyMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void CreateLayer()
    {
        foreach (Layer layer in _map.layers)
        {
            GameObject o = new GameObject(layer.name);
            o.transform.SetParent(transform, false);

            if (layer.properties.Any(p => p.name == "collidable" && p.value == true))
            {
                CompositeCollider2D col = o.AddComponent<CompositeCollider2D>();
                col.geometryType = CompositeCollider2D.GeometryType.Polygons;

                o.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }

            CreateLine(layer, o);
        }

        Debug.Log("Map Generated");
    }

    private void CreateLine(Layer layer, GameObject parent)
    {
        for (int i = 0; i < _map.height; i++)
        {
            GameObject o = new GameObject("Line " + i);
            o.transform.SetParent(parent.transform, false);

            Vector3 pos = o.transform.position;
            pos.y -= i;

            o.transform.position = pos;

            List<int> lineData = layer.data.Skip(_map.width * i).Take(_map.width).ToList();

            CreateTile(lineData, layer, o);
        }
    }

    private void CreateTile(List<int> lineData, Layer layer, GameObject parent)
    {
        for (int i = 0; i < lineData.Count; i++)
        {
            int id = lineData[i] - 1;

            if (id < 0)
            {
                continue;
            }

            GameObject o = new GameObject("Tile " + i);
            o.transform.SetParent(parent.transform, false);

            Vector3 pos = o.transform.position;
            pos.x += i;

            o.transform.position = pos;

            o.AddComponent<SpriteRenderer>();

            if (layer.properties.Any(p => p.name == "collidable" && p.value == true))
            {
                o.layer = 8;

                BoxCollider2D col = o.AddComponent<BoxCollider2D>();

                col.usedByComposite = true;

                col.size = new Vector2(1, 1);
            }

            o.GetComponent<SpriteRenderer>().sprite = _tiles[id];

            o.GetComponent<SpriteRenderer>().sortingOrder = _map.layers.IndexOf(layer);
        }
    }
}
