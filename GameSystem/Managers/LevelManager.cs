using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Sprite defaultTile;

    [SerializeField]
    private Transform map;

    [SerializeField]
    private Texture2D[] mapData;

    [SerializeField]
    private MapElement[] mapElements;

    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();

    [SerializeField]
    private SpriteAtlas waterAtlas;

    private Vector3 worldStartPosition
    {
        get { return Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); }
    }
    // Start is called before the first frame update
    void Start()
    {        
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        int height = mapData[0].height;
        int width = mapData[0].width;

        for (int i = 0; i < mapData.Length; i++)
        {
            for (int x = 0; x < mapData[i].width; x++)
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y);

                    MapElement newElement = Array.Find(mapElements, e => e.MyColor == c);

                    if(newElement != null)
                    {
                        float xPos = worldStartPosition.x + (defaultTile.bounds.size.x * x);
                        float yPos = worldStartPosition.y + (defaultTile.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.MyElementPrefab);
                        go.transform.position = new Vector2(xPos,yPos);

                        if(newElement.MyTileTag == "Water")
                        {
                            waterTiles.Add(new Point(x, y), go);
                        }

                        if(newElement.MyTileTag == "Tree")
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height*2 - y*2;
                        }
                        go.transform.parent = map;
                    }
                }
            }
        }
        CheckWater();
    }

    public void CheckWater()
    {
        foreach (KeyValuePair<Point, GameObject> tile in waterTiles)
        {
            string composition = TileCheck(tile.Key);

            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w0");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w1");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w2");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w3");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w4");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w5");
            }
            if (composition[1] == 'W' && composition[4] == 'W' && composition[3] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w6");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w7");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w8");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w9");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w10");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w11");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w12");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w13");
            }
            if (composition[3] == 'W' && composition[5] == 'E' && composition[6] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w14");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[2] == 'E' && composition[4] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w15");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w16");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w17");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 15)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w19");
                }
            }
            if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 10)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("w20");
                }

            }

        }
    }

    private string TileCheck(Point currentPoint)
    {
        string composition = string.Empty;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x != 0 || y != 0)
                {
                    if(waterTiles.ContainsKey(new Point(currentPoint.MyX + x, currentPoint.MyY + y)))
                    {
                        composition += "W";
                    }
                    else
                    {
                        composition += "E";
                    }
                }
            }
        }
        return composition;
    }
}
[Serializable]
public class MapElement
{
    [SerializeField]
    private string tileTag;
    [SerializeField]
    private Color color;
    [SerializeField]
    private GameObject elementPrefab;

    public GameObject MyElementPrefab { get => elementPrefab; }
    public Color MyColor { get => color; }
    public string MyTileTag { get => tileTag;}
}

public struct Point
{
    public int MyX { get; set; }
    public int MyY { get; set; }
    public Point(int x,int y)
    {
        this.MyX = x;
        this.MyY = y;
    }
}
