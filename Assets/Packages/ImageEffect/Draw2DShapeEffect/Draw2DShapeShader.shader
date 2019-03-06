Shader "Hidden/Draw2DShapeEffect"
{
    Properties
    {
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

            #pragma target 5.0
            #pragma vertex vert_img
            #pragma fragment frag

            struct Shape
            {
                int    type;
                float4 pos;
                float4 param;
                float4 color;
            };

            StructuredBuffer<Shape> _ShapeBuffer;

            sampler2D _MainTex;

            float4 alphaBlend(float4 src, float4 dest)
            {
                return float4(dest.rgb * (1 - src.a) + src.rgb * src.a, 1);
            }

            void drawCircle(float2 inputPos, float2 centerPos, float radius, fixed4 color, inout fixed4 dest)
            {
                dest = length(inputPos - centerPos) < radius ? alphaBlend(color, dest) : dest;
            }

            void drawRing(float2 inputPos, float2 centerPos, float innerRad, float outerRad, fixed4 color, inout fixed4 dest)
            {
                float l = length(inputPos - centerPos);

                dest = innerRad < l && l < outerRad ? alphaBlend(color, dest) : dest;
            }

            void drawSqare(float2 inputPos, float2 centerPos, float size, fixed4 color, inout fixed4 dest)
            {
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
                float4 dest   = tex2D(_MainTex, input.uv);;
                float  aspect = _ScreenParams.x / _ScreenParams.y;

                float2 inputPos = float2(input.uv.x * aspect, input.uv.y);

                Shape shape;
                int shapeLength = (int)_ShapeBuffer.Length;

                for (int i = 0; i < shapeLength; i++)
                {
                    shape     = _ShapeBuffer[i];
                    shape.pos = float4(shape.pos.x * aspect, shape.pos.y, shape.pos.z * aspect, shape.pos.w);

                    switch(shape.type)
                    {
                        case 0:
                            drawCircle(inputPos, shape.pos, shape.param.x, shape.color, dest);
                            break;
                        case 1:
                            drawRing(inputPos, shape.pos, shape.param.x, shape.param.y, shape.color, dest);
                            break;
                        case 2:
                            drawSqare(inputPos, shape.pos, shape.param.x, shape.color, dest);
                            break;
                        case 3:
                            drawRect(inputPos, shape.pos, shape.color, dest);
                            break;
                        default:
                            // Nothing to do.
                            break;
                    }
                }

                return dest;
            }

            ENDCG
        }
    }
}