// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/FresnelSphereOpaque"
{
	Properties
	{
		_FinalPower("Final Power", Range( 0 , 10)) = 2
		_FinalColor("Final Color", Color) = (1,1,1,1)
		_FinalFresnelExp("Final Fresnel Exp", Range( 0.2 , 4)) = 1
		_Ramp("Ramp", 2D) = "white" {}
		_OffsetTex("Offset Tex", 2D) = "white" {}
		_OffsetTiling("Offset Tiling", Float) = 1
		_OffsetScrollSpeed("Offset Scroll Speed", Float) = 0.25
		_OffsetPower("Offset Power", Range( 0 , 1)) = 0.15
		_OffsetRemapMinNew("Offset Remap Min New", Float) = -1
		_OffsetRemapMaxNew("Offset Remap Max New", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Ramp;
		uniform float _FinalFresnelExp;
		uniform float _FinalPower;
		uniform float4 _FinalColor;
		uniform sampler2D _OffsetTex;
		uniform float _OffsetTiling;
		uniform float _OffsetScrollSpeed;
		uniform float _OffsetRemapMinNew;
		uniform float _OffsetRemapMaxNew;
		uniform float _OffsetPower;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 temp_output_41_0 = abs( ase_worldNormal );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult31 = (float2(( ( ase_worldPos * _OffsetTiling ) + ( _Time.y * _OffsetScrollSpeed ) ).y , ( ( ase_worldPos * _OffsetTiling ) + ( _Time.y * _OffsetScrollSpeed ) ).z));
			float2 appendResult32 = (float2(( ( ase_worldPos * _OffsetTiling ) + ( _Time.y * _OffsetScrollSpeed ) ).z , ( ( ase_worldPos * _OffsetTiling ) + ( _Time.y * _OffsetScrollSpeed ) ).x));
			float2 appendResult34 = (float2(( ( ase_worldPos * _OffsetTiling ) + ( _Time.y * _OffsetScrollSpeed ) ).x , ( ( ase_worldPos * _OffsetTiling ) + ( _Time.y * _OffsetScrollSpeed ) ).y));
			float3 weightedBlendVar38 = ( temp_output_41_0 * temp_output_41_0 );
			float weightedBlend38 = ( weightedBlendVar38.x*tex2Dlod( _OffsetTex, float4( appendResult31, 0, 0.0) ).r + weightedBlendVar38.y*tex2Dlod( _OffsetTex, float4( appendResult32, 0, 0.0) ).g + weightedBlendVar38.z*tex2Dlod( _OffsetTex, float4( appendResult34, 0, 0.0) ).b );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( (_OffsetRemapMinNew + (weightedBlend38 - 0) * (_OffsetRemapMaxNew - _OffsetRemapMinNew) / (1 - 0)) * _OffsetPower * ase_vertexNormal );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			o.Normal = ase_vertexNormal;
			float temp_output_49_0 = 0.0;
			float3 temp_cast_0 = (temp_output_49_0).xxx;
			o.Albedo = temp_cast_0;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult16 = dot( ase_worldViewDir , ase_vertexNormal );
			float temp_output_21_0 = pow( ( 1.0 - dotResult16 ) , _FinalFresnelExp );
			float2 appendResult43 = (float2(temp_output_21_0 , 0.0));
			o.Emission = ( tex2D( _Ramp, appendResult43 ) * temp_output_21_0 * _FinalPower * _FinalColor * i.vertexColor ).rgb;
			o.Metallic = temp_output_49_0;
			o.Smoothness = temp_output_49_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
182;606;1906;1004;3152.192;494.7272;1.625142;True;False
Node;AmplifyShaderEditor.RangedFloatNode;26;-3289.072,547.4752;Float;False;Property;_OffsetScrollSpeed;Offset Scroll Speed;6;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;25;-3277.158,163.8231;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TimeNode;24;-3277.2,407.7923;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;-3260.081,309.3203;Float;False;Property;_OffsetTiling;Offset Tiling;5;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-2963.111,223.5488;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-2959.159,465.292;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-2760.28,340.2055;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;17;-2123.051,-458.3078;Float;False;World;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.NormalVertexDataNode;20;-2123.406,-289.8293;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;16;-1847.049,-395.3078;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;30;-2588.492,333.3736;Float;False;FLOAT3;1;0;FLOAT3;0,0,0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;40;-2070.213,24.07498;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;31;-2085.224,208.6592;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.TexturePropertyNode;39;-2159.174,634.2896;Float;True;Property;_OffsetTex;Offset Tex;4;0;None;False;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.DynamicAppendNode;32;-2083.845,362.9922;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.OneMinusNode;19;-1717.505,-393.1286;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;34;-2082.464,515.9473;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;22;-1818.905,-295.6285;Float;False;Property;_FinalFresnelExp;Final Fresnel Exp;2;0;1;0.2;4;0;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;41;-1846.567,25.34195;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;36;-1819.936,367.8106;Float;True;Property;_TextureSample1;Texture Sample 1;10;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;35;-1814.736,573.21;Float;True;Property;_TextureSample0;Texture Sample 0;11;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;-1489.439,-254.7507;Float;False;Constant;_Float0;Float 0;9;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;21;-1506.904,-360.6285;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;37;-1819.934,172.811;Float;True;Property;_TextureSample2;Texture Sample 2;9;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1678.865,20.142;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SummedBlendNode;38;-1206.336,360.0106;Float;False;5;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;14;-981,613;Float;False;Property;_OffsetRemapMinNew;Offset Remap Min New;8;0;-1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;43;-1262.094,-319.7693;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;15;-985,692;Float;False;Property;_OffsetRemapMaxNew;Offset Remap Max New;9;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;46;-1114.239,-350.1507;Float;True;Property;_Ramp;Ramp;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;5;-642.9001,105.9997;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;10;-643,626;Float;False;Property;_OffsetPower;Offset Power;7;0;0.15;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;4;-747.6999,-142.1;Float;False;Property;_FinalPower;Final Power;0;0;2;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.WireNode;48;-1191.215,-39.1478;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;11;-623,447;Float;False;5;0;FLOAT;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.0;False;4;FLOAT;1,1,1,1;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;6;-675.0997,-63.20008;Float;False;Property;_FinalColor;Final Color;1;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-360.2,-221.5;Float;False;5;5;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;2;FLOAT;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.NormalVertexDataNode;51;-390.2271,41.59341;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-290,498;Float;False;3;3;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;FLOAT3;0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;49;-360.1987,-47.98241;Float;False;Constant;_Float1;Float 1;10;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;SineVFX/AuraEffectsPro/FresnelSphereOpaque;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;1;0;False;0;0;Opaque;0.5;True;False;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;25;0
WireConnection;28;1;13;0
WireConnection;27;0;24;2
WireConnection;27;1;26;0
WireConnection;29;0;28;0
WireConnection;29;1;27;0
WireConnection;16;0;17;0
WireConnection;16;1;20;0
WireConnection;30;0;29;0
WireConnection;31;0;30;1
WireConnection;31;1;30;2
WireConnection;32;0;30;2
WireConnection;32;1;30;0
WireConnection;19;0;16;0
WireConnection;34;0;30;0
WireConnection;34;1;30;1
WireConnection;41;0;40;0
WireConnection;36;0;39;0
WireConnection;36;1;32;0
WireConnection;35;0;39;0
WireConnection;35;1;34;0
WireConnection;21;0;19;0
WireConnection;21;1;22;0
WireConnection;37;0;39;0
WireConnection;37;1;31;0
WireConnection;42;0;41;0
WireConnection;42;1;41;0
WireConnection;38;0;42;0
WireConnection;38;1;37;1
WireConnection;38;2;36;2
WireConnection;38;3;35;3
WireConnection;43;0;21;0
WireConnection;43;1;44;0
WireConnection;46;1;43;0
WireConnection;48;0;21;0
WireConnection;11;0;38;0
WireConnection;11;3;14;0
WireConnection;11;4;15;0
WireConnection;3;0;46;0
WireConnection;3;1;48;0
WireConnection;3;2;4;0
WireConnection;3;3;6;0
WireConnection;3;4;5;0
WireConnection;9;0;11;0
WireConnection;9;1;10;0
WireConnection;9;2;20;0
WireConnection;0;0;49;0
WireConnection;0;1;51;0
WireConnection;0;2;3;0
WireConnection;0;3;49;0
WireConnection;0;4;49;0
WireConnection;0;11;9;0
ASEEND*/
//CHKSM=195C1E4C89AF6680424AB1B84F2597E8E1241286