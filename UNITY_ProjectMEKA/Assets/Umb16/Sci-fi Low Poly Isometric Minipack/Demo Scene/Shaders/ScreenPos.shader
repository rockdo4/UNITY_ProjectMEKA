Shader "Custom/ScreenPosPoolWater"
{
    Properties{
      _MainTex("Texture", 2D) = "white" {}
    }
        SubShader{
          Tags { "RenderType" = "Opaque" }
          CGPROGRAM
          #pragma surface surf Lambert
          struct Input {
              float4 screenPos;
          };
          sampler2D _MainTex;
          void surf(Input IN, inout SurfaceOutput o) {
              float ratio = _ScreenParams.x / _ScreenParams.y;
              float2 screenUV = (IN.screenPos.xy / IN.screenPos.w);
              screenUV *= float2(ratio, 1);
              o.Emission = tex2D(_MainTex, screenUV).rgb*2;
          }
          ENDCG
    }
        Fallback "Diffuse"
}
