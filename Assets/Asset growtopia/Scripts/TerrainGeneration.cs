 using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public AudioSource hitTile;
    public int worldSize = 100;
    public float noiseFreq = 0.05f;
    public float heightMultiplier = 40f;
    public float seed;
    public Texture2D noiseTexture;

    public IBlock blockHandler;


    public Sprite grass;
    public Sprite dirt;
    public Sprite stone;

    public GameObject grassDrop;
    public GameObject dirtDrop;
    public GameObject stoneDrop;

    void Start()
    {
        seed = Random.Range(-10000, 10000);
        blockHandler = new Block();  
        GenerateNoiseTexture();
        GenerateTerrain();
    }


    public void GenerateTerrain()
    {
        Block block = new Block();

        for (int x = 0; x < worldSize; x++)
        {
            int height =
            Mathf.RoundToInt(
            Mathf.PerlinNoise((x + seed) , seed)
            * heightMultiplier);

            for (int y = 0; y < height; y++)
            {
                if (y == height - 1)
                {
                    block.Place(grass, new Vector2(x,y), "Grass", grassDrop);
                }
                else if (y >= height - 4)
                {
                    block.Place(dirt, new Vector2(x,y), "Dirt", dirtDrop);
                }
                else
                {
                    block.Place(stone, new Vector2(x,y), "Stone", stoneDrop);
                }
            }
        }
    }

    void GenerateNoiseTexture()
    {
        noiseTexture = new Texture2D(worldSize, worldSize);
        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) , (y + seed));
                noiseTexture.SetPixel(x, y, new Color(v, v, v));
            }
        }
        noiseTexture.Apply();
    }

    public void RemoveTile(Vector2 mouseWorldPos)
    {
        blockHandler.Break(mouseWorldPos);
       
        
    }

    public bool PlaceTile(string itemName, Vector2 pos)
    {
        Block block = new Block();
        hitTile.Play(); 

        if(itemName == "Stone")
            return block.Place(stone,pos,"Stone",stoneDrop);
             
        if(itemName == "Dirt")
            return block.Place(dirt,pos,"Dirt",dirtDrop);

        if(itemName == "Grass")
            return block.Place(grass,pos,"Grass",grassDrop);

        return false;
    }
}
