Shader "Hidden/Atmosphere"
{
    Properties
    {
        _MainTex("Screen Blended", 2D) = "" {}
        _Overlay("Noise", 2D) = "grey" {}
    }
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always

	CGINCLUDE
			
	#include "UnityCG.cginc"

    sampler2D _Overlay;
    sampler2D _MainTex;

    half _Intensity;
    half4 _MainTex_TexelSize;
    half4 _UV_Transform = half4(1, 0, 0, 1);
    float _CamPosX;

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

    struct v2f 
    {
        float4 pos : SV_POSITION;
        float2 uv[2] : TEXCOORD0;
    };

    v2f vert(appdata_img v) {
        v2f o;
        o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

        o.uv[0] = float2(
            dot(v.texcoord.xy, _UV_Transform.xy),
            dot(v.texcoord.xy, _UV_Transform.zw)
        );

        #if UNITY_UV_STARTS_AT_TOP
        if (_MainTex_TexelSize.y<0.0)
            o.uv[0].y = 1.0 - o.uv[0].y;
        #endif

        o.uv[1] = v.texcoord.xy;
        return o;
    }

    float noise(float2 p)
    {
        return tex2D(_Overlay, p).x;
    }

    float fnoise(float2 uv, float4 sc) {
        float f = sc.x*noise(uv); uv = 2.*uv + .11532185;
        f += sc.y*noise(uv); uv = 2.*uv + .23548563;
        f += sc.z*noise(uv); uv = 2.*uv + .12589452;
        f += sc.w*noise(uv); uv = 2.*uv + .26489542;
        return f;
    }


    float terrain(float x) {
        float w = 0.;
        float a = 1.;
        x *= 20.;
        w += sin(x*.3521)*4.;
        for (int i = 0; i<5; i++) {
            x *= 1.53562;
            x += 7.56248;
            w += sin(x)*a;
            a *= .5;
        }
        return .2 + w*.015;
    }


    float scene(float2 p) {
        float t = terrain(p.x);
        float s = step(0., p.y + t);
        float tx = floor(p.x / .2)*.2 + .1;
        return s;
    }

    float aascene(float2 p) {
        float2 pix = float2(0., max(.25, 6. + _CamPosX) / _ScreenParams.x);
        float aa = scene(p);
        aa += scene(p + pix.xy);
        aa += scene(p + pix.yy);
        aa += scene(p + pix.yx);
        return aa*.25;
    }

    float4 frag(v2f i) : SV_Target
    {
        float2 uv = i.uv[0] - 0.5;
        uv.x *= _ScreenParams.x / _ScreenParams.y;
        float v = 0.;
        float light = 0.;
        float xPos = -_CamPosX*0.01;
        float time = -_Time.y * 0.1;
        float2 c = float2(-xPos,0.);
        float2 p;
        float sc = clamp(xPos*xPos*.5,.05,.15);
        uv.y -= .25;
        uv.x -= .2;

        for (int j = 0; j < 25; j++) 
        {
            p = uv*sc;
            light = pow(max(0.,1. - length(p)*2.),15.);
            light = .02 + light*.8;
            v += scene(p + c)*pow(float(j + 1) / 30.,2.) * light;
            sc += .006;
        }

        float cloud = fnoise((uv - float2(xPos - time,0.))*float2(.03,.15),float4(.8,.6,.3,.1))*max(0.,1. - uv.y*3.);
        float tx = uv.x - xPos * 3.0;
        float backTerrain = .5 + step(0., uv.y - fnoise(float2(tx,tx)*.015,
            float4(1.,.5,.3,.1))*(.23*(1. + sin(tx*3.2342)*.25)) + .5);
        float scene = (backTerrain + cloud*.8);

        v *= .025;
        float color = min(1.,.05 + scene*0.01);
        color = sqrt(color)*2.05 - .5;

        float4 fragColor = float4(color, color * 0.7, color * 0.2, color);

        //return lerp(tex2D(_MainTex, i.uv[1]), fragColor, fragColor.a * _Intensity);
        return tex2D(_MainTex, i.uv[1]) * 1.5 + fragColor * _Intensity;
    }
    
    ENDCG

    Subshader 
    {
        ZTest Always Cull Off ZWrite Off
        ColorMask RGB

        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }

    Fallback off
}
