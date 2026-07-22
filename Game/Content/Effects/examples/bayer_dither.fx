#define NUM_COLORS 32
#define SPREAD 0.25
#define NORMALIZE 0.5

sampler Texture;
float3 Palette[NUM_COLORS];
float Width;
float Height;

int Bayer4[] = {
    0, 8, 2, 10,
    12, 4, 14, 6,
    3, 11, 1, 9,
    15, 7, 13, 5
};

int Bayer8[] = {
     0, 32,  8, 40,  2, 34, 10, 42,
    48, 16, 56, 24, 50, 18, 58, 26,
    12, 44,  4, 36, 14, 46,  6, 38,
    60, 28, 52, 20, 62, 30, 54, 22,
     3, 35, 11, 43,  1, 33,  9, 41,
    51, 19, 59, 27, 49, 17, 57, 25,
    15, 47,  7, 39, 13, 45,  5, 37,
    63, 31, 55, 23, 61, 29, 53, 21
};

float4 closestColor(float3 color) {
    float best = 1e20;
    float4 result = {0, 0, 0, 1};

    for (int i = 0; i < NUM_COLORS; i++) {
        float dist = distance(color, Palette[i]);
        if (best > dist) {
            best = dist;
            result.rgb = Palette[i];
        }
    }

    return result;
}

float4 mainPS(float2 coords: TEXCOORD0) : COLOR0 {
    float3 color = tex2D(Texture, coords).rgb;
    int2 screenPos = int2(coords.x * Width, coords.y * Height);

    float threshold = Bayer4[(screenPos.x % 4) + (screenPos.y % 4) * 4] / 16.0;
    float3 ditherColor = color.rgb + SPREAD * (threshold - NORMALIZE);

    return closestColor(ditherColor);
}

technique Technique1 {
    pass Pass1 {
        PixelShader = compile ps_3_0 mainPS();
    }
}
