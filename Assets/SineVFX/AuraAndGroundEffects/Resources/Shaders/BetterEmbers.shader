// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/BetterEmbers"
{
	Properties
	{
		_FinalPower("Final Power", Range( 0 , 10)) = 2
		_OpacityPower("Opacity Power", Range( 0 , 4)) = 1
		_EmbersTexture("Embers Texture", 2D) = "white" {}
		_Ramp("Ramp", 2D) = "white" {}
		_RampMask("Ramp Mask", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		_RampMultiplyTiling("Ramp Multiply Tiling", Float) = 1
		[Toggle]_RampFlip("Ramp Flip", Int) = 0
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01Tiling("Noise 01 Tiling", Float) = 1.25
		_Noise01ScrollSpeedU("Noise 01 Scroll Speed U", Float) = 0.25
		_Noise01ScrollSpeedV("Noise 01 Scroll Speed V", Float) = 0.25
		_Noise01Power("Noise 01 Power", Range( 0 , 1)) = 0.25
		_DistortionNoise("Distortion Noise", 2D) = "white" {}
		_DistortionNoiseTiling("Distortion Noise Tiling", Float) = 1
		_DistortionNoiseScrollSpeedU("Distortion Noise Scroll Speed U", Float) = 0.25
		_DistortionNoiseScrollSpeedV("Distortion Noise Scroll Speed V", Float) = 0.25
		_DistortionNoisePower("Distortion Noise Power", Range( 0 , 1)) = 0.25
		_SoftParticleValue("Soft Particle Value", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _RAMPFLIP_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float3 uv2_texcoord2;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float4 _RampColorTint;
		uniform float _FinalPower;
		uniform sampler2D _Ramp;
		uniform float _RampMultiplyTiling;
		uniform sampler2D _EmbersTexture;
		uniform sampler2D _DistortionNoise;
		uniform float _DistortionNoiseTiling;
		uniform float _DistortionNoiseScrollSpeedU;
		uniform float _DistortionNoiseScrollSpeedV;
		uniform float _DistortionNoisePower;
		uniform sampler2D _Noise01;
		uniform float _Noise01Tiling;
		uniform float _Noise01ScrollSpeedU;
		uniform float _Noise01ScrollSpeedV;
		uniform float _Noise01Power;
		uniform sampler2D _RampMask;
		uniform float _OpacityPower;
		uniform sampler2D _CameraDepthTexture;
		uniform float _SoftParticleValue;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord3 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float3 uv2_TexCoord36 = i.uv2_texcoord2;
			uv2_TexCoord36.xy = i.uv2_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
			float2 appendResult5 = (float2(uv_TexCoord3.x , ( ( uv_TexCoord3.y * 0.25 ) + ( 0.25 * floor( uv2_TexCoord36.x ) ) )));
			float2 uv_TexCoord17 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 appendResult24 = (float2(( ( _DistortionNoiseTiling * uv_TexCoord17 * uv2_TexCoord36.y ).x + ( _Time.y * _DistortionNoiseScrollSpeedU ) + uv2_TexCoord36.x ) , ( ( _DistortionNoiseTiling * uv_TexCoord17 * uv2_TexCoord36.y ).y + ( _Time.y * _DistortionNoiseScrollSpeedV ) + uv2_TexCoord36.x )));
			float2 appendResult78 = (float2(( ( _Noise01Tiling * uv_TexCoord17 * uv2_TexCoord36.y ).x + ( _Time.y * _Noise01ScrollSpeedU ) + uv2_TexCoord36.x ) , ( ( _Noise01Tiling * uv_TexCoord17 * uv2_TexCoord36.y ).y + ( _Time.y * _Noise01ScrollSpeedV ) + uv2_TexCoord36.x )));
			float clampResult52 = clamp( ( tex2D( _Noise01, appendResult78 ).r + ( _Noise01Power * uv2_TexCoord36.z ) ) , 0.0 , 1.0 );
			float temp_output_41_0 = ( tex2D( _EmbersTexture, ( appendResult5 + ( (-1.0 + (tex2D( _DistortionNoise, appendResult24 ).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _DistortionNoisePower ) ) ).r * i.vertexColor.a * clampResult52 );
			float ResultMask68 = temp_output_41_0;
			float clampResult67 = clamp( ( _RampMultiplyTiling * ResultMask68 ) , 0.0 , 1.0 );
			#ifdef _RAMPFLIP_ON
				float staticSwitch61 = ( 1.0 - clampResult67 );
			#else
				float staticSwitch61 = clampResult67;
			#endif
			float2 appendResult62 = (float2(staticSwitch61 , 0.0));
			o.Emission = ( _RampColorTint * _FinalPower * tex2D( _Ramp, appendResult62 ) * i.vertexColor * tex2D( _RampMask, appendResult62 ).r ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth81 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth81 = abs( ( screenDepth81 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _SoftParticleValue ) );
			float clampResult83 = clamp( distanceDepth81 , 0.0 , 1.0 );
			float clampResult43 = clamp( ( temp_output_41_0 * _OpacityPower * clampResult83 ) , 0.0 , 1.0 );
			o.Alpha = clampResult43;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;1399.551;13.6246;1.221954;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-3286.452,498.6562;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;34;-3299.22,408.1044;Float;False;Property;_DistortionNoiseTiling;Distortion Noise Tiling;14;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-3282.509,620.6843;Float;False;1;-1;3;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;22;-3083.303,983.3493;Float;False;Property;_DistortionNoiseScrollSpeedU;Distortion Noise Scroll Speed U;15;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;16;-2991.911,840.0518;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;23;-3082.303,1062.35;Float;False;Property;_DistortionNoiseScrollSpeedV;Distortion Noise Scroll Speed V;16;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2975.02,500.2038;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT2;0;False;2;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2658.219,974.0335;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-2659.206,856.2919;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;76;-3240.24,766.3961;Float;False;Property;_Noise01Tiling;Noise 01 Tiling;9;0;1.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;35;-2822.62,496.2043;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WireNode;44;-2966.263,220.444;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;72;-3027.968,1233.41;Float;False;Property;_Noise01ScrollSpeedV;Noise 01 Scroll Speed V;11;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-2250.215,604.0215;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-2248.751,479.6834;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-2972.494,695.6555;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT2;0;False;2;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;71;-3027.89,1139.112;Float;False;Property;_Noise01ScrollSpeedU;Noise 01 Scroll Speed U;10;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-2654.523,1096.399;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-1817.518,214.6583;Float;False;Constant;_Float2;Float 2;1;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;77;-2827.24,695.3961;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-2654.524,1199.599;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;24;-2089.698,519.359;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.FloorOpNode;6;-1772,346;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1893.622,84.75056;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-2252.32,1043.091;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1610.518,175.6583;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-1849.637,493.6918;Float;True;Property;_DistortionNoise;Distortion Noise;13;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1610.741,279.6469;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;73;-2255.811,863.8755;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;26;-1632.748,704.3555;Float;False;Property;_DistortionNoisePower;Distortion Noise Power;17;0;0.25;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;39;-1526.826,522.1783;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-1.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;50;-1103.621,827.2776;Float;False;Property;_Noise01Power;Noise 01 Power;12;0;0.25;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.WireNode;55;-2907.669,1815.692;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-1440,216;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;78;-2088.117,962.233;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.DynamicAppendNode;5;-1244,129;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;45;-928.9998,623.2382;Float;True;Property;_Noise01;Noise 01;8;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1295.259,558.7211;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-785.4265,827.8869;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-592.8379,704.559;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-896.4346,238.8525;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.ClampOpNode;52;-453.6541,700.559;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;28;-614.8504,238.5999;Float;True;Property;_EmbersTexture;Embers Texture;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;40;-494.5864,429.7433;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-184.7126,376.6164;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;57;-1612.265,-457.1555;Float;False;Property;_RampMultiplyTiling;Ramp Multiply Tiling;6;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;56;-1583.44,-312.9178;Float;False;68;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;68;16.29496,303.81;Float;False;ResultMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1346.265,-379.1555;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;67;-1194.265,-384.1555;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;82;-447.6482,868.6265;Float;False;Property;_SoftParticleValue;Soft Particle Value;18;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;59;-1033.188,-491.3585;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DepthFade;81;-217.9207,869.8475;Float;False;True;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;61;-856.6082,-413.6473;Float;False;Property;_RampFlip;Ramp Flip;7;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;60;-779.8391,-307.7366;Float;False;Constant;_Float3;Float 3;12;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;83;-7.74538,867.4039;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;-304.731,498.2407;Float;False;Property;_OpacityPower;Opacity Power;1;0;1;0;4;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;62;-583.5042,-371.8015;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ColorNode;63;-314.6573,-672.2866;Float;False;Property;_RampColorTint;Ramp Color Tint;5;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;64;-395.9894,-402.533;Float;True;Property;_Ramp;Ramp;3;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Ramps/Ramp14.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;65;-378.7344,-494.0837;Float;False;Property;_FinalPower;Final Power;0;0;2;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;16.64405,417.3462;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;80;-393.1755,-211.0426;Float;True;Property;_RampMask;Ramp Mask;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-29.73328,-512.8838;Float;False;5;5;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ClampOpNode;43;171.9736,406.8077;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;494.2687,32.23762;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/BetterEmbers;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;34;0
WireConnection;33;1;17;0
WireConnection;33;2;36;2
WireConnection;21;0;16;2
WireConnection;21;1;23;0
WireConnection;20;0;16;2
WireConnection;20;1;22;0
WireConnection;35;0;33;0
WireConnection;44;0;36;1
WireConnection;19;0;35;1
WireConnection;19;1;21;0
WireConnection;19;2;36;1
WireConnection;18;0;35;0
WireConnection;18;1;20;0
WireConnection;18;2;36;1
WireConnection;75;0;76;0
WireConnection;75;1;17;0
WireConnection;75;2;36;2
WireConnection;69;0;16;2
WireConnection;69;1;71;0
WireConnection;77;0;75;0
WireConnection;70;0;16;2
WireConnection;70;1;72;0
WireConnection;24;0;18;0
WireConnection;24;1;19;0
WireConnection;6;0;44;0
WireConnection;74;0;77;1
WireConnection;74;1;70;0
WireConnection;74;2;36;1
WireConnection;10;0;3;2
WireConnection;10;1;11;0
WireConnection;15;1;24;0
WireConnection;13;0;11;0
WireConnection;13;1;6;0
WireConnection;73;0;77;0
WireConnection;73;1;69;0
WireConnection;73;2;36;1
WireConnection;39;0;15;1
WireConnection;55;0;36;3
WireConnection;4;0;10;0
WireConnection;4;1;13;0
WireConnection;78;0;73;0
WireConnection;78;1;74;0
WireConnection;5;0;3;1
WireConnection;5;1;4;0
WireConnection;45;1;78;0
WireConnection;25;0;39;0
WireConnection;25;1;26;0
WireConnection;53;0;50;0
WireConnection;53;1;55;0
WireConnection;51;0;45;1
WireConnection;51;1;53;0
WireConnection;14;0;5;0
WireConnection;14;1;25;0
WireConnection;52;0;51;0
WireConnection;28;1;14;0
WireConnection;41;0;28;1
WireConnection;41;1;40;4
WireConnection;41;2;52;0
WireConnection;68;0;41;0
WireConnection;58;0;57;0
WireConnection;58;1;56;0
WireConnection;67;0;58;0
WireConnection;59;0;67;0
WireConnection;81;0;82;0
WireConnection;61;0;59;0
WireConnection;61;1;67;0
WireConnection;83;0;81;0
WireConnection;62;0;61;0
WireConnection;62;1;60;0
WireConnection;64;1;62;0
WireConnection;79;0;41;0
WireConnection;79;1;42;0
WireConnection;79;2;83;0
WireConnection;80;1;62;0
WireConnection;66;0;63;0
WireConnection;66;1;65;0
WireConnection;66;2;64;0
WireConnection;66;3;40;0
WireConnection;66;4;80;1
WireConnection;43;0;79;0
WireConnection;0;2;66;0
WireConnection;0;9;43;0
ASEEND*/
//CHKSM=4588D380256A4F492461C3E7813F081D61E4A09F