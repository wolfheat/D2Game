// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/GroundPatternAura"
{
	Properties
	{
		_FinalPower("Final Power", Range( 0 , 20)) = 5
		[Toggle]_MaskConstantThickness("Mask Constant Thickness", Int) = 0
		_MaskThickness("Mask Thickness", Float) = 1
		_MaskDistance("Mask Distance", Float) = 1
		_MaskMultiply("Mask Multiply", Range( 0 , 4)) = 1
		_MaskExp("Mask Exp", Range( 0.2 , 10)) = 1
		[Toggle]_MaskTextureEnabled("Mask Texture Enabled", Int) = 1
		_MaskTexture("Mask Texture", 2D) = "white" {}
		_MaskTextureTiling("Mask Texture Tiling", Float) = 2
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		_RampMultiplyTiling("Ramp Multiply Tiling", Float) = 1
		[Toggle]_RampFlip("Ramp Flip", Int) = 0
		_RadialNoise01("Radial Noise 01", 2D) = "white" {}
		_RadialNoise01TilingU("Radial Noise 01 Tiling U", Float) = 1
		_RadialNoise01TilingV("Radial Noise 01 Tiling V", Float) = 4
		_RadialNoise01ScrollSpeed("Radial Noise 01 Scroll Speed", Float) = -0.25
		_RadialNoise01Add("Radial Noise 01 Add", Range( 0 , 1)) = 0
		_RadialNoiseDistortion("Radial Noise Distortion", 2D) = "white" {}
		_RadialNoiseDistortionTilingU("Radial Noise Distortion Tiling U", Float) = 1
		_RadialNoiseDistortionTilingV("Radial Noise Distortion Tiling V", Float) = 2
		_RadialNoiseDistortionScrollSpeed("Radial Noise Distortion Scroll Speed", Float) = -0.15
		_RadialNoiseDistortionPower("Radial Noise Distortion Power", Range( 0 , 2)) = 0.25
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _MASKCONSTANTTHICKNESS_ON
		#pragma shader_feature _MASKTEXTUREENABLED_ON
		#pragma shader_feature _RAMPFLIP_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _RampColorTint;
		uniform float _FinalPower;
		uniform sampler2D _Ramp;
		uniform float _RampMultiplyTiling;
		uniform sampler2D _MaskTexture;
		uniform float4 _AuraSourcePosition;
		uniform float _MaskDistance;
		uniform sampler2D _RadialNoiseDistortion;
		uniform float _RadialNoiseDistortionTilingV;
		uniform float _RadialNoiseDistortionTilingU;
		uniform float _RadialNoiseDistortionScrollSpeed;
		uniform float _RadialNoiseDistortionPower;
		uniform float _MaskThickness;
		uniform float _MaskExp;
		uniform float _MaskMultiply;
		uniform float _MaskTextureTiling;
		uniform sampler2D _RadialNoise01;
		uniform float _RadialNoise01ScrollSpeed;
		uniform float _RadialNoise01TilingU;
		uniform float _RadialNoise01TilingV;
		uniform float _RadialNoise01Add;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 appendResult17 = (float3(_AuraSourcePosition.x , _AuraSourcePosition.y , _AuraSourcePosition.z));
			float3 ASP178 = appendResult17;
			float2 appendResult231 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult235 = (float2(ASP178.x , ASP178.z));
			float2 appendResult182 = (float2(distance( ASP178 , ase_worldPos ) , (0.0 + (atan2( ( appendResult231 - appendResult235 ).x , ( appendResult231 - appendResult235 ).y ) - ( -1.0 * UNITY_PI )) * (1.0 - 0.0) / (UNITY_PI - ( -1.0 * UNITY_PI )))));
			float2 FinalRadialUVs191 = appendResult182;
			float2 appendResult210 = (float2((0.0 + (FinalRadialUVs191.y - 0.0) * (_RadialNoiseDistortionTilingV - 0.0) / (1.0 - 0.0)) , ( ( FinalRadialUVs191.x * _RadialNoiseDistortionTilingU ) + ( _Time.y * _RadialNoiseDistortionScrollSpeed ) )));
			float NoiseResult116 = ( tex2D( _RadialNoiseDistortion, appendResult210 ).r * _RadialNoiseDistortionPower );
			float temp_output_18_0 = ( -distance( ase_worldPos , ASP178 ) + _MaskDistance + NoiseResult116 );
			float clampResult125 = clamp( (0.0 + (temp_output_18_0 - 0.0) * (1.0 - 0.0) / (( _MaskThickness + NoiseResult116 ) - 0.0)) , 0.0 , 1.0 );
			float clampResult6 = clamp( (0.0 + (temp_output_18_0 - 0.0) * (1.0 - 0.0) / (( -_MaskDistance + NoiseResult116 ) - 0.0)) , 0.0 , 1.0 );
			#ifdef _MASKCONSTANTTHICKNESS_ON
				float staticSwitch51 = clampResult125;
			#else
				float staticSwitch51 = clampResult6;
			#endif
			float clampResult28 = clamp( ( ( 1.0 - pow( ( 1.0 - staticSwitch51 ) , _MaskExp ) ) * _MaskMultiply ) , 0.0 , 1.0 );
			float2 appendResult37 = (float2(clampResult28 , (0.0 + (FinalRadialUVs191.y - 0.0) * (_MaskTextureTiling - 0.0) / (1.0 - 0.0))));
			#ifdef _MASKTEXTUREENABLED_ON
				float staticSwitch50 = tex2D( _MaskTexture, appendResult37 ).r;
			#else
				float staticSwitch50 = clampResult28;
			#endif
			float FinalMask53 = staticSwitch50;
			float2 appendResult209 = (float2(( ( _Time.y * _RadialNoise01ScrollSpeed ) + ( _RadialNoise01TilingU * FinalRadialUVs191.x ) ) , (0.0 + (FinalRadialUVs191.y - 0.0) * (_RadialNoise01TilingV - 0.0) / (1.0 - 0.0))));
			float clampResult250 = clamp( ( tex2D( _RadialNoise01, appendResult209 ).r + _RadialNoise01Add ) , 0.0 , 1.0 );
			float NoiseResultEmission245 = clampResult250;
			float clampResult45 = clamp( ( _RampMultiplyTiling * FinalMask53 * NoiseResultEmission245 ) , 0.0 , 1.0 );
			#ifdef _RAMPFLIP_ON
				float staticSwitch49 = ( 1.0 - clampResult45 );
			#else
				float staticSwitch49 = clampResult45;
			#endif
			float2 appendResult46 = (float2(staticSwitch49 , 0.0));
			o.Emission = ( _RampColorTint * _FinalPower * tex2D( _Ramp, appendResult46 ) ).rgb;
			o.Alpha = ( NoiseResultEmission245 * staticSwitch50 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;4293.356;1766.731;1.144809;True;False
Node;AmplifyShaderEditor.Vector4Node;16;-3710.548,-1538.842;Float;False;Global;_AuraSourcePosition;_AuraSourcePosition;0;0;0,0,0,0;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;17;-3435.349,-1510.762;Float;False;FLOAT3;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.RegisterLocalVarNode;178;-3277.045,-1518.952;Float;False;ASP;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.GetLocalVarNode;233;-4425.111,-1098.881;Float;False;178;0;1;FLOAT3
Node;AmplifyShaderEditor.WorldPosInputsNode;230;-4168.952,-1255.052;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;234;-4218.23,-1098.138;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;231;-3904.891,-1219.142;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.DynamicAppendNode;235;-3900.975,-1094.268;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;236;-3659.365,-1165.932;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.BreakToComponentsNode;156;-3378.353,-1168.954;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;183;-3073.71,-635.9186;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;180;-3067.932,-742.725;Float;False;178;0;1;FLOAT3
Node;AmplifyShaderEditor.ATan2OpNode;154;-3111.119,-1171.801;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.PiNode;176;-3170.296,-1050.385;Float;False;1;0;FLOAT;-1.0;False;1;FLOAT
Node;AmplifyShaderEditor.PiNode;177;-3169.079,-973.7508;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;184;-2794.178,-718.9055;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;175;-2889.854,-1077.738;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;182;-2518.637,-904.6594;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.GetLocalVarNode;193;-6385.258,728.51;Float;False;191;0;1;FLOAT2
Node;AmplifyShaderEditor.RegisterLocalVarNode;191;-2374.004,-908.2232;Float;False;FinalRadialUVs;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;205;-6069.025,898.1688;Float;False;Property;_RadialNoiseDistortionTilingU;Radial Noise Distortion Tiling U;20;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;195;-6023.323,757.0835;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;201;-6107.937,1142.792;Float;False;Property;_RadialNoiseDistortionScrollSpeed;Radial Noise Distortion Scroll Speed;22;0;-0.15;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;199;-5976.729,988.7597;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;-5636.449,849.0513;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-5620.791,1033.051;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;208;-5662.316,756.4655;Float;False;Property;_RadialNoiseDistortionTilingV;Radial Noise Distortion Tiling V;21;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;203;-5291.794,886.9857;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;207;-5284.656,698.3983;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;210;-4977.826,793.2402;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-2204.315,-8.66721;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;212;-4721.778,728.5659;Float;True;Property;_RadialNoiseDistortion;Radial Noise Distortion;19;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Noises/NoiseOffset01.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;215;-4713.228,928.8906;Float;False;Property;_RadialNoiseDistortionPower;Radial Noise Distortion Power;23;0;0.25;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;179;-2193.871,135.9816;Float;False;178;0;1;FLOAT3
Node;AmplifyShaderEditor.DistanceOpNode;1;-1909.316,52.33274;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;-4345.708,792.7782;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;19;-1784.543,167.6551;Float;False;Property;_MaskDistance;Mask Distance;3;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;-4146.95,790.5504;Float;False;NoiseResult;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;131;-1794.097,-131.4298;Float;False;Property;_MaskThickness;Mask Thickness;2;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.NegateNode;133;-1739.854,51.93799;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.NegateNode;242;-1589.937,367.4593;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;117;-1792.946,261.0985;Float;False;116;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-1490.543,92.65519;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;115;-1485.976,233.6826;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;132;-1486.097,-18.42981;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1322.375,146.794;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;130;-1319.097,-41.42984;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;6;-1127.429,130.68;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;125;-1127.547,-4.22699;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;51;-946.3127,5.138;Float;False;Property;_MaskConstantThickness;Mask Constant Thickness;1;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;35;-595.9975,106.2657;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-716.9952,206.7743;Float;False;Property;_MaskExp;Mask Exp;6;0;1;0.2;10;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;192;-6360.644,222.3861;Float;False;191;0;1;FLOAT2
Node;AmplifyShaderEditor.TimeNode;186;-5948.441,-203.9894;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;194;-6000.616,203.3369;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;190;-5999.917,46.40808;Float;False;Property;_RadialNoise01TilingU;Radial Noise 01 Tiling U;15;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;188;-6027.678,-46.38335;Float;False;Property;_RadialNoise01ScrollSpeed;Radial Noise 01 Scroll Speed;17;0;-0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;23;-382.0311,168.0194;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;237;-656.6316,449.7931;Float;False;191;0;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;26;-367.5374,276.0275;Float;False;Property;_MaskMultiply;Mask Multiply;5;0;1;0;4;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;181;-5629.116,183.7976;Float;False;Property;_RadialNoise01TilingV;Radial Noise 01 Tiling V;16;0;4;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;36;-214.1002,173.2545;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;-5632.428,79.05439;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;-5632.961,-135.1057;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;198;-5282.67,13.38163;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;206;-5261.699,117.7229;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;240;-255.9945,555.0403;Float;False;Property;_MaskTextureTiling;Mask Texture Tiling;9;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;238;-277.8575,454.9111;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-38.88911,224.8188;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;209;-4954.401,66.85846;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ClampOpNode;28;127.1131,278.2716;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;239;109.874,424.1991;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;243;-4707.744,242.3066;Float;False;Property;_RadialNoise01Add;Radial Noise 01 Add;18;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;37;345.9705,330.4571;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;211;-4722.704,41.4527;Float;True;Property;_RadialNoise01;Radial Noise 01;14;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Noises/NoiseSmooth01.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;40;521.6888,303.522;Float;True;Property;_MaskTexture;Mask Texture;8;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WireNode;52;502.7022,556.324;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;249;-4350.325,129.9854;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;50;1067.811,410.6607;Float;False;Property;_MaskTextureEnabled;Mask Texture Enabled;7;0;0;True;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;250;-4216.325,130.9854;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;43;-147.8049,-258.483;Float;False;Property;_RampMultiplyTiling;Ramp Multiply Tiling;12;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;54;-129.9801,-129.2452;Float;False;53;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;245;-4053.335,130.9653;Float;False;NoiseResultEmission;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;246;-198.7837,-29.79752;Float;False;245;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;1440.737,466.5699;Float;False;FinalMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;118.195,-180.483;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;45;270.1952,-185.483;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;48;416.2719,-298.6859;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;55;684.6213,-109.0641;Float;False;Constant;_Float1;Float 1;12;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;49;607.8522,-214.9748;Float;False;Property;_RampFlip;Ramp Flip;13;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;46;880.9562,-173.1289;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;42;1085.726,-295.4112;Float;False;Property;_FinalPower;Final Power;0;0;5;0;20;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;39;1068.471,-203.8604;Float;True;Property;_Ramp;Ramp;10;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;248;1121.152,240.0599;Float;False;245;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;47;1149.803,-473.614;Float;False;Property;_RampColorTint;Ramp Color Tint;11;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;247;1458.93,306.4937;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1463.574,-322.7592;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;33;1873.747,-14.53287;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/GroundPatternAura;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Custom;0.5;True;False;0;True;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;4;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;16;1
WireConnection;17;1;16;2
WireConnection;17;2;16;3
WireConnection;178;0;17;0
WireConnection;234;0;233;0
WireConnection;231;0;230;1
WireConnection;231;1;230;3
WireConnection;235;0;234;0
WireConnection;235;1;234;2
WireConnection;236;0;231;0
WireConnection;236;1;235;0
WireConnection;156;0;236;0
WireConnection;154;0;156;0
WireConnection;154;1;156;1
WireConnection;184;0;180;0
WireConnection;184;1;183;0
WireConnection;175;0;154;0
WireConnection;175;1;176;0
WireConnection;175;2;177;0
WireConnection;182;0;184;0
WireConnection;182;1;175;0
WireConnection;191;0;182;0
WireConnection;195;0;193;0
WireConnection;204;0;195;0
WireConnection;204;1;205;0
WireConnection;202;0;199;2
WireConnection;202;1;201;0
WireConnection;203;0;204;0
WireConnection;203;1;202;0
WireConnection;207;0;195;1
WireConnection;207;4;208;0
WireConnection;210;0;207;0
WireConnection;210;1;203;0
WireConnection;212;1;210;0
WireConnection;1;0;2;0
WireConnection;1;1;179;0
WireConnection;214;0;212;1
WireConnection;214;1;215;0
WireConnection;116;0;214;0
WireConnection;133;0;1;0
WireConnection;242;0;19;0
WireConnection;18;0;133;0
WireConnection;18;1;19;0
WireConnection;18;2;117;0
WireConnection;115;0;242;0
WireConnection;115;1;117;0
WireConnection;132;0;131;0
WireConnection;132;1;117;0
WireConnection;22;0;18;0
WireConnection;22;2;115;0
WireConnection;130;0;18;0
WireConnection;130;2;132;0
WireConnection;6;0;22;0
WireConnection;125;0;130;0
WireConnection;51;0;125;0
WireConnection;51;1;6;0
WireConnection;35;0;51;0
WireConnection;194;0;192;0
WireConnection;23;0;35;0
WireConnection;23;1;24;0
WireConnection;36;0;23;0
WireConnection;189;0;190;0
WireConnection;189;1;194;0
WireConnection;187;0;186;2
WireConnection;187;1;188;0
WireConnection;198;0;187;0
WireConnection;198;1;189;0
WireConnection;206;0;194;1
WireConnection;206;4;181;0
WireConnection;238;0;237;0
WireConnection;25;0;36;0
WireConnection;25;1;26;0
WireConnection;209;0;198;0
WireConnection;209;1;206;0
WireConnection;28;0;25;0
WireConnection;239;0;238;1
WireConnection;239;4;240;0
WireConnection;37;0;28;0
WireConnection;37;1;239;0
WireConnection;211;1;209;0
WireConnection;40;1;37;0
WireConnection;52;0;28;0
WireConnection;249;0;211;1
WireConnection;249;1;243;0
WireConnection;50;0;40;1
WireConnection;50;1;52;0
WireConnection;250;0;249;0
WireConnection;245;0;250;0
WireConnection;53;0;50;0
WireConnection;44;0;43;0
WireConnection;44;1;54;0
WireConnection;44;2;246;0
WireConnection;45;0;44;0
WireConnection;48;0;45;0
WireConnection;49;0;48;0
WireConnection;49;1;45;0
WireConnection;46;0;49;0
WireConnection;46;1;55;0
WireConnection;39;1;46;0
WireConnection;247;0;248;0
WireConnection;247;1;50;0
WireConnection;41;0;47;0
WireConnection;41;1;42;0
WireConnection;41;2;39;0
WireConnection;33;2;41;0
WireConnection;33;9;247;0
ASEEND*/
//CHKSM=6F20B20E569C1A2D73D5F9AF5945D9CFD529BCC4