using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHandler : MonoBehaviour{

    private static string GAME_OBJECT_NAME = "map";

    [Header("Noise")]
    public int width;
    public int height;
	public float noiseScale;
	public int octaves;
	[Range(0,1)] public float persistance;
	public float lacunarity;
	public int seed;

    [Header("Mesh")]
	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;
    public Texture2D texture;
    public PhysicMaterial physicMaterial;
    public Vector3 poisiton;
    public Vector3 rotation;
    public Vector3 scale;
  


    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private GameObject map;
    
    private Vector2 noiseOffset;


    private void Start() {
        map = new GameObject(GAME_OBJECT_NAME);
        map.transform.position = poisiton;
        map.transform.rotation = Quaternion.Euler(rotation);
        map.transform.localScale = scale;

        meshFilter = map.AddComponent<MeshFilter>();
        meshRenderer = map.AddComponent<MeshRenderer>();
        meshCollider = map.AddComponent<MeshCollider>();
        noiseOffset = new Vector2(0, 0);

        meshFilter.mesh.MarkDynamic();
    }

    private void Update() {
        UpdateMap(SnowboardController.zAxisChange);
    }

    public float FindHeightAtPosition(Vector2 position) {
        float noise = NoiseGenerator.findNoise(position, noiseScale, octaves, persistance, lacunarity, seed);
        return meshHeightCurve.Evaluate(noise) * meshHeightMultiplier;
    }

    public void UpdateMap(float snowboardVerticalVelocity) {
        noiseOffset.y -= snowboardVerticalVelocity;
        float[,] noiseMap = NoiseGenerator.Create(
            width,
            height, 
            noiseScale,
            octaves,
            persistance,
            lacunarity,
            seed,
            noiseOffset
        );

        Mesh mesh = MeshGenerator.Create(noiseMap, meshHeightMultiplier, meshHeightCurve);
        meshFilter.mesh = mesh;
        meshRenderer.material.mainTexture = texture;
        meshCollider.sharedMesh = meshFilter.mesh;   
        meshCollider.material = physicMaterial;
    }

}

