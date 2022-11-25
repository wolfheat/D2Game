// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DarkBackgroundSprite"
{
	Properties
	{
		_MaskExp("Mask Exp", Range( 0.2 , 4)) = 1
		_MaskAdd("Mask Add", Range( 0 , 1)) = 1
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01ScrollSpeed("Noise 01 Scroll Speed", Float) = -0.25
		_DistortionNoise("Distortion Noise", 2D) = "white" {}
		_DistortionNoisePower("Distortion Noise Power", Range( 0 , 1)) = 0.25
		_Mask("Mask", 2D) = "white" {}
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
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _MaskAdd;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float _MaskExp;
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
			float3 temp_cast_0 = (0.0).xxx;
			o.Emission = temp_cast_0;
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float temp_output_9_0 = pow( tex2D( _Mask, uv_Mask ).r , _MaskExp );
			float2 uv_Noise01 = i.uv_texcoord * _Noise01_ST.xy + _Noise01_ST.zw;
			float2 appendResult19 = (float2(uv_Noise01.x , ( uv_Noise01.y + ( _Time.y * _Noise01ScrollSpeed ) )));
			float2 uv_DistortionNoise = i.uv_texcoord * _DistortionNoise_ST.xy + _DistortionNoise_ST.zw;
			float clampResult11 = clamp( ( ( _MaskAdd * temp_output_9_0 ) + ( temp_output_9_0 * tex2D( _Noise01, ( appendResult19 + ( tex2D( _DistortionNoise, uv_DistortionNoise ).r * _DistortionNoisePower ) ) ).r ) ) , 0.0 , 1.0 );
			o.Alpha = ( clampResult11 * i.vertexColor.a );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;-0.4230042;232.204;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;16;-1325.577,709.796;Float;False;Property;_Noise01ScrollSpeed;Noise 01 Scroll Speed;3;0;-0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;17;-1303.577,564.796;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1028.576,640.796;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1138.896,425.1858;Float;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-1284.577,867.796;Float;False;0;20;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;22;-1009.577,1043.796;Float;False;Property;_DistortionNoisePower;Distortion Noise Power;5;0;0.25;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;20;-1031.577,847.796;Float;True;Property;_DistortionNoise;Distortion Noise;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-799.5765,541.796;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;19;-645.5765,475.796;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-684.577,961.796;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-438.577,493.796;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;10;-669.5975,373.3856;Float;False;Property;_MaskExp;Mask Exp;0;0;1;0.2;4;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;30;-688.577,146.796;Float;True;Property;_Mask;Mask;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PowerNode;9;-198.9963,225.1851;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;26;-239.577,27.79601;Float;False;Property;_MaskAdd;Mask Add;1;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-253.5004,449.0999;Float;True;Property;_Noise01;Noise 01;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;176.423,123.796;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;169.704,355.2855;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;12;412.8037,237.0854;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;29;895.423,390.796;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;11;924.4019,270.7856;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;1147.423,368.796;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;3;1204.2,-78.4;Float;False;Constant;_Float0;Float 0;2;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1410.699,-58.00004;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;DarkBackgroundSprite;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Premultiply;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;3;One;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;17;2
WireConnection;18;1;16;0
WireConnection;20;1;24;0
WireConnection;15;0;13;2
WireConnection;15;1;18;0
WireConnection;19;0;13;1
WireConnection;19;1;15;0
WireConnection;21;0;20;1
WireConnection;21;1;22;0
WireConnection;23;0;19;0
WireConnection;23;1;21;0
WireConnection;9;0;30;1
WireConnection;9;1;10;0
WireConnection;2;1;23;0
WireConnection;25;0;26;0
WireConnection;25;1;9;0
WireConnection;14;0;9;0
WireConnection;14;1;2;1
WireConnection;12;0;25;0
WireConnection;12;1;14;0
WireConnection;11;0;12;0
WireConnection;28;0;11;0
WireConnection;28;1;29;4
WireConnection;0;2;3;0
WireConnection;0;9;28;0
ASEEND*/
//CHKSM=8D06E2BEAA5840E66B85134D03E40A4A5D2387AE