Shader "VertexPaint/Fast Vertex Colors RGBA" {
	Properties {
		_AOColor ("AO Color", Color) = (0,0,0,1)
		_AOIntensity ("AO Intensity", Range(0, 1)) = 1.0
	}
	Subshader {
		Tags {
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Fog { Mode Off }
		Pass {
			Lighting Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			half4 _AOColor;
			float _AOIntensity;
			
			struct appdata_color {
			    float4 vertex    : POSITION;
			    fixed4 color     : COLOR;
			};
			struct v2f {
				float4 pos       : SV_POSITION;
				float4 color     : COLOR;
			};
			v2f vert(appdata_color v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.color.rgb = (1-v.color).rgb * _AOColor;
				o.color.a = (1-v.color).a *_AOIntensity ;
				return o;
			}
			float4 frag( v2f i ) : COLOR
			{
				return half4 (i.color);
			}
			ENDCG
		} 
	} 
}