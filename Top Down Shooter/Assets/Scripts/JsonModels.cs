using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    public List<Layer> layers;
    public int height;
    public int width;
    public int tileheight;
    public int tilewidth;

}

[System.Serializable]
public class Layer
{
    public List<int> data;
    public string name;
    public List<Property> properties;
}

[System.Serializable]
public class Property
{
    public string name;
    public bool value;
}
