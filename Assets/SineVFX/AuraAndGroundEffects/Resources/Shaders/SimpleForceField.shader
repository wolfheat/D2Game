// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SimpleForceField"
{
	Properties
	{
		_FresnelExp("Fresnel Exp", Range( 0.2 , 4)) = 1
		_DepthFadeDistance("Depth Fade Distance", Float) = 0.25
		_DepthFadeExp("Depth Fade Exp", Range( 0.2 , 10)) = 4
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			fixed ASEVFace : VFACE;
			float4 screenPos;
		};

		uniform float _FresnelExp;
		uniform sampler2D _CameraDepthTexture;
		uniform float _DepthFadeDistance;
		uniform float _DepthFadeExp;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float3 temp_cast_0 = (1.0).xxx;
			o.Emission = temp_cast_0;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 switchResult27 = (((i.ASEVFace>0)?(float3(0,0,1)):(float3(0,0,-1))));
			float dotResult1 = dot( ase_worldViewDir , WorldNormalVector( i , switchResult27 ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth11 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth11 = abs( ( screenDepth11 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFadeDistance ) );
			float clampResult19 = clamp( ( 1.0 - distanceDepth11 ) , 0.0 , 1.0 );
			float clampResult17 = clamp( max( pow( ( 1.0 - dotResult1 ) , _FresnelExp ) , pow( clampResult19 , _DepthFadeExp ) ) , 0.0 , 1.0 );
			o.Alpha = clampResult17;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
284;557;1906;1004;665.7031;-130.5597;1.298652;True;False
Node;AmplifyShaderEditor.Vector3Node;29;-911.67,146.9046;Float;False;Constant;_Vector0;Vector 0;3;0;0,0,1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector3Node;30;-916.4661,311.566;Float;False;Constant;_Vector1;Vector 1;3;0;0,0,-1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SwitchByFaceNode;27;-694.2536,241.2251;Float;False;2;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;13;-606.5987,488.5381;Float;False;Property;_DepthFadeDistance;Depth Fade Distance;1;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;8;-519.3041,239.1077;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DepthFade;11;-378.5986,490.5381;Float;False;True;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;3;-511.304,-68.89219;Float;False;World;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;12;-183.5987,489.5381;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;1;-167.3031,23.10783;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;7;-168.3031,118.1078;Float;False;Property;_FresnelExp;Fresnel Exp;0;0;1;0.2;4;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;16;-144.128,609.7081;Float;False;Property;_DepthFadeExp;Depth Fade Exp;2;0;4;0.2;10;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;19;-21.74458,490.1898;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;5;-43.3031,26.10784;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;15;176.076,537.7083;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;6;159.6968,62.10778;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMaxOpNode;10;431.4555,283.7894;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;17;568.455,283.7894;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.NormalVertexDataNode;2;-521.3041,92.10777;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;4;580.2929,-101.5447;Float;False;Constant;_Float0;Float 0;0;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1010.994,-43.18921;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SimpleForceField;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Off;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;29;0
WireConnection;27;1;30;0
WireConnection;8;0;27;0
WireConnection;11;0;13;0
WireConnection;12;0;11;0
WireConnection;1;0;3;0
WireConnection;1;1;8;0
WireConnection;19;0;12;0
WireConnection;5;0;1;0
WireConnection;15;0;19;0
WireConnection;15;1;16;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;10;0;6;0
WireConnection;10;1;15;0
WireConnection;17;0;10;0
WireConnection;0;2;4;0
WireConnection;0;9;17;0
ASEEND*/
//CHKSM=5CAF059DAFC79337F6735DF808FF49F40956219F