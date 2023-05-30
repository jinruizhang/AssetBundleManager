// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/UI-GrayToColor"
{
    Properties
    {
		// 主图
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    	//混合图
		_BlendTex ("Texture", 2D) = "white" {}
		// 插值
        _Lerp("Color Lerp",Range(0,1.0)) = 0
    }
    SubShader
    {
		// 标签
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent"}

    	Stencil 
    	{
    		Ref 1              //要比较的缓冲值
            Comp equal        //比较方法：相等
        }
    	
       ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		LOD 100

		pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata{
			
				float4 vertex: POSITION;
				float2 uv:TEXCOORD0;
				fixed4 color:COLOR;

			};

			struct v2f{
			
				float2 uv:TEXCOORD0;
				float4 vertex:SV_POSITION;
				fixed4 color:COLOR;
			};

			sampler2D _MainTex;
			sampler2D _BlendTex;
			float4 _MainTex_ST;
			float _Lerp;

			v2f vert(appdata v){
			
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv,_MainTex);
				o.color = v.color;

				return o;
			}


			fixed4 frag(v2f i):SV_Target{

				// 主图颜色
				fixed4 renderTex = tex2D(_MainTex,i.uv);
				//blendTex为混合图rgba 四维向量
				fixed4 blendTex = tex2D(_BlendTex,i.uv);

				// 灰度 上面的参考灰度 可以比对效果
				//fixed gray = 0.2125*renderTex.r + 0.7154*renderTex.g + 0.0721*renderTex.b;
				fixed gray = 0.30 * renderTex.r + 0.59 * renderTex.g + 0.11 * renderTex.b;
				// 灰度颜色
				fixed3 grayLerp = fixed3(gray,gray,gray);

				fixed3 alphaLerp = renderTex.rgb;
				 
				// 插值灰度变彩色
				fixed3 finalColor = lerp(grayLerp,renderTex.rgb,_Lerp);
				
				fixed3 tempBlendColor = lerp(blendTex.rgb,alphaLerp,_Lerp);
				fixed4 finalBlendColor = fixed4(tempBlendColor.r, tempBlendColor.g, tempBlendColor.b, blendTex.a);

				finalColor = finalColor * (1 - finalBlendColor.a) + finalBlendColor * (finalBlendColor.a);

				// 最终颜色
				return fixed4(finalColor, renderTex.a);
				
			}


			ENDCG
		
		}

	}
    FallBack "Diffuse"
}
