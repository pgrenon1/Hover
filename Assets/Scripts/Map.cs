using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    #region
    private static Map _instance;
    public static Map Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public bool autoUpdate;
    public int seed;
    public int mapWidth;
    public int mapHeight;
    public int octaves;
    public float scale;
    public float persistance;
    public float lacunarity;
    public Vector2 offset;
    public List<GameObject> islandPrefabs;
    public AnimationCurve difficultyCurve;
    public List<GameObject> islands;
    public GameObject passengerPrefab;

    public float threshold = .5f;
    public float sparsity;


    public void Destroy()
    {
        for (int i = 0; i < islands.Count; i++)
        {
            DestroyImmediate(islands[i].gameObject);
        }

        islands = new List<GameObject>();
    }

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        Destroy();

        islands = new List<GameObject>();
        //var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //plane.transform.localScale = new Vector3(mapWidth, 1, mapHeight);

        System.Random prng = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                float perlinValue = 1;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > threshold)
                {
                    Vector3 position;
                    if (GameManager.Instance.isFlat)
                        position = new Vector3(x - mapWidth / 2, 0, y - mapHeight / 2);
                    else
                        position = new Vector3(x - mapWidth / 2, Random.Range(-5, 5), y - mapHeight / 2);

                    if (!islands.Exists(c => Vector3.Distance(c.transform.position, position) < 5))
                    {
                        var prefab = islandPrefabs[Random.Range(0, islandPrefabs.Count)];
                        var newIsland = Instantiate(prefab, position, Quaternion.identity, transform);
                        newIsland.transform.position = position;
                        newIsland.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
                        var scaleUp = Random.Range(13f, 20f);
                        newIsland.transform.localScale = Vector3.one * scaleUp * 2;
                        var difficulty = difficultyCurve.Evaluate(scaleUp);
                        var numPassengers = (int)scaleUp;
                        var target = newIsland.GetComponent<Target>();
                        target.Multiplier = (float)System.Math.Round(difficulty, 1);
                        target.Passengers = numPassengers * 2;
                        target.passengerPrefab = passengerPrefab;
                        newIsland.transform.localScale.Scale(new Vector3(1, Random.Range(0.8f, 1.2f), 1));
                        islands.Add(newIsland);
                    }
                }
            }
        }

        for (int i = 0; i < islands.Count; i++)
        {
            islands[i].transform.Translate(Vector3.MoveTowards(transform.position, islands[i].transform.position * 10, sparsity));
        }
    }

}
