using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Noise
    {
        // TODO: add octaves.
        public static float[,] CreateNoiseMap(NoiseType noiseType, int width, int height, float scale, float angleOffset = 5f, float cellDensity = 2f)
        {
            float[,] noiseMap = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float sampleX = x / scale;
                    float sampleY = y / scale;

                    float noiseValue = noiseType == NoiseType.Perlin ? Mathf.PerlinNoise(sampleX, sampleY) : VoronoiNoise(sampleX, sampleY, angleOffset, cellDensity);
                    noiseMap[x, y] = noiseValue;
                }
            }

            return noiseMap;
        }

        public static float VoronoiNoise(float sampleX, float sampleY, float angleOffset, float cellDensity)
        {
            const float maxDistance = 1.41421356237f; // Square root of 2
            return Mathf.Clamp01(VoronoiNoiseAlgorithm(sampleX, sampleY, angleOffset, cellDensity) / maxDistance);
        }

        // Translated myself from the documentation of Voronoi noise from shader graph
        // See https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Voronoi-Node.html
        private static float VoronoiNoiseAlgorithm(float sampleX, float sampleY, float angleOffset, float cellDensity = 5f)
        {
            Vector2 uv = new Vector2(sampleX, sampleY);

            Vector2 g = new Vector2(Mathf.Floor(uv.x * cellDensity), Mathf.Floor(uv.y * cellDensity));
            Vector2 f = new Vector2(Frac(uv.x * cellDensity), Frac(uv.y * cellDensity));

            Vector3 res = new Vector3(8.0f, 0.0f, 0.0f);

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    Vector2 lattice = new Vector2(x, y);
                    Vector2 offset = RandomVector_LegacySine(lattice + g, angleOffset);
                    float d = Vector2.Distance(lattice + offset, f);

                    if (d < res.x)
                    {
                        res.x = d;
                    }
                }
            }

            return res.x;
        }

        private static Vector2 RandomVector_LegacySine(Vector2 uv, float offset)
        {
            float x = uv.x * 15.27f + uv.y * 99.41f;
            float y = uv.x * 47.63f + uv.y * 89.98f;

            float sx = Mathf.Sin(x) * 46839.32f;
            float sy = Mathf.Sin(y) * 46839.32f;
            float fx = Frac(sx);
            float fy = Frac(sy);

            float ox = Mathf.Sin(fy * offset) * 0.5f + 0.5f;
            float oy = Mathf.Cos(fx * offset) * 0.5f + 0.5f;

            return new Vector2(ox, oy);
        }

        // Returns the fractional part of the input.
        private static float Frac(float val)
        {
            return val - Mathf.Floor(val);
        }
    }
}
