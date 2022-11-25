// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/SimpleFlare"
{
	Properties
	{
		_FinalPower("Final Power", Range( 0 , 10)) = 2
		_Ramp("Ramp", 2D) = "white" {}
		_RampMask("Ramp Mask", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		_RampMultiplyTiling("Ramp Multiply Tiling", Float) = 1
		[Toggle]_RampFlip("Ramp Flip", Int) = 0
		_FlareTex("Flare Tex", 2D) = "white" {}
		_OpacityPower("Opacity Power", Range( 0 , 4)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma target 3.0
		#pragma shader_feature _RAMPFLIP_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _RampColorTint;
		uniform float _FinalPower;
		uniform sampler2D _Ramp;
		uniform float _RampMultiplyTiling;
		uniform sampler2D _FlareTex;
		uniform float4 _FlareTex_ST;
		uniform sampler2D _RampMask;
		uniform float _OpacityPower;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_FlareTex = i.uv_texcoord * _FlareTex_ST.xy + _FlareTex_ST.zw;
			float4 tex2DNode14 = tex2D( _FlareTex, uv_FlareTex );
			float ResultMask17 = tex2DNode14.r;
			float clampResult4 = clamp( ( _RampMultiplyTiling * ResultMask17 ) , 0.0 , 1.0 );
			#ifdef _RAMPFLIP_ON
				float staticSwitch6 = ( 1.0 - clampResult4 );
			#else
				float staticSwitch6 = clampResult4;
			#endif
			float2 appendResult8 = (float2(staticSwitch6 , 0.0));
			o.Emission = ( _RampColorTint * _FinalPower * tex2D( _Ramp, appendResult8 ) * tex2D( _RampMask, appendResult8 ).r * i.vertexColor ).rgb;
			o.Alpha = ( tex2DNode14.r * _OpacityPower * i.vertexColor.a );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;1741.566;-31.11652;1.173489;True;False
Node;AmplifyShaderEditor.SamplerNode;14;-621.8949,486.9438;Float;True;Property;_FlareTex;Flare Tex;6;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Masks/Flare01.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;1;-1867.055,64.86041;Float;False;Property;_RampMultiplyTiling;Ramp Multiply Tiling;4;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-239.9598,662.3587;Float;False;ResultMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;2;-1848.705,181.6018;Float;False;17;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1589.271,112.7455;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;4;-1437.271,107.7455;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;5;-1276.194,0.5424638;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;6;-1099.614,78.25365;Float;False;Property;_RampFlip;Ramp Flip;5;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;7;-1022.845,184.1643;Float;False;Constant;_Float1;Float 1;12;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;8;-826.5098,120.0994;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;10;-638.9949,89.36797;Float;True;Property;_Ramp;Ramp;1;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Ramps/Ramp19.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-621.7399,-2.182764;Float;False;Property;_FinalPower;Final Power;0;0;2;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;18;-497.1986,766.5828;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;16;-603.9581,688.5457;Float;False;Property;_OpacityPower;Opacity Power;7;0;1;0;4;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;12;-636.181,280.8583;Float;True;Property;_RampMask;Ramp Mask;2;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Ramps/RampMask02.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;9;-558.6719,-175.3391;Float;False;Property;_RampColorTint;Ramp Color Tint;3;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-272.7386,-20.98285;Float;False;5;5;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-237.3411,551.0637;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/SimpleFlare;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;14;1
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;4;0;3;0
WireConnection;5;0;4;0
WireConnection;6;0;5;0
WireConnection;6;1;4;0
WireConnection;8;0;6;0
WireConnection;8;1;7;0
WireConnection;10;1;8;0
WireConnection;12;1;8;0
WireConnection;13;0;9;0
WireConnection;13;1;11;0
WireConnection;13;2;10;0
WireConnection;13;3;12;1
WireConnection;13;4;18;0
WireConnection;15;0;14;1
WireConnection;15;1;16;0
WireConnection;15;2;18;4
WireConnection;0;2;13;0
WireConnection;0;9;15;0
ASEEND*/
//CHKSM=640BB897B58ADB851AB7D70B9BEC0D189B5C8AAA