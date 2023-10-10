using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class NoiseGen
{
    // A method for generating noise maps. 
    // Accepts an array of waves and superimposes the output maps of each to create more interesting/natural looking noisemaps.
    public static float[,] Generate (int width, int height, float scale, Wave[] waves, Vector2 offset)
    {
        float[,] noiseMap = new float[width, height];

        Parallel.For(0, width, x =>
        {
            float xPos = (float)x * scale + offset.y;

            for (int y = 0; y < height; ++y)
            {
                float yPos = (float)y * scale + offset.y;

                // calculate noise value at [x,y] for each wave
                // superimpose noise values by averaging each
                float normalizationFactor = 0.0f;
                Unity.Mathematics.float2 pos = new Unity.Mathematics.float2();
                foreach (Wave w in waves)
                {
                    pos.x = xPos * w.frequency + w.seed;
                    pos.y = yPos * w.frequency + w.seed;
                    float2 noiseValue = Unity.Mathematics.noise.cellular(pos);
                    float noiseValueSummed = (noiseValue.x + noiseValue.y);
                    noiseMap[x, y] += w.amplitude * noiseValueSummed;
                    normalizationFactor += w.amplitude;
                }

                noiseMap[x, y] /= normalizationFactor;
            }
        });

        return noiseMap;
    }
}