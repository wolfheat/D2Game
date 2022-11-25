// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/SimpleEmbers"
{
	Properties
	{
		_FinalPower("Final Power", Range( 0 , 10)) = 1
		_FinalColor("Final Color", Color) = (1,1,1,1)
		_FinalOpacityPower("Final Opacity Power", Range( 0 , 4)) = 1
		_EmberTex("Ember Tex", 2D) = "white" {}
		_Ramp("Ramp", 2D) = "white" {}
		_RampMask("Ramp Mask", 2D) = "white" {}
		_SoftParticleValue("Soft Particle Value", Float) = 0.25
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform sampler2D _RampMask;
		uniform sampler2D _EmberTex;
		uniform float4 _EmberTex_ST;
		uniform sampler2D _Ramp;
		uniform float4 _FinalColor;
		uniform float _FinalPower;
		uniform float _FinalOpacityPower;
		uniform sampler2D _CameraDepthTexture;
		uniform float _SoftParticleValue;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_EmberTex = i.uv_texcoord * _EmberTex_ST.xy + _EmberTex_ST.zw;
			float4 tex2DNode1 = tex2D( _EmberTex, uv_EmberTex );
			float2 appendResult3 = (float2(tex2DNode1.r , 0.0));
			o.Emission = ( tex2D( _RampMask, appendResult3 ).r * tex2D( _Ramp, appendResult3 ) * i.vertexColor * _FinalColor * _FinalPower ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth17 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth17 = abs( ( screenDepth17 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _SoftParticleValue ) );
			float clampResult19 = clamp( distanceDepth17 , 0.0 , 1.0 );
			float clampResult13 = clamp( ( i.vertexColor.a * _FinalOpacityPower * tex2DNode1.r * clampResult19 ) , 0.0 , 1.0 );
			o.Alpha = clampResult13;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;1243.564;222.3367;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;18;-689.7635,668.1627;Float;False;Property;_SoftParticleValue;Soft Particle Value;6;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DepthFade;17;-457.064,664.2622;Float;False;True;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;4;-1211.401,-81.30001;Float;False;Constant;_Float0;Float 0;1;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-1356.401,-271.3;Float;True;Property;_EmberTex;Ember Tex;3;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/SpriteSheets/BetterEmbers05.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;19;-243.8643,660.3625;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.WireNode;16;-796.7661,539.0632;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-667.7784,418.7971;Float;False;Property;_FinalOpacityPower;Final Opacity Power;2;0;1;0;4;0;1;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;6;-577.3997,3.899979;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;3;-977.3997,-182.3;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;115.4166,229.1591;Float;False;4;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;7;-699.3997,-196.3001;Float;True;Property;_Ramp;Ramp;4;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Ramps/Ramp19.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;8;-608.3996,172.7;Float;False;Property;_FinalColor;Final Color;1;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;10;-697.3997,-393.2998;Float;True;Property;_RampMask;Ramp Mask;5;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Ramps/RampMask02.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;9;-670.3997,339.7002;Float;False;Property;_FinalPower;Final Power;0;0;1;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;13;257.403,230.7234;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;105.7952,-91.23809;Float;False;5;5;0;FLOAT;0.0;False;1;COLOR;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;565.9949,-44.53809;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/SimpleEmbers;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;18;0
WireConnection;19;0;17;0
WireConnection;16;0;1;1
WireConnection;3;0;1;1
WireConnection;3;1;4;0
WireConnection;11;0;6;4
WireConnection;11;1;12;0
WireConnection;11;2;16;0
WireConnection;11;3;19;0
WireConnection;7;1;3;0
WireConnection;10;1;3;0
WireConnection;13;0;11;0
WireConnection;5;0;10;1
WireConnection;5;1;7;0
WireConnection;5;2;6;0
WireConnection;5;3;8;0
WireConnection;5;4;9;0
WireConnection;0;2;5;0
WireConnection;0;9;13;0
ASEEND*/
//CHKSM=3C81A0F9B885B41B565FE2F937DC82E82043F765