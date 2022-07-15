using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {

	public static Mesh Create(float[,] noiseMap, float heightMultiplier, AnimationCurve heightCurve) {
		int width = noiseMap.GetLength (0);
		int height = noiseMap.GetLength (1);

        Vector3[] vertices = new Vector3[width * height];
		Vector2[] uvs = new Vector2[width * height];
		int[] triangles = new int[(width-1)*(height-1)*6];

		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

	    int vertexIndex = 0;
        int triangleIndex = 0;

		for (int y = 0; y < height; y++) {

			for (int x = 0; x < width; x++) {
                float vertexHeight = heightCurve.Evaluate(noiseMap[x, y]) * heightMultiplier;
				vertices[vertexIndex] = new Vector3(topLeftX + x, vertexHeight, topLeftZ - y);
				uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                // Check if not in right or bottom edge
				if (x < width - 1 && y < height - 1) {
                    // First triangle
                    triangles[triangleIndex++] = vertexIndex;
                    triangles[triangleIndex++] = vertexIndex + width + 1;
                    triangles[triangleIndex++] = vertexIndex + width;
                    // Second triangle
                    triangles[triangleIndex++] = vertexIndex + width + 1;
                    triangles[triangleIndex++] = vertexIndex;
                    triangles[triangleIndex++] = vertexIndex + 1;
				}
				vertexIndex++;
			}
		}

        Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		return mesh;

	}
}
