using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private TextAsset _mapJson;

    [SerializeField]
    private Sprite[] _tiles;

    private Map _map;

    // Start is called before the first frame update
    void Start()
    {
        _map = JsonUtility.FromJson<Map>(_mapJson.ToString());
        //Debug.Log("Height: " + _map.height);
        //Debug.Log("width: " + _map.width);

        //Debug.Log("tileheight: " + _map.tileheight);
        //Debug.Log("tilewidth: " + _map.tilewidth);
        CreateLayer();
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

            CreateLine(layer.data, o);
        }
    }

    private void CreateLine(List<int> data, GameObject parent)
    {
        for(int i = 1; i <= _map.height; i++)
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

            //Vector3 pos = Vector3.zero;
            //pos.y = _map.tileheight * (i - 1);

            //o.transform.localPosition = pos;
            //o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y + _map.tileheight, o.transform.position.z);

            //List<int> lineData = data.TakeWhile(
            //    tileID => 
            //    (data.IndexOf(tileID) < _map.width * i) && 
            //    (data.IndexOf(tileID) > _map.width * (i - 1)) 
            //    ).ToList();

            List<int> lineData = data.Skip(_map.width * (i - 1)).Take(_map.width).ToList();

            //foreach (int line in lineData)
            //{
            //    Debug.Log(line);
            //}
            CreateTile(lineData, o);
        }
    }

    private void CreateTile(List<int> lineData, GameObject parent)
    {
        for(int i = 0; i < lineData.Count; i++)
        {
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
