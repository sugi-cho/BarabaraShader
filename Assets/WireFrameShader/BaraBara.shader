Shader "Custom/Barabara" {
	Properties {
		_T("init time", Float) = 0
	}
 
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "Random.cginc"
		#include "Transform.cginc"
		
		half _T;
		
		struct v2f {
			float4 pos : SV_POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			float3 bary : TEXCOORD1;
		};
 
		v2f vert (appdata_full v)
		{
			half3 center = v.tangent.xyz;
			half t = max(0,_Time.y-_T-1-pow(rand(center+_T),2));
			v.vertex.xyz = rotate(v.vertex.xyz, normalize(center), t*10, center);
			
			v.vertex.y -= 4.9*t*t;
			v.vertex.xyz += 5*v.tangent.xyz*t;
			
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.bary = v.color.rgb;
			o.color = v.normal.y/2+0.5;
			o.texcoord = v.texcoord;
			return o;
		}
		
		v2f vert2(appdata_full v){
			v.normal.xyz = -v.normal.xyz;
			return vert(v);
		}
			
		fixed4 frag (v2f i) : COLOR
		{
			float3 d = fwidth(i.bary.xyz);
			float3 a3 = smoothstep(float3(0.0,0.0,0.0), 1.0 * d, i.bary);
			half4 c = i.color;
			return c * saturate(min(min(a3.x, a3.y), a3.z));
		}
	ENDCG
	
	SubShader {
		Cull Back
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0
			#pragma glsl
			ENDCG 
		}
		Cull Front
		Pass {
			CGPROGRAM
			#pragma vertex vert2
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0
			#pragma glsl
			ENDCG 
		}
	}
}