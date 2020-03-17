Shader "QFX/MFX/Materialization_Local"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_AlbedoColor("Albedo Color", Color) = (0,0,0,0)
		_MetallicTexture("Metallic Texture", 2D) = "white" {}
		_Metallic("Metallic", Range(0 , 1)) = 0
		_Smoothness("Smoothness", Range(0 , 1)) = 0
		[Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		_EmissionTexture("Emission Texture", 2D) = "white" {}
		[HDR]_Emission("Emission", Color) = (0,0,0,0)
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_ScaleDissolveTex("Scale Dissolve Tex", Range(0 , 20)) = 1
		_DissolveSpeed("Dissolve Speed", Vector) = (0.19,0,0,0)
		_GlowDistributionTex("Glow Distribution Tex", 2D) = "white" {}
		_ScaleGlowTex("Scale Glow Tex", Range(0 , 20)) = 1
		_GlowSpeed("Glow Speed", Vector) = (0.19,0,0,0)
		[HDR]_DissolveColor("Dissolve Color", Color) = (0,0,0,0)
		[HDR]_GlowDistributionColor("Glow Distribution Color", Color) = (0,0,0,0)
		_Cutoff("Mask Clip Value", Float) = 0
		_Dissolve("Dissolve", Range(0 , 1)) = 0
		_DissolveDistance("Dissolve Distance", Range(0 , 1)) = 0
		_GlowDistance("Glow Distance", Range(0 , 1)) = 0.1
		_GlowDistribution("Glow Distribution", Range(0 , 1)) = 0
		_StartPoint("Start Point", Range(-3 , 3)) = 0
		[Toggle]_MirrorMode("Mirror Mode", Float) = 0
		[HideInInspector] _texcoord("", 2D) = "white" {}
		[HideInInspector] __dirty("", Int) = 1
	}

		SubShader
		{
			Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
			Cull Off
			CGPROGRAM
			#include "UnityShaderVariables.cginc"
			#pragma target 3.0
			#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
			struct Input
			{
				float2 uv_texcoord;
				float3 worldPos;
			};

			uniform sampler2D _NormalMap;
			uniform float4 _NormalMap_ST;
			uniform float4 _AlbedoColor;
			uniform sampler2D _Albedo;
			uniform float4 _Albedo_ST;
			uniform float _MirrorMode;
			uniform float _StartPoint;
			uniform float _DissolveDistance;
			uniform sampler2D _DissolveTexture;
			uniform float2 _DissolveSpeed;
			uniform float _ScaleDissolveTex;
			uniform float _Dissolve;
			uniform float _GlowDistance;
			uniform float4 _DissolveColor;
			uniform sampler2D _EmissionTexture;
			uniform float4 _EmissionTexture_ST;
			uniform float4 _Emission;
			uniform float _GlowDistribution;
			uniform float4 _GlowDistributionColor;
			uniform sampler2D _GlowDistributionTex;
			uniform float2 _GlowSpeed;
			uniform float _ScaleGlowTex;
			uniform sampler2D _MetallicTexture;
			uniform float4 _MetallicTexture_ST;
			uniform float _Metallic;
			uniform float _Smoothness;
			uniform float _Cutoff = 0;

			void surf(Input i , inout SurfaceOutputStandard o)
			{
				float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
				o.Normal = tex2D(_NormalMap, uv_NormalMap).rgb;
				float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
				o.Albedo = (_AlbedoColor * tex2D(_Albedo, uv_Albedo)).rgb;
				float3 ase_vertex3Pos = mul(unity_WorldToObject, float4(i.worldPos , 1));
				float temp_output_102_0 = (_StartPoint - ase_vertex3Pos.y);
				float temp_output_112_0 = (lerp(temp_output_102_0,length(temp_output_102_0),_MirrorMode) - _DissolveDistance);
				float2 panner15 = (float2(0,0) + _Time.y * _DissolveSpeed);
				float2 uv_TexCoord16 = i.uv_texcoord * float2(1,1) + panner15;
				float temp_output_118_0 = (temp_output_112_0 + (_DissolveDistance * (tex2D(_DissolveTexture, (uv_TexCoord16*_ScaleDissolveTex + float2(0,0))).r * (1.0 - _Dissolve))));
				float2 uv_EmissionTexture = i.uv_texcoord * _EmissionTexture_ST.xy + _EmissionTexture_ST.zw;
				float2 panner245 = (float2(0,0) + _Time.y * _GlowSpeed);
				float2 uv_TexCoord247 = i.uv_texcoord * float2(1,1) + panner245;
				o.Emission = ((temp_output_118_0 <= _GlowDistance) ? _DissolveColor : (((tex2D(_EmissionTexture, uv_EmissionTexture) * _Emission) + saturate((1.0 - ((1.0 - (-1.0 + (_GlowDistribution - 0.0) * (1.0 - -1.0) / (1.0 - 0.0))) + temp_output_112_0)))) * (_GlowDistributionColor * tex2D(_GlowDistributionTex, (uv_TexCoord247*_ScaleGlowTex + float2(0,0)))))).rgb;
				float2 uv_MetallicTexture = i.uv_texcoord * _MetallicTexture_ST.xy + _MetallicTexture_ST.zw;
				float4 tex2DNode22 = tex2D(_MetallicTexture, uv_MetallicTexture);
				o.Metallic = (tex2DNode22.r * _Metallic);
				o.Smoothness = (tex2DNode22.a * _Smoothness);
				o.Alpha = 1;
				clip(temp_output_118_0 - _Cutoff);
			}

			ENDCG
		}
			Fallback "Diffuse"
				CustomEditor "ASEMaterialInspector"
}