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

            if(id < 0)
            {
                continue;
            }

            GameObject o = new GameObject("Tile " + i);
            o.transform.SetParent(parent.transform, false);

            Vector3 pos = o.transform.position;
            pos.x += i;

            o.transform.position = pos;

            o.AddComponent<SpriteRenderer>();

            if(layer.properties.Any(p => p.name == "collidable" && p.value == true))
            {
                o.AddComponent<BoxCollider2D>();

                BoxCollider2D col = o.GetComponent<BoxCollider2D>();

                col.usedByComposite = true;
                
                col.size = new Vector2(1, 1);
            }

            o.GetComponent<SpriteRenderer>().sprite = _tiles[id];

            o.GetComponent<SpriteRenderer>().sortingOrder = _map.layers.IndexOf(layer);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
