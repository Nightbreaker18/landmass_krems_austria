using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts.Testing
{
    public class PlaneGenerator : MonoBehaviour
    {
        public enum NoiseType
        {
            Perlin,
            Voronoi
        }

        public NoiseType noiseType;
        public Material noise_test;
        [Min(1)] public int width = 1, height = 1;
        [Min(0.01f)] public float noiseScale;
        public float angleOffset;
        public float cellDensity;
        public bool modifyHeight = false; // Implement custom plane mesh

        private void Start()
        {
            GameObject plane = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Plane), null);
            plane.transform.localScale = new Vector3(width, 1, height);
            plane.GetComponent<MeshRenderer>().material = noise_test;

            if (noiseType == NoiseType.Perlin)
            {
                float[,] noiseMap = Noise.PerlinNoiseMap(width, height, noiseScale);
                noise_test.mainTexture = SetTexture(noiseMap);
            }

            else if (noiseType == NoiseType.Voronoi)
            {
                float[,] noiseMap = Noise.VoronoiNoiseMap(width, height, noiseScale, angleOffset, cellDensity);
                noise_test.mainTexture = SetTexture(noiseMap);
            }
        }

        public Texture2D SetTexture(float[,] noiseMap)
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] colors = new Color[width * height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    colors[j * width + i] = Color.Lerp(Color.black, Color.white, noiseMap[i, j]);
                }
            }

            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels(colors);
            texture.Apply();

            return texture;
        }
    }
}
