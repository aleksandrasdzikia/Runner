﻿Shader "Custom/yar" {
	Properties {
      _ColorLow ("Color Low", COLOR) = (0,0,0,1)
      _ColorHigh ("Color High", COLOR) = (0,0,0,1)
      _yPosLow ("Y Pos Low", Float) = 0
      _yPosHigh ("Y Pos High", Float) = 10
      _GradientStrength ("Graident Strength", Float) = 1
      _EmissiveStrengh ("Emissive Strengh ", Float) = 1
      _ColorX ("Color X", COLOR) = (1,1,1,1)
      _ColorY ("Color Y", COLOR) = (1,1,1,1)
	  _ColorZ ("Color Z", COLOR) = (1,1,1,1)
	}
	SubShader {
		Tags { 
      "Queue" = "Geometry"
      "RenderType"="Opaque" 
      }
		
		CGPROGRAM
		#pragma surface surf Lambert
      #define WHITE3 fixed3(1,1,1)
      #define UP float3(0,1,0)
      #define RIGHT float3(1,0,0)
	  #define FORWARD float3(0,0,1)

      fixed4 _ColorLow;
      fixed4 _ColorHigh;
      fixed4 _ColorX;
      fixed4 _ColorY;
	  fixed4 _ColorZ;
      half _yPosLow;
      half _yPosHigh;
      half _GradientStrength;
      half _EmissiveStrengh;
      
		struct Input {
			float2 uv_MainTex;
         float3 worldPos;
         float3 normal;
		};

		void surf (Input IN, inout SurfaceOutput o) {
         // gradient color at this height
         half3 gradient = lerp(_ColorLow, _ColorHigh,  smoothstep( _yPosLow, _yPosHigh, IN.worldPos.y )).rgb;
         
         // lerp the 
         gradient = lerp(WHITE3, gradient, _GradientStrength);
         // add ColorX if the normal is facing positive X-ish
         half3 finalColor = _ColorX.rgb * max(0,dot(o.Normal, -RIGHT))* _ColorX.a;
         
         // add ColorY if the normal is facing positive Y-ish (up)
         finalColor += _ColorY.rgb * max(0,dot(o.Normal, UP)) * _ColorY.a;

		 finalColor += _ColorZ.rgb * max(0,dot(o.Normal, -FORWARD)) * _ColorZ.a;
         
         // add the gradient color
         finalColor += gradient;
         
         // scale down to 0-1 values
         finalColor = saturate(finalColor);
         
         // how much should go to emissive
         o.Emission = lerp(half3(0,0,0), finalColor, _EmissiveStrengh);
         
         // the "color" before lighting is applied
         o.Albedo = finalColor * saturate(1 - _EmissiveStrengh);
         
         // opaque
			o.Alpha = 1;
		}
		ENDCG
	} 
   fallback "Vertex Lit"
}