texture ScreenTexture;

sampler2D ScreenSampler = sampler_state
{
    Texture = <ScreenTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = None;
    AddressU = Clamp;
    AddressV = Clamp;
};

float2 iResolution = float2(960, 540);
float iTime;
float Strength = 1.0;

struct PS_INPUT
{
    float2 TexCoord : TEXCOORD0;
};

float4 PS_Distort(PS_INPUT input) : COLOR0
{
    float2 uv = input.TexCoord;

    // simple horizontal scroll distortion
    float2 sine = float2(0.5 * sin(uv.y * 12 + iTime * 0.5), 0);
    float2 sine2 = float2(1.0 * sin(uv.y * 5 + iTime * 0.2), 0);
    float2 sine3 = float2(uv.y * 0.8 * sin(uv.y * 3 + iTime * 0.35), 0);

    float2 center = float2(0.5, 0.5);
    float2 p = uv - center;
    float dist = length(float2(p.x, p.y * iResolution.y / iResolution.x));
    float angle = exp2(-500 * dist * dist) * 3.1415926 * 0.5 * Strength;
    float s = sin(angle);
    float c = cos(angle);
    float2 rotated = float2(p.x * c - p.y * s, p.x * s + p.y * c);

    float2 offset = float2(0.0, 0.0);
    offset += sine;
    offset += sine2;
    offset += sine3;
    offset.x *= 2;

    // 49-neighbor average blur
    float2 pos = center + rotated + offset * Strength * 0.005;
    float4 color = tex2D(ScreenSampler, pos);
    float4 colorBlur = float4(0, 0, 0, 0);
    float2 texel = 1 / iResolution;

    // Gaussian-ish 7-tap weights (1D)
    static const float w0 = 0.0701593269590261; // offset 3
    static const float w1 = 0.131074878967366;  // offset 2
    static const float w2 = 0.190712823569637;  // offset 1
    static const float w3 = 0.216105941008356;  // offset 0

    for(int i = -3; i <= 3; i++) {
        colorBlur += tex2D(ScreenSampler, pos + float2(i * texel.x, 0)) * (
            abs(i) == 3 ? w0 :
            abs(i) == 2 ? w1 :
            abs(i) == 1 ? w2 :
            w3
        );
    }
    colorBlur /= 1.15;

    // vignette effect
    color = lerp(color, colorBlur, Strength);
    float centerDist = length(uv - center);
    if (centerDist > 0.4) {
        color /= lerp(1, 3 + Strength, centerDist - 0.4);
    }
    color.a = 1;

    return color;
}


technique Distort
{
    pass P0
    {
        PixelShader = compile ps_3_0 PS_Distort();
    }
}