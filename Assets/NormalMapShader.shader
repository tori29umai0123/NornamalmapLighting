Shader "Custom/NormalMapShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _NormalMap;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;

            // ノーマルマップを取得し、デコード
            half3 normalMap = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
            o.Normal = UnityObjectToWorldNormal(normalMap);
        }
        ENDCG
    }

    FallBack "Diffuse"
}