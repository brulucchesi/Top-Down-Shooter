using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private TextAsset _mapJson = null;

    [SerializeField]
    private Sprite[] _tiles = null;

    private Map _map;

    // Start is called before the first frame update
   public void GenerateMap()
    {
        _map = JsonUtility.FromJson<Map>(_mapJson.ToString());

        DestroyMap();

        CreateLayer();
    }

    private void DestroyMap()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
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

            o.AddComponent<RectTransform>();
            RectTransform rect = o.GetComponent<RectTransform>();

            o.transform.localScale = Vector3.one;
            rect.sizeDelta = Vector2.zero;

            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0, 1);

            if (layer.properties.Any(p => p.name == "collidable" && p.value == true))
            {
                o.AddComponent<CompositeCollider2D>();
                o.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
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

            o.AddComponent<RectTransform>();
            RectTransform rect = o.GetComponent<RectTransform>();

            o.transform.localScale = Vector3.one;
            rect.sizeDelta = new Vector2(0, _map.tileheight);

            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0, 1);

            Vector3 pos = o.transform.localPosition;
            pos.y -= _map.tileheight * i;

            o.transform.localPosition = pos;

            List<int> lineData = layer.data.Skip(_map.width * i).Take(_map.width).ToList();

            CreateTile(lineData, layer.properties, o);
        }
    }

    private void CreateTile(List<int> lineData, List<Property> properties, GameObject parent)
    {
        for (int i = 0; i < lineData.Count; i++)
        {
            int id = lineData[i] - 1;

            if(id < 0)
            {
                continue;
            }

            GameObject o = new GameObject("Tile " + i);
            o.transform.SetParent(parent.transform, false);

            o.AddComponent<RectTransform>();
            RectTransform rect = o.GetComponent<RectTransform>();

            o.transform.localScale = Vector3.one;
            rect.sizeDelta = Vector2.zero;

            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
            rect.sizeDelta = new Vector2(_map.tilewidth, _map.tileheight);

            Vector3 pos = o.transform.localPosition;
            pos.x += _map.tilewidth * i;

            o.transform.localPosition = pos;

            o.AddComponent<Image>();

            if(properties.Any(p => p.name == "collidable" && p.value == true))
            {
                o.AddComponent<BoxCollider2D>();

                BoxCollider2D col = o.GetComponent<BoxCollider2D>();

                col.usedByComposite = true;

                col.offset = new Vector2(_map.tilewidth/2, -_map.tileheight/2);
                col.size = new Vector2(_map.tilewidth, _map.tileheight);
            }

            o.GetComponent<Image>().sprite = _tiles[id];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
