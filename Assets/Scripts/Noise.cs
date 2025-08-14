using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Noise
    {
        // Absolutely primitive rn, not a priority.
        public static float[,] PerlinNoiseMap(int width, int height, float scale)
        {
            float[,] noiseMap = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float sampleX = x / scale;
                    float sampleY = y / scale;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseMap[x, y] = perlinValue;
                }
            }

            return noiseMap;
        }

        public static float[,] VoronoiNoiseMap(int width, int height, float scale, float angleOffset, float cellDensity)
        {
            float[,] noiseMap = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float sampleX = x / scale;
                    float sampleY = y / scale;

                    float voronoiValue = SampleVoronoiNoise01(sampleX, sampleY, angleOffset, cellDensity);
                    noiseMap[x, y] = voronoiValue;
                }
            }

            return noiseMap;
        }

        private static float SampleVoronoiNoise01(float sampleX, float sampleY, float angleOffset, float cellDensity)
        {
            const float maxDistance = 1.41421356237f; // Square root of 2
            return Mathf.Clamp01(VoronoiNoise(sampleX, sampleY, angleOffset, cellDensity) / maxDistance);
        }

        private static float VoronoiNoise(float sampleX, float sampleY, float angleOffset, float cellDensity = 5f)
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

        private static float Frac(float val)
        {
            return val - Mathf.Floor(val);
        }

        /*public static float[,] VoronoiNoise(int width, int height, float scale)
        {
            float[,] noiseMap = new float[width, height];
            Vector2[,] pixelUVs = new Vector2[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float u = (float)x / (width - 1);
                    float v = (float)y / (height - 1);

                    pixelUVs[x, y] = new Vector2(Mathf.Abs(Fraction(u) - 0.5f), Mathf.Abs(Fraction(v) - 0.5f)) * 4;
                }
            }
            return noiseMap;
        }

        private static float Fraction(float num)
        {
            return num - Mathf.Floor(num);
        }*/
    }
}
