// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/DistortionSphere"
{
	Properties
	{
		[Toggle]_MaskDebugMode("Mask Debug Mode", Int) = 0
		[Toggle]_MaskSubtractCenter("Mask Subtract Center", Int) = 0
		_FresnelMaskExpFirst("Fresnel Mask Exp First", Range( 0.2 , 1)) = 4
		_FresnelMaskExpSecond("Fresnel Mask Exp Second", Range( 1 , 10)) = 0
		_DistortionPower("Distortion Power", Range( 0 , 0.5)) = 0.25
		_NoiseNormal01("Noise Normal 01", 2D) = "white" {}
		_NoiseNormal01ScrollSpeed("Noise Normal 01 Scroll Speed", Float) = 0.25
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _MASKSUBTRACTCENTER_ON
		#pragma shader_feature _MASKDEBUGMODE_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float3 worldNormal;
			float3 viewDir;
			float4 screenPos;
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float _FresnelMaskExpFirst;
		uniform float _FresnelMaskExpSecond;
		uniform sampler2D _GrabTexture;
		uniform sampler2D _NoiseNormal01;
		uniform float4 _NoiseNormal01_ST;
		uniform float _NoiseNormal01ScrollSpeed;
		uniform float _DistortionPower;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float dotResult5 = dot( i.worldNormal , i.viewDir );
			float clampResult15 = clamp( dotResult5 , 0.0 , 1.0 );
			float temp_output_50_0 = pow( ( 1.0 - pow( ( 1.0 - clampResult15 ) , _FresnelMaskExpFirst ) ) , _FresnelMaskExpSecond );
			float clampResult55 = clamp( ( temp_output_50_0 - ( temp_output_50_0 * temp_output_50_0 ) ) , 0.0 , 1.0 );
			#ifdef _MASKSUBTRACTCENTER_ON
				float staticSwitch56 = clampResult55;
			#else
				float staticSwitch56 = temp_output_50_0;
			#endif
			float4 temp_cast_0 = (staticSwitch56).xxxx;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos4 = ase_screenPos;
			#if UNITY_UV_STARTS_AT_TOP
			float scale4 = -1.0;
			#else
			float scale4 = 1.0;
			#endif
			float halfPosW4 = ase_screenPos4.w * 0.5;
			ase_screenPos4.y = ( ase_screenPos4.y - halfPosW4 ) * _ProjectionParams.x* scale4 + halfPosW4;
			ase_screenPos4.xyzw /= ase_screenPos4.w;
			float3 temp_output_33_0 = abs( i.worldNormal );
			float2 uv_NoiseNormal01 = i.uv_texcoord * _NoiseNormal01_ST.xy + _NoiseNormal01_ST.zw;
			float temp_output_27_0 = ( _Time.y * _NoiseNormal01ScrollSpeed );
			float2 appendResult25 = (float2(uv_NoiseNormal01.x , ( uv_NoiseNormal01.y + temp_output_27_0 )));
			float3 tex2DNode40 = UnpackNormal( tex2D( _NoiseNormal01, appendResult25 ) );
			float2 appendResult22 = (float2(tex2DNode40.r , tex2DNode40.g));
			float3 ase_worldPos = i.worldPos;
			float2 appendResult30 = (float2(ase_worldPos.x , ( ase_worldPos.z + temp_output_27_0 )));
			float3 tex2DNode36 = UnpackNormal( tex2D( _NoiseNormal01, appendResult30 ) );
			float2 appendResult37 = (float2(tex2DNode36.r , tex2DNode36.g));
			float3 weightedBlendVar28 = ( temp_output_33_0 * temp_output_33_0 );
			float2 weightedBlend28 = ( weightedBlendVar28.x*appendResult22 + weightedBlendVar28.y*appendResult37 + weightedBlendVar28.z*appendResult22 );
			float4 screenColor3 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ( ase_screenPos4 + float4( ( weightedBlend28 * _DistortionPower * staticSwitch56 ), 0.0 , 0.0 ) ) ) );
			#ifdef _MASKDEBUGMODE_ON
				float4 staticSwitch52 = temp_cast_0;
			#else
				float4 staticSwitch52 = screenColor3;
			#endif
			o.Emission = staticSwitch52.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;2650.783;590.1009;2.619922;True;False
