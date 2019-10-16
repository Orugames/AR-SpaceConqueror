﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColourSettings : ScriptableObject {

    public Gradient gradient;
    public Material planetMaterial;

    public BiomeColourSettings biomeColourSettings;

    [System.SerializableAttribute]
    public class BiomeColourSettings
    {
        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrengh;
        [Range(0, 1)]
        public float blendAmount;

        [System.SerializableAttribute]

        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range(0,1)]
            public float startHeight;
            [Range(0, 1)]
            public float tintPercent;
        }
    }
} 
