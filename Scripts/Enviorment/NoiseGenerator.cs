using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public static class NoiseGenerator {

	private static int RANDOM_NUMBER_RANGE = 10000;
	private static float MINIMUM_SCALE = 0.0001f;

	public static float[,] Create(int width, int height, float scale, int octaves, float persistance, float lacunarity, int seed, Vector2 offset) {
		float[,] noiseMap = new float[width, height];

		System.Random random = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		for (int index = 0; index < octaves; index++) {
			octaveOffsets[index] = new Vector2(
				random.Next(-RANDOM_NUMBER_RANGE, RANDOM_NUMBER_RANGE) + offset.x,
				random.Next(-RANDOM_NUMBER_RANGE, RANDOM_NUMBER_RANGE) + offset.y
			);
		}

		//
		if (scale <= 0) {
			scale = MINIMUM_SCALE;
		}

		
		float widthCenter = width / 2f;
		float heightCenter = height / 2f;

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		for (int y = 0; y < height; y++) {

			for (int x = 0; x < width; x++) {
		
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x - widthCenter) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - heightCenter) / scale * frequency + octaveOffsets[i].y;

					// Between -1 and 1
					float perlinNoiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinNoiseValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				maxNoiseHeight = Mathf.Max(maxNoiseHeight, noiseHeight);
				minNoiseHeight = Mathf.Min(minNoiseHeight, noiseHeight);

				noiseMap[x, y] = noiseHeight;
			}
		}

		// Normalize Values
		for (int y = 0; y < height; y++) {

			for (int x = 0; x < width; x++) {
				noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
		}

		return noiseMap;
	}

	public static float findNoise(Vector2 position, float scale, int octaves, float persistance, float lacunarity, int seed) {

		System.Random random = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		for (int index = 0; index < octaves; index++) {
			octaveOffsets[index] = new Vector2(
				random.Next(-RANDOM_NUMBER_RANGE, RANDOM_NUMBER_RANGE) + position.x,
				random.Next(-RANDOM_NUMBER_RANGE, RANDOM_NUMBER_RANGE) + position.y
			);
		}

		//
		if (scale <= 0) {
			scale = MINIMUM_SCALE;
		}

		float amplitude = 1;
		float frequency = 1;
		float noiseHeight = 0;

		for (int i = 0; i < octaves; i++) {
			float sampleX = position.x / scale * frequency + octaveOffsets[i].x;
			float sampleY = position.y / scale * frequency + octaveOffsets[i].y;

			// Between -1 and 1
			float perlinNoiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
			noiseHeight += perlinNoiseValue * amplitude;

			amplitude *= persistance;
			frequency *= lacunarity;
		}

		return noiseHeight;
	}
}