Node;AmplifyShaderEditor.WorldNormalVector;6;-2602.688,1058.166;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;7;-2594.688,1208.168;Float;False;World;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;5;-2294.689,1138.167;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;15;-2164.687,1140.167;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-2301.393,1269.872;Float;False;Property;_FresnelMaskExpFirst;Fresnel Mask Exp First;2;0;4;0.2;1;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;48;-2015.48,1145.288;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;11;-1831.072,1181.821;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;26;-2503.953,523.83;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-2586.887,667.4197;Float;False;Property;_NoiseNormal01ScrollSpeed;Noise Normal 01 Scroll Speed;6;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;49;-1654.014,1185.228;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;51;-1800.218,1287.398;Float;False;Property;_FresnelMaskExpSecond;Fresnel Mask Exp Second;3;0;0;1;10;0;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-2363.601,369.4569;Float;False;0;35;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-2255.145,593.149;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;29;-2319.051,203.3791;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PowerNode;50;-1445.03,1225.081;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-2002.167,307.3575;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1996.441,475.5544;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;30;-1802.875,166.2436;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1211.985,1368.926;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;35;-2005.882,692.3251;Float;True;Property;_NoiseNormal01;Noise Normal 01;5;0;None;True;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.WorldNormalVector;32;-1889.522,-88.75121;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1808.29,409.9488;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;36;-1580.065,225.6602;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;53;-987.9842,1234.926;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;40;-1597.246,436.8414;Float;True;Property;_TextureSample1;Texture Sample 1;4;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;33;-1684.041,-89.98874;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.DynamicAppendNode;37;-1268.13,257.8437;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ClampOpNode;55;-828.6782,1235.328;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1518.171,-99.89145;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.DynamicAppendNode;22;-1264.732,462.4441;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.StaticSwitch;56;-546.0043,1053.719;Float;False;Property;_MaskSubtractCenter;Mask Subtract Center;1;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SummedBlendNode;28;-767.4752,159.8148;Float;False;5;0;FLOAT3;0,0,0;False;1;FLOAT2;0;False;2;FLOAT2;0,0;False;3;FLOAT2;0.0,0;False;4;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;19;-830.2181,523.4987;Float;False;Property;_DistortionPower;Distortion Power;4;0;0.25;0;0.5;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-461.1324,505.7238;Float;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;2;FLOAT;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.GrabScreenPosition;4;-400.6196,177.7783;Float;False;0;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-11.5915,307.8679;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.ScreenColorNode;3;203.3788,175.7783;Float;False;Global;_GrabScreen0;Grab Screen 0;0;0;Object;-1;False;1;0;FLOAT4;0,0,0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;38;-1098.546,255.3675;Float;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2
Node;AmplifyShaderEditor.TFHCRemapNode;20;-1098.344,465.5573;Float;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2
Node;AmplifyShaderEditor.StaticSwitch;52;451.8922,267.4698;Float;False;Property;_MaskDebugMode;Mask Debug Mode;0;0;0;False;True;;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;830.9298,-43.8433;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/DistortionSphere;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;15;0;5;0
WireConnection;48;0;15;0
WireConnection;11;0;48;0
WireConnection;11;1;12;0
WireConnection;49;0;11;0
WireConnection;27;0;26;2
WireConnection;27;1;24;0
WireConnection;50;0;49;0
WireConnection;50;1;51;0
WireConnection;31;0;29;3
WireConnection;31;1;27;0
WireConnection;23;0;21;2
WireConnection;23;1;27;0
WireConnection;30;0;29;1
WireConnection;30;1;31;0
WireConnection;54;0;50;0
WireConnection;54;1;50;0
WireConnection;25;0;21;1
WireConnection;25;1;23;0
WireConnection;36;0;35;0
WireConnection;36;1;30;0
WireConnection;53;0;50;0
WireConnection;53;1;54;0
WireConnection;40;0;35;0
WireConnection;40;1;25;0
WireConnection;33;0;32;0
WireConnection;37;0;36;1
WireConnection;37;1;36;2
WireConnection;55;0;53;0
WireConnection;34;0;33;0
WireConnection;34;1;33;0
WireConnection;22;0;40;1
WireConnection;22;1;40;2
WireConnection;56;0;55;0
WireConnection;56;1;50;0
WireConnection;28;0;34;0
WireConnection;28;1;22;0
WireConnection;28;2;37;0
WireConnection;28;3;22;0
WireConnection;16;0;28;0
WireConnection;16;1;19;0
WireConnection;16;2;56;0
WireConnection;18;0;4;0
WireConnection;18;1;16;0
WireConnection;3;0;18;0
WireConnection;38;0;37;0
WireConnection;20;0;22;0
WireConnection;52;0;56;0
WireConnection;52;1;3;0
WireConnection;0;2;52;0
ASEEND*/
//CHKSM=BECE30D92DFB8E8FA51D3976263C253F70EB44F4