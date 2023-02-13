Shader "Hidden/FigureShader"
{
    Properties
    {
        [HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull   Off
        ZWrite Off
        ZTest  Always

        Pass
        {
            CGPROGRAM
            
            #include "UnityCG.cginc"

            #pragma target   5.0
            #pragma vertex   vert_img
            #pragma fragment frag

            struct FigureData
            {
                // NOTE:
                // parameter.xy = position.
                // parameter.zw = each parameters.

                int    type;
                float4 parameter;
                float4 color;
            };

            StructuredBuffer<FigureData> _FigureDataBuffer;

            sampler2D _MainTex;

            float4 alphaBlend(float4 src, float4 dest)
            {
                return float4(dest.rgb * (1 - src.a) + src.rgb * src.a, 1);
            }

            void drawCircle(float2 inputPos, float2 centerPos, float radius, fixed4 color, inout fixed4 dest)
            {
                float aspect = _ScreenParams.x / _ScreenParams.y;

                inputPos.x  = inputPos.x  * aspect;
                centerPos.x = centerPos.x * aspect;

                dest = length(inputPos - centerPos) < radius ? alphaBlend(color, dest) : dest;
            }

            void drawRing(float2 inputPos, float2 centerPos, float innerRad, float outerRad, fixed4 color, inout fixed4 dest)
            {
                float aspect = _ScreenParams.x / _ScreenParams.y;

                inputPos.x  = inputPos.x  * aspect;
                centerPos.x = centerPos.x * aspect;

                float l = length(inputPos - centerPos);

                dest = innerRad < l && l < outerRad ? alphaBlend(color, dest) : dest;
            }

            void drawSqare(float2 inputPos, float2 centerPos, float size, fixed4 color, inout fixed4 dest)
            {
                float aspect = _ScreenParams.x / _ScreenParams.y;

                inputPos.x  = inputPos.x  * aspect;
                centerPos.x = centerPos.x * aspect;

                float2 q = (inputPos - centerPos) / size;

                dest = abs(q.x) < 1.0 && abs(q.y) < 1.0 ? alphaBlend(color, dest) : dest;
            }

            void drawRect(float2 inputPos, float4 minmaxPos, fixed4 color, inout fixed4 dest)
            {
                dest = minmaxPos.x < inputPos.x
                    && minmaxPos.y < inputPos.y
                    && inputPos.x < minmaxPos.z
                    && inputPos.y < minmaxPos.w ? alphaBlend(color, dest) : dest;
            }

            float4 frag(v2f_img input) : SV_Target
            {
                float4 color = tex2D(_MainTex, input.uv);

                int figureDataBufferLength = (int)_FigureDataBuffer.Length;

                for (int i = 0; i < figureDataBufferLength; i++)
                {
                    FigureData figureData = _FigureDataBuffer[i];

                    switch(figureData.type)
                    {
                        case 0:
                            drawCircle(input.uv,
                                       figureData.parameter.xy,
                                       figureData.parameter.z,
                                       figureData.color,
                                       color);
                            break;
                        case 1:
                            drawRing(input.uv,
                                     figureData.parameter.xy,
                                     figureData.parameter.z,
                                     figureData.parameter.w,
                                     figureData.color,
                                     color);
                            break;
                        case 2:
                            drawSqare(input.uv,
                                      figureData.parameter.xy,
                                      figureData.parameter.z,
                                      figureData.color,
                                      color);
                            break;
                        case 3:
                            drawRect(input.uv,
                                     figureData.parameter,
                                     figureData.color,
                                     color);
                            break;
                        default:
                            // Nothing to do.
                            break;
                    }
                }

                return color;
            }

            ENDCG
        }
    }
}