using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public static class Noise
    {
        // Absolutely primitive rn, not a priority.
        public static float[,] PerlinNoise(int width, int height, float scale)
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
