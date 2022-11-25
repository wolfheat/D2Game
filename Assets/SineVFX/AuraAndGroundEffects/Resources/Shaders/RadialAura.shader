// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/RadialAura"
{
	Properties
	{
		_FinalPower("Final Power", Range( 0 , 10)) = 2
		[Toggle]_MaskConstantThickness("Mask Constant Thickness", Int) = 0
		_MaskThickness("Mask Thickness", Float) = 1
		_MaskDistance("Mask Distance", Float) = 1
		_MaskMultiply("Mask Multiply", Range( 0 , 4)) = 1
		_MaskExp("Mask Exp", Range( 0.2 , 10)) = 1
		[Toggle]_MaskTextureEnabled("Mask Texture Enabled", Int) = 1
		_MaskTexture("Mask Texture", 2D) = "white" {}
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		_RampMultiplyTiling("Ramp Multiply Tiling", Float) = 1
		[Toggle]_RampFlip("Ramp Flip", Int) = 0
		_FOVFix("FOV Fix", Float) = 1.05
		_RadialNoise01("Radial Noise 01", 2D) = "white" {}
		_RadialNoise01TilingU("Radial Noise 01 Tiling U", Float) = 1
		_RadialNoise01TilingV("Radial Noise 01 Tiling V", Float) = 4
		_RadialNoise01ScrollSpeed("Radial Noise 01 Scroll Speed", Float) = -0.25
		_RadialNoise01Power("Radial Noise 01 Power", Range( 0 , 2)) = 0.5
		[Toggle]_RadialNoiseDistortionU("Radial Noise Distortion U", Int) = 1
		[Toggle]_RadialNoiseDistortionV("Radial Noise Distortion V", Int) = 1
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
		#pragma shader_feature _RADIALNOISEDISTORTIONU_ON
		#pragma shader_feature _RADIALNOISEDISTORTIONV_ON
		#pragma shader_feature _MASKCONSTANTTHICKNESS_ON
		#pragma shader_feature _MASKTEXTUREENABLED_ON
		#pragma shader_feature _RAMPFLIP_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform float4 _RampColorTint;
		uniform float _FinalPower;
		uniform sampler2D _Ramp;
		uniform float _RampMultiplyTiling;
		uniform sampler2D _MaskTexture;
		uniform float4 _AuraSourcePosition;
		uniform float _MaskDistance;
		uniform sampler2D _RadialNoise01;
		uniform float _RadialNoise01ScrollSpeed;
		uniform float _RadialNoise01TilingU;
		uniform float _FOVFix;
		uniform float _RadialNoise01TilingV;
		uniform sampler2D _RadialNoiseDistortion;
		uniform float _RadialNoiseDistortionTilingV;
		uniform float _RadialNoiseDistortionTilingU;
		uniform float _RadialNoiseDistortionScrollSpeed;
		uniform float _RadialNoiseDistortionPower;
		uniform float _RadialNoise01Power;
		uniform float _MaskThickness;
		uniform float _MaskExp;
		uniform float _MaskMultiply;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 appendResult17 = (float3(_AuraSourcePosition.x , _AuraSourcePosition.y , _AuraSourcePosition.z));
			float3 ASP178 = appendResult17;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos173 = ase_screenPos;
			#if UNITY_UV_STARTS_AT_TOP
			float scale173 = -1.0;
			#else
			float scale173 = 1.0;
			#endif
			float halfPosW173 = ase_screenPos173.w * 0.5;
			ase_screenPos173.y = ( ase_screenPos173.y - halfPosW173 ) * _ProjectionParams.x* scale173 + halfPosW173;
			ase_screenPos173.xyzw /= ase_screenPos173.w;
			float AspectRatio158 = ( _ScreenParams.x / _ScreenParams.y );
			float2 appendResult171 = (float2(( ( ( ase_screenPos173.r * 2.0 ) - 1.0 ) * AspectRatio158 ) , ( ( ase_screenPos173.g * 2.0 ) - 1.0 )));
			float2 FinalScreenUVs172 = appendResult171;
			float2 appendResult145 = (float2(mul( UNITY_MATRIX_V, float4( ( _WorldSpaceCameraPos - ASP178 ) , 0.0 ) ).xyz.x , mul( UNITY_MATRIX_V, float4( ( _WorldSpaceCameraPos - ASP178 ) , 0.0 ) ).xyz.y));
			float2 appendResult182 = (float2(distance( ASP178 , ase_worldPos ) , (0.0 + (atan2( ( FinalScreenUVs172 + ( AspectRatio158 * ( appendResult145 / mul( UNITY_MATRIX_V, float4( ( _WorldSpaceCameraPos - ASP178 ) , 0.0 ) ).xyz.z ) * _FOVFix ) ).x , ( FinalScreenUVs172 + ( AspectRatio158 * ( appendResult145 / mul( UNITY_MATRIX_V, float4( ( _WorldSpaceCameraPos - ASP178 ) , 0.0 ) ).xyz.z ) * _FOVFix ) ).y ) - ( -1.0 * UNITY_PI )) * (1.0 - 0.0) / (UNITY_PI - ( -1.0 * UNITY_PI )))));
			float2 FinalRadialUVs191 = appendResult182;
			float2 appendResult209 = (float2(( ( _Time.y * _RadialNoise01ScrollSpeed ) + ( _RadialNoise01TilingU * FinalRadialUVs191.x ) ) , (0.0 + (FinalRadialUVs191.y - 0.0) * (_RadialNoise01TilingV - 0.0) / (1.0 - 0.0))));
			float2 appendResult210 = (float2((0.0 + (FinalRadialUVs191.y - 0.0) * (_RadialNoiseDistortionTilingV - 0.0) / (1.0 - 0.0)) , ( ( FinalRadialUVs191.x * _RadialNoiseDistortionTilingU ) + ( _Time.y * _RadialNoiseDistortionScrollSpeed ) )));
			#ifdef _RADIALNOISEDISTORTIONU_ON
				float staticSwitch223 = 1.0;
			#else
				float staticSwitch223 = 0.0;
			#endif
			#ifdef _RADIALNOISEDISTORTIONV_ON
				float staticSwitch228 = 1.0;
			#else
				float staticSwitch228 = 0.0;
			#endif
			float2 appendResult225 = (float2(staticSwitch223 , staticSwitch228));
			float NoiseResult116 = ( tex2D( _RadialNoise01, ( appendResult209 + ( ( tex2D( _RadialNoiseDistortion, appendResult210 ).r * _RadialNoiseDistortionPower ) * appendResult225 ) ) ).r * _RadialNoise01Power );
			float temp_output_18_0 = ( -distance( ase_worldPos , ASP178 ) + _MaskDistance + NoiseResult116 );
			float clampResult125 = clamp( (0.0 + (temp_output_18_0 - 0.0) * (1.0 - 0.0) / (( _MaskThickness + NoiseResult116 ) - 0.0)) , 0.0 , 1.0 );
			float clampResult6 = clamp( (0.0 + (temp_output_18_0 - 0.0) * (1.0 - 0.0) / (( _MaskDistance + NoiseResult116 ) - 0.0)) , 0.0 , 1.0 );
			#ifdef _MASKCONSTANTTHICKNESS_ON
				float staticSwitch51 = clampResult125;
			#else
				float staticSwitch51 = clampResult6;
			#endif
			float clampResult28 = clamp( ( ( 1.0 - pow( ( 1.0 - staticSwitch51 ) , _MaskExp ) ) * _MaskMultiply ) , 0.0 , 1.0 );
			float2 appendResult37 = (float2(clampResult28 , 0.0));
			#ifdef _MASKTEXTUREENABLED_ON
				float staticSwitch50 = tex2D( _MaskTexture, appendResult37 ).r;
			#else
				float staticSwitch50 = clampResult28;
			#endif
			float FinalMask53 = staticSwitch50;
			float clampResult45 = clamp( ( _RampMultiplyTiling * FinalMask53 ) , 0.0 , 1.0 );
			#ifdef _RAMPFLIP_ON
				float staticSwitch49 = ( 1.0 - clampResult45 );
			#else
				float staticSwitch49 = clampResult45;
			#endif
			float2 appendResult46 = (float2(staticSwitch49 , 0.0));
			o.Emission = ( _RampColorTint * _FinalPower * tex2D( _Ramp, appendResult46 ) ).rgb;
			o.Alpha = staticSwitch50;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;5502.656;-125.0792;1.411348;True;False
Node;AmplifyShaderEditor.Vector4Node;16;-3710.548,-1538.842;Float;False;Global;_AuraSourcePosition;_AuraSourcePosition;0;0;0,0,0,0;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;17;-3435.349,-1510.762;Float;False;FLOAT3;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;134;-4172.099,-1036.273;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;180;-4083.834,-830.4494;Float;False;178;0;1;FLOAT3
Node;AmplifyShaderEditor.RegisterLocalVarNode;178;-3277.045,-1518.952;Float;False;ASP;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ViewMatrixNode;157;-3792.906,-998.2509;Float;False;0;1;FLOAT4x4
Node;AmplifyShaderEditor.GrabScreenPosition;173;-4824.822,-2185.616;Float;False;0;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenParams;148;-3672.35,-1799.274;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;135;-3810.014,-910.6927;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;162;-4294.423,-2141.415;Float;False;Constant;_Float2;Float 2;26;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;149;-3430.548,-1775.874;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;166;-4289.871,-1899.612;Float;False;Constant;_Float6;Float 6;26;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;161;-4125.42,-2220.713;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;164;-4124.122,-2121.914;Float;False;Constant;_Float3;Float 3;26;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;-3566.329,-967.5073;Float;False;2;2;0;FLOAT4x4;0.0;False;1;FLOAT3;0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.GetLocalVarNode;170;-3961.619,-2123.215;Float;False;158;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;163;-3965.521,-2222.013;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;144;-3380.428,-968.8077;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;-4120.869,-1978.911;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;168;-4119.571,-1880.112;Float;False;Constant;_Float7;Float 7;26;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;158;-3267.33,-1781.016;Float;False;AspectRatio;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;169;-3723.719,-2222.014;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;145;-3047.625,-1009.108;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;167;-3960.969,-1980.211;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;160;-2908.907,-853.2503;Float;False;Property;_FOVFix;FOV Fix;13;0;1.05;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;146;-2870.826,-950.6077;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.DynamicAppendNode;171;-3428.619,-2068.615;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.GetLocalVarNode;159;-2900.907,-1138.251;Float;False;158;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;-2636.829,-1048.108;Float;False;3;3;0;FLOAT;0,0;False;1;FLOAT2;0;False;2;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RegisterLocalVarNode;172;-3283.02,-2067.316;Float;False;FinalScreenUVs;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.GetLocalVarNode;174;-2798.7,-1390.897;Float;False;172;0;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;151;-2375.526,-1163.809;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.BreakToComponentsNode;156;-2241.629,-1166.412;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PiNode;176;-2033.572,-1047.843;Float;False;1;0;FLOAT;-1.0;False;1;FLOAT
Node;AmplifyShaderEditor.PiNode;177;-2032.355,-971.2084;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;183;-4089.612,-723.643;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ATan2OpNode;154;-1974.395,-1169.259;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;184;-3810.08,-806.6299;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;175;-1753.129,-1075.196;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;182;-1388.243,-768.5982;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RegisterLocalVarNode;191;-1243.61,-772.162;Float;False;FinalRadialUVs;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.GetLocalVarNode;193;-6385.258,728.51;Float;False;191;0;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;205;-6069.025,898.1688;Float;False;Property;_RadialNoiseDistortionTilingU;Radial Noise Distortion Tiling U;22;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;199;-5976.729,988.7597;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;201;-6107.937,1142.792;Float;False;Property;_RadialNoiseDistortionScrollSpeed;Radial Noise Distortion Scroll Speed;24;0;-0.15;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;195;-6023.323,757.0835;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;208;-5662.316,756.4655;Float;False;Property;_RadialNoiseDistortionTilingV;Radial Noise Distortion Tiling V;23;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;-5636.449,849.0513;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-5620.791,1033.051;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;192;-6385.258,633.265;Float;False;191;0;1;FLOAT2
Node;AmplifyShaderEditor.TFHCRemapNode;207;-5284.656,698.3983;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;203;-5291.794,886.9857;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;186;-5973.055,206.8895;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;188;-6052.292,364.4956;Float;False;Property;_RadialNoise01ScrollSpeed;Radial Noise 01 Scroll Speed;17;0;-0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;227;-4989.892,1156.551;Float;False;Constant;_Float8;Float 8;26;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;226;-4991.892,1070.551;Float;False;Constant;_Float5;Float 5;26;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;190;-6024.531,457.2872;Float;False;Property;_RadialNoise01TilingU;Radial Noise 01 Tiling U;15;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;210;-4977.826,793.2402;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.BreakToComponentsNode;194;-6025.23,614.2159;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;-5657.042,489.9335;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;223;-4727.077,1050.027;Float;False;Property;_RadialNoiseDistortionU;Radial Noise Distortion U;19;0;0;True;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;212;-4721.778,728.5659;Float;True;Property;_RadialNoiseDistortion;Radial Noise Distortion;21;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Noises/NoiseOffset01.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;215;-4713.228,928.8906;Float;False;Property;_RadialNoiseDistortionPower;Radial Noise Distortion Power;25;0;0.25;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;228;-4721.892,1158.551;Float;False;Property;_RadialNoiseDistortionV;Radial Noise Distortion V;20;0;0;True;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;-5657.575,275.7732;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;181;-5653.73,594.6766;Float;False;Property;_RadialNoise01TilingV;Radial Noise 01 Tiling V;16;0;4;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;225;-4257.898,1105.551;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;198;-5307.284,424.2607;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;-4345.708,792.7782;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;206;-5286.313,528.602;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;209;-4979.015,477.7375;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;224;-3980.448,884.9143;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;213;-3324.84,475.5447;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-2204.315,-8.66721;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;216;-3044.892,534.7984;Float;False;Property;_RadialNoise01Power;Radial Noise 01 Power;18;0;0.5;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;211;-3066.56,337.6728;Float;True;Property;_RadialNoise01;Radial Noise 01;14;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Noises/NoiseSmooth01.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;179;-2193.871,135.9816;Float;False;178;0;1;FLOAT3
Node;AmplifyShaderEditor.DistanceOpNode;1;-1909.316,52.33274;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-2697.028,466.2052;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;19;-1781.543,172.6551;Float;False;Property;_MaskDistance;Mask Distance;3;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.NegateNode;133;-1739.854,51.93799;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;117;-1792.946,261.0985;Float;False;116;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;-2532.25,459.6871;Float;False;NoiseResult;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;131;-1774.097,-89.42984;Float;False;Property;_MaskThickness;Mask Thickness;2;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-1488.543,96.65519;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;115;-1485.976,233.6826;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;132;-1487.097,-18.42981;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;130;-1319.097,-41.42984;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1322.375,146.794;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;6;-1127.429,130.68;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;125;-1127.547,-4.22699;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;51;-946.3127,5.138;Float;False;Property;_MaskConstantThickness;Mask Constant Thickness;1;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-716.9952,206.7743;Float;False;Property;_MaskExp;Mask Exp;6;0;1;0.2;10;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;35;-595.9975,106.2657;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;23;-382.0311,168.0194;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;36;-214.1002,173.2545;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;26;-367.5374,276.0275;Float;False;Property;_MaskMultiply;Mask Multiply;5;0;1;0;4;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-38.88911,224.8188;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;28;127.1131,278.2716;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;222;127.3718,414.122;Float;False;Constant;_Float0;Float 0;26;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;37;345.9705,330.4571;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.WireNode;52;502.7022,556.324;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;40;521.6888,303.522;Float;True;Property;_MaskTexture;Mask Texture;8;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;50;1067.811,410.6607;Float;False;Property;_MaskTextureEnabled;Mask Texture Enabled;7;0;0;True;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;1440.737,466.5699;Float;False;FinalMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;43;-147.8049,-258.483;Float;False;Property;_RampMultiplyTiling;Ramp Multiply Tiling;11;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;54;-118.9801,-114.2452;Float;False;53;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;118.195,-180.483;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;45;270.1952,-185.483;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;48;416.2719,-298.6859;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;55;684.6213,-109.0641;Float;False;Constant;_Float1;Float 1;12;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;49;607.8522,-214.9748;Float;False;Property;_RampFlip;Ramp Flip;12;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;46;880.9562,-173.1289;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ColorNode;47;1149.803,-473.614;Float;False;Property;_RampColorTint;Ramp Color Tint;10;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;1085.726,-295.4112;Float;False;Property;_FinalPower;Final Power;0;0;2;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;39;1068.471,-203.8604;Float;True;Property;_Ramp;Ramp;9;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;150;-4788.243,-2009.884;Float;False;1;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1421.727,-319.4112;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;33;1873.747,-14.53287;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/RadialAura;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Custom;0.5;True;False;0;True;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;4;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;16;1
WireConnection;17;1;16;2
WireConnection;17;2;16;3
WireConnection;178;0;17;0
WireConnection;135;0;134;0
WireConnection;135;1;180;0
WireConnection;149;0;148;1
WireConnection;149;1;148;2
WireConnection;161;0;173;1
WireConnection;161;1;162;0
WireConnection;142;0;157;0
WireConnection;142;1;135;0
WireConnection;163;0;161;0
WireConnection;163;1;164;0
WireConnection;144;0;142;0
WireConnection;165;0;173;2
WireConnection;165;1;166;0
WireConnection;158;0;149;0
WireConnection;169;0;163;0
WireConnection;169;1;170;0
WireConnection;145;0;144;0
WireConnection;145;1;144;1
WireConnection;167;0;165;0
WireConnection;167;1;168;0
WireConnection;146;0;145;0
WireConnection;146;1;144;2
WireConnection;171;0;169;0
WireConnection;171;1;167;0
WireConnection;147;0;159;0
WireConnection;147;1;146;0
WireConnection;147;2;160;0
WireConnection;172;0;171;0
WireConnection;151;0;174;0
WireConnection;151;1;147;0
WireConnection;156;0;151;0
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
WireConnection;207;0;195;1
WireConnection;207;4;208;0
WireConnection;203;0;204;0
WireConnection;203;1;202;0
WireConnection;210;0;207;0
WireConnection;210;1;203;0
WireConnection;194;0;192;0
WireConnection;189;0;190;0
WireConnection;189;1;194;0
WireConnection;223;0;227;0
WireConnection;223;1;226;0
WireConnection;212;1;210;0
WireConnection;228;0;227;0
WireConnection;228;1;226;0
WireConnection;187;0;186;2
WireConnection;187;1;188;0
WireConnection;225;0;223;0
WireConnection;225;1;228;0
WireConnection;198;0;187;0
WireConnection;198;1;189;0
WireConnection;214;0;212;1
WireConnection;214;1;215;0
WireConnection;206;0;194;1
WireConnection;206;4;181;0
WireConnection;209;0;198;0
WireConnection;209;1;206;0
WireConnection;224;0;214;0
WireConnection;224;1;225;0
WireConnection;213;0;209;0
WireConnection;213;1;224;0
WireConnection;211;1;213;0
WireConnection;1;0;2;0
WireConnection;1;1;179;0
WireConnection;217;0;211;1
WireConnection;217;1;216;0
WireConnection;133;0;1;0
WireConnection;116;0;217;0
WireConnection;18;0;133;0
WireConnection;18;1;19;0
WireConnection;18;2;117;0
WireConnection;115;0;19;0
WireConnection;115;1;117;0
WireConnection;132;0;131;0
WireConnection;132;1;117;0
WireConnection;130;0;18;0
WireConnection;130;2;132;0
WireConnection;22;0;18;0
WireConnection;22;2;115;0
WireConnection;6;0;22;0
WireConnection;125;0;130;0
WireConnection;51;0;125;0
WireConnection;51;1;6;0
WireConnection;35;0;51;0
WireConnection;23;0;35;0
WireConnection;23;1;24;0
WireConnection;36;0;23;0
WireConnection;25;0;36;0
WireConnection;25;1;26;0
WireConnection;28;0;25;0
WireConnection;37;0;28;0
WireConnection;37;1;222;0
WireConnection;52;0;28;0
WireConnection;40;1;37;0
WireConnection;50;0;40;1
WireConnection;50;1;52;0
WireConnection;53;0;50;0
WireConnection;44;0;43;0
WireConnection;44;1;54;0
WireConnection;45;0;44;0
WireConnection;48;0;45;0
WireConnection;49;0;48;0
WireConnection;49;1;45;0
WireConnection;46;0;49;0
WireConnection;46;1;55;0
WireConnection;39;1;46;0
WireConnection;41;0;47;0
WireConnection;41;1;42;0
WireConnection;41;2;39;0
WireConnection;33;2;41;0
WireConnection;33;9;50;0
ASEEND*/
//CHKSM=94CF0BB353E8AFED156B9C09A79618B8423805CF