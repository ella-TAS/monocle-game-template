#define NUM_COLORS 8

sampler Texture;
float4 Palette[NUM_COLORS];

float4 closestColor(float4 color) {
    float best = 1e20;
    float4 result = Palette[0];

    for (int i = 0; i < NUM_COLORS; i++) {
        float dist = distance(color.rgb, Palette[i].rgb);
        if (best > dist) {
            best = dist;
            result = Palette[i];
        }
    }

    return result;
}

float4 mainPS(float2 coords: TEXCOORD0) : COLOR0 {
    float4 color = tex2D(Texture, coords);
    return closestColor(color);
}

technique Technique1 {
    pass Pass1 {
        PixelShader = compile ps_3_0 mainPS();
    }
}
