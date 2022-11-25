// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/DarkShellSphere"
{
	Properties
	{
		_FresnelExp("Fresnel Exp", Range( 0.2 , 2)) = 1
		_FresnelMultiply("Fresnel Multiply", Range( 1 , 4)) = 1
		_Noise01("Noise 01", 2D) = "black" {}
		_Noise01ScrollSpeed("Noise 01 Scroll Speed", Float) = 0.25
		_DistortionNoise("Distortion Noise", 2D) = "white" {}
		_DistortionNoisePower("Distortion Noise Power", Range( 0 , 1)) = 0.25
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend One OneMinusSrcAlpha
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:premul keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			fixed ASEVFace : VFACE;
			float2 uv_texcoord;
		};

		uniform float _FresnelExp;
		uniform float _FresnelMultiply;
		uniform sampler2D _Noise01;
		uniform float4 _Noise01_ST;
		uniform float _Noise01ScrollSpeed;
		uniform sampler2D _DistortionNoise;
		uniform float4 _DistortionNoise_ST;
		uniform float _DistortionNoisePower;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float3 temp_cast_0 = (0.0).xxx;
			o.Emission = temp_cast_0;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 switchResult9 = (((i.ASEVFace>0)?(float3(0,0,1)):(float3(0,0,-1))));
			float dotResult1 = dot( ase_worldViewDir , WorldNormalVector( i , switchResult9 ) );
			float2 uv_Noise01 = i.uv_texcoord * _Noise01_ST.xy + _Noise01_ST.zw;
			float2 appendResult35 = (float2(uv_Noise01.x , ( uv_Noise01.y + ( _Time.y * _Noise01ScrollSpeed ) )));
			float2 uv_DistortionNoise = i.uv_texcoord * _DistortionNoise_ST.xy + _DistortionNoise_ST.zw;
			float clampResult17 = clamp( ( ( pow( ( 1.0 - dotResult1 ) , _FresnelExp ) * _FresnelMultiply ) + tex2D( _Noise01, ( appendResult35 + ( tex2D( _DistortionNoise, uv_DistortionNoise ).r * _DistortionNoisePower ) ) ).r ) , 0.0 , 1.0 );
			o.Alpha = clampResult17;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;1288.527;-301.7637;1;True;False
Node;AmplifyShaderEditor.Vector3Node;10;-1199.213,315.7068;Float;False;Constant;_Vector0;Vector 0;1;0;0,0,1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector3Node;11;-1195.313,465.207;Float;False;Constant;_Vector1;Vector 1;1;0;0,0,-1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SwitchByFaceNode;9;-943.1132,385.9066;Float;False;2;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;34;-1004.527,961.7637;Float;False;Property;_Noise01ScrollSpeed;Noise 01 Scroll Speed;3;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;32;-980.527,819.7637;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;8;-783.1,383.6001;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;3;-783,76;Float;False;World;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-835.3845,611.8348;Float;False;0;27;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-721.527,883.7637;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-936.527,1141.764;Float;False;0;36;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;1;-528,154;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;36;-619.527,1023.764;Float;True;Property;_DistortionNoise;Distortion Noise;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;40;-602.527,1218.764;Float;False;Property;_DistortionNoisePower;Distortion Noise Power;5;0;0.25;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-529.527,728.7637;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;5;-371.1,164.4999;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-282.527,1119.764;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;35;-354.527,650.7637;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;7;-523,251;Float;False;Property;_FresnelExp;Fresnel Exp;0;0;1;0.2;2;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;29;-313.1071,330.6312;Float;False;Property;_FresnelMultiply;Fresnel Multiply;1;0;1;1;4;0;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;6;-192,194;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-141.527,724.7637;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;27;32.48779,619.1072;Float;True;Property;_Noise01;Noise 01;2;0;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;2.892944,220.6312;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;26;176.1876,313.1071;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;4;307.6002,-247.9005;Float;False;Constant;_Float0;Float 0;0;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;17;333.4877,301.4069;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.NormalVertexDataNode;2;-785.4,230.8999;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;592.7996,-37.7;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/DarkShellSphere;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Premultiply;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;3;One;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;10;0
WireConnection;9;1;11;0
WireConnection;8;0;9;0
WireConnection;33;0;32;2
WireConnection;33;1;34;0
WireConnection;1;0;3;0
WireConnection;1;1;8;0
WireConnection;36;1;37;0
WireConnection;31;0;30;2
WireConnection;31;1;33;0
WireConnection;5;0;1;0
WireConnection;39;0;36;1
WireConnection;39;1;40;0
WireConnection;35;0;30;1
WireConnection;35;1;31;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;38;0;35;0
WireConnection;38;1;39;0
WireConnection;27;1;38;0
WireConnection;28;0;6;0
WireConnection;28;1;29;0
WireConnection;26;0;28;0
WireConnection;26;1;27;1
WireConnection;17;0;26;0
WireConnection;0;2;4;0
WireConnection;0;9;17;0
ASEEND*/
//CHKSM=567C9BCB60782A53AE3383C5BA6AA68E8B0BFAEF