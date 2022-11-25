// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/BasicAura"
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
		_NoiseDistortionPower("Noise Distortion Power", Range( 0 , 10)) = 1
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01Tiling("Noise 01 Tiling", Float) = 1
		_Noise01ScrollSpeed("Noise 01 Scroll Speed", Float) = 0.25
		[Toggle]_Noise02Enabled("Noise 02 Enabled", Int) = 1
		_Noise02("Noise 02", 2D) = "white" {}
		_Noise02Tiling("Noise 02 Tiling", Float) = 1
		_Noise02ScrollSpeed("Noise 02 Scroll Speed", Float) = 0.25
		[Toggle]_NoiseMaskDistortionEnabled("Noise Mask Distortion Enabled", Int) = 1
		_NoiseMaskDistortion("Noise Mask Distortion", 2D) = "white" {}
		_NoiseMaskDistortionPower("Noise Mask Distortion Power", Range( 0 , 2)) = 0.5
		_NoiseMaskDistortionTiling("Noise Mask Distortion Tiling", Float) = 0.5
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
		#pragma shader_feature _NOISEMASKDISTORTIONENABLED_ON
		#pragma shader_feature _NOISE02ENABLED_ON
		#pragma shader_feature _MASKCONSTANTTHICKNESS_ON
		#pragma shader_feature _MASKTEXTUREENABLED_ON
		#pragma shader_feature _RAMPFLIP_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float4 _RampColorTint;
		uniform float _FinalPower;
		uniform sampler2D _Ramp;
		uniform float _RampMultiplyTiling;
		uniform sampler2D _MaskTexture;
		uniform float4 _AuraSourcePosition;
		uniform float _MaskDistance;
		uniform sampler2D _Noise01;
		uniform float _Noise01Tiling;
		uniform float _Noise01ScrollSpeed;
		uniform sampler2D _NoiseMaskDistortion;
		uniform float _NoiseMaskDistortionTiling;
		uniform float _NoiseMaskDistortionPower;
		uniform sampler2D _Noise02;
		uniform float _Noise02Tiling;
		uniform float _Noise02ScrollSpeed;
		uniform float _NoiseDistortionPower;
		uniform float _MaskThickness;
		uniform float _MaskExp;
		uniform float _MaskMultiply;


		inline float4 TriplanarSamplingSF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float tilling, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= projNormal.x + projNormal.y + projNormal.z;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( topTexMap, tilling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( topTexMap, tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( topTexMap, tilling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float3 appendResult17 = (float3(_AuraSourcePosition.x , _AuraSourcePosition.y , _AuraSourcePosition.z));
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 temp_output_96_0 = abs( ase_worldNormal );
			float3 temp_output_103_0 = ( temp_output_96_0 * temp_output_96_0 );
			float4 triplanar118 = TriplanarSamplingSF( _NoiseMaskDistortion, ase_worldPos, ase_worldNormal, 1.0, _NoiseMaskDistortionTiling, 0 );
			float4 temp_cast_1 = (0.0).xxxx;
			#ifdef _NOISEMASKDISTORTIONENABLED_ON
				float4 staticSwitch126 = ( triplanar118 * _NoiseMaskDistortionPower );
			#else
				float4 staticSwitch126 = temp_cast_1;
			#endif
			float2 appendResult61 = (float2(( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch126 ).y , ( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch126 ).z));
			float2 appendResult63 = (float2(( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch126 ).z , ( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch126 ).x));
			float2 appendResult62 = (float2(( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch126 ).x , ( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch126 ).y));
			float3 weightedBlendVar72 = temp_output_103_0;
			float weightedBlend72 = ( weightedBlendVar72.x*tex2D( _Noise01, appendResult61 ).r + weightedBlendVar72.y*tex2D( _Noise01, appendResult63 ).r + weightedBlendVar72.z*tex2D( _Noise01, appendResult62 ).r );
			float2 appendResult99 = (float2(( float4( ( ase_worldPos * _Noise02Tiling ) , 0.0 ) + ( _Time.y * _Noise02ScrollSpeed ) + staticSwitch126 ).y , ( float4( ( ase_worldPos * _Noise02Tiling ) , 0.0 ) + ( _Time.y * _Noise02ScrollSpeed ) + staticSwitch126 ).z));
			float2 appendResult97 = (float2(( float4( ( ase_worldPos * _Noise02Tiling ) , 0.0 ) + ( _Time.y * _Noise02ScrollSpeed ) + staticSwitch126 ).z , ( float4( ( ase_worldPos * _Noise02Tiling ) , 0.0 ) + ( _Time.y * _Noise02ScrollSpeed ) + staticSwitch126 ).x));
			float2 appendResult95 = (float2(( float4( ( ase_worldPos * _Noise02Tiling ) , 0.0 ) + ( _Time.y * _Noise02ScrollSpeed ) + staticSwitch126 ).x , ( float4( ( ase_worldPos * _Noise02Tiling ) , 0.0 ) + ( _Time.y * _Noise02ScrollSpeed ) + staticSwitch126 ).y));
			float3 weightedBlendVar104 = temp_output_103_0;
			float weightedBlend104 = ( weightedBlendVar104.x*tex2D( _Noise02, appendResult99 ).r + weightedBlendVar104.y*tex2D( _Noise02, appendResult97 ).r + weightedBlendVar104.z*tex2D( _Noise02, appendResult95 ).r );
			#ifdef _NOISE02ENABLED_ON
				float staticSwitch128 = weightedBlend104;
			#else
				float staticSwitch128 = 1.0;
			#endif
			float NoiseResult116 = ( weightedBlend72 * staticSwitch128 * _NoiseDistortionPower );
			float temp_output_18_0 = ( -distance( ase_worldPos , appendResult17 ) + _MaskDistance + NoiseResult116 );
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
7;29;1906;1004;2850.854;474.062;1;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;119;-4765.088,936.6506;Float;True;Property;_NoiseMaskDistortion;Noise Mask Distortion;22;0;None;False;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.RangedFloatNode;120;-4831.907,1133.977;Float;False;Property;_NoiseMaskDistortionTiling;Noise Mask Distortion Tiling;24;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;121;-4363.501,1183.234;Float;False;Property;_NoiseMaskDistortionPower;Noise Mask Distortion Power;23;0;0.5;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.TriplanarNode;118;-4427.98,985.9814;Float;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;0;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;8;0;SAMPLER2D;;False;5;FLOAT;1.0;False;1;SAMPLER2D;;False;6;FLOAT;0.0;False;2;SAMPLER2D;;False;7;FLOAT;0.0;False;3;FLOAT;1.0;False;4;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TimeNode;108;-3869.922,1645.363;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;127;-4013.706,1191.542;Float;False;Constant;_Float4;Float 4;23;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;107;-3870.355,1551.526;Float;False;Property;_Noise02Tiling;Noise 02 Tiling;19;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-4012.5,1089.234;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.WorldPosInputsNode;106;-3869.88,1401.393;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;105;-3889.881,1793.863;Float;False;Property;_Noise02ScrollSpeed;Noise 02 Scroll Speed;20;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-3555.83,1461.119;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.StaticSwitch;126;-3796.607,1127.843;Float;False;Property;_NoiseMaskDistortionEnabled;Noise Mask Distortion Enabled;21;0;0;True;True;;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-3551.878,1702.863;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;58;-3908.523,619.2789;Float;False;Property;_Noise01Tiling;Noise 01 Tiling;15;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;89;-3908.09,713.1155;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;60;-3908.048,469.1457;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;92;-3928.049,861.6151;Float;False;Property;_Noise01ScrollSpeed;Noise 01 Scroll Speed;16;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;111;-3353,1577.776;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0,0,0;False;2;FLOAT4;0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-3590.049,770.6151;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-3594.001,528.8714;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.BreakToComponentsNode;112;-3174.403,1579.453;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;94;-2464.6,1184.91;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-3391.17,645.5284;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0,0,0;False;2;FLOAT4;0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.DynamicAppendNode;95;-2675.186,1753.519;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.TexturePropertyNode;98;-2752.132,1852.928;Float;True;Property;_Noise02;Noise 02;18;0;None;False;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.DynamicAppendNode;97;-2676.565,1600.563;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.AbsOpNode;96;-2240.954,1186.177;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.BreakToComponentsNode;82;-3219.382,638.6965;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;99;-2677.945,1446.23;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-2073.252,1180.977;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;100;-2412.656,1605.381;Float;True;Property;_TextureSample3;Texture Sample 3;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;102;-2407.456,1810.781;Float;True;Property;_TextureSample5;Texture Sample 5;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;101;-2412.655,1410.381;Float;True;Property;_TextureSample4;Texture Sample 4;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;61;-2716.114,513.9819;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.DynamicAppendNode;63;-2714.735,668.3152;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.TexturePropertyNode;56;-2790.3,920.6797;Float;True;Property;_Noise01;Noise 01;14;0;None;False;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.DynamicAppendNode;62;-2713.354,821.2708;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;129;-1766.988,1744.942;Float;False;Constant;_Float5;Float 5;24;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SummedBlendNode;104;-1799.054,1597.581;Float;False;5;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector4Node;16;-2428.374,109.0923;Float;False;Global;_AuraSourcePosition;_AuraSourcePosition;0;0;0,0,0,0;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;66;-2445.625,878.5335;Float;True;Property;_TextureSample2;Texture Sample 2;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;65;-2450.825,673.1335;Float;True;Property;_TextureSample1;Texture Sample 1;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;64;-2450.823,478.1336;Float;True;Property;_TextureSample0;Texture Sample 0;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-2204.315,-8.66721;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;17;-2153.175,137.1723;Float;False;FLOAT3;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.SummedBlendNode;72;-1837.224,665.3335;Float;False;5;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;113;-1754.929,1211.806;Float;False;Property;_NoiseDistortionPower;Noise Distortion Power;13;0;1;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;128;-1556.363,1651.299;Float;False;Property;_Noise02Enabled;Noise 02 Enabled;17;0;0;True;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-1118.805,1036.481;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;1;-1909.316,52.33274;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.NegateNode;133;-1739.854,51.93799;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;131;-1774.097,-89.42984;Float;False;Property;_MaskThickness;Mask Thickness;2;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;117;-1792.946,261.0985;Float;False;116;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;19;-1781.543,172.6551;Float;False;Property;_MaskDistance;Mask Distance;3;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;-954.7827,1032.309;Float;False;NoiseResult;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;132;-1487.097,-18.42981;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-1488.543,96.65519;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;115;-1485.976,233.6826;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;130;-1319.097,-41.42984;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1322.375,146.794;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;6;-1127.429,130.68;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;125;-1127.547,-4.22699;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;51;-946.3127,5.138;Float;False;Property;_MaskConstantThickness;Mask Constant Thickness;1;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;35;-595.9975,106.2657;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-716.9952,206.7743;Float;False;Property;_MaskExp;Mask Exp;6;0;1;0.2;10;0;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;23;-382.0311,168.0194;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;26;-367.5374,276.0275;Float;False;Property;_MaskMultiply;Mask Multiply;5;0;1;0;4;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;36;-214.1002,173.2545;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-38.88911,224.8188;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;28;127.1131,278.2716;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;38;125.0246,401.2724;Float;False;Constant;_Float0;Float 0;4;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;37;345.9705,330.4571;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.WireNode;52;502.7022,556.324;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;40;521.6888,303.522;Float;True;Property;_MaskTexture;Mask Texture;8;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;50;1067.811,410.6607;Float;False;Property;_MaskTextureEnabled;Mask Texture Enabled;7;0;0;True;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;1440.737,466.5699;Float;False;FinalMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;54;-118.9801,-114.2452;Float;False;53;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;43;-147.8049,-258.483;Float;False;Property;_RampMultiplyTiling;Ramp Multiply Tiling;11;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;118.195,-180.483;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;45;270.1952,-185.483;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;48;416.2719,-298.6859;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;55;684.6213,-109.0641;Float;False;Constant;_Float1;Float 1;12;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;49;607.8522,-214.9748;Float;False;Property;_RampFlip;Ramp Flip;12;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;46;880.9562,-173.1289;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ColorNode;47;1149.803,-473.614;Float;False;Property;_RampColorTint;Ramp Color Tint;10;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;39;1068.471,-203.8604;Float;True;Property;_Ramp;Ramp;9;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;1085.726,-295.4112;Float;False;Property;_FinalPower;Final Power;0;0;2;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1421.727,-319.4112;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;33;1873.747,-14.53287;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/BasicAura;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Custom;0.5;True;False;0;True;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;4;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;118;0;119;0
WireConnection;118;3;120;0
WireConnection;122;0;118;0
WireConnection;122;1;121;0
WireConnection;109;0;106;0
WireConnection;109;1;107;0
WireConnection;126;0;122;0
WireConnection;126;1;127;0
WireConnection;110;0;108;2
WireConnection;110;1;105;0
WireConnection;111;0;109;0
WireConnection;111;1;110;0
WireConnection;111;2;126;0
WireConnection;91;0;89;2
WireConnection;91;1;92;0
WireConnection;90;0;60;0
WireConnection;90;1;58;0
WireConnection;112;0;111;0
WireConnection;84;0;90;0
WireConnection;84;1;91;0
WireConnection;84;2;126;0
WireConnection;95;0;112;0
WireConnection;95;1;112;1
WireConnection;97;0;112;2
WireConnection;97;1;112;0
WireConnection;96;0;94;0
WireConnection;82;0;84;0
WireConnection;99;0;112;1
WireConnection;99;1;112;2
WireConnection;103;0;96;0
WireConnection;103;1;96;0
WireConnection;100;0;98;0
WireConnection;100;1;97;0
WireConnection;102;0;98;0
WireConnection;102;1;95;0
WireConnection;101;0;98;0
WireConnection;101;1;99;0
WireConnection;61;0;82;1
WireConnection;61;1;82;2
WireConnection;63;0;82;2
WireConnection;63;1;82;0
WireConnection;62;0;82;0
WireConnection;62;1;82;1
WireConnection;104;0;103;0
WireConnection;104;1;101;1
WireConnection;104;2;100;1
WireConnection;104;3;102;1
WireConnection;66;0;56;0
WireConnection;66;1;62;0
WireConnection;65;0;56;0
WireConnection;65;1;63;0
WireConnection;64;0;56;0
WireConnection;64;1;61;0
WireConnection;17;0;16;1
WireConnection;17;1;16;2
WireConnection;17;2;16;3
WireConnection;72;0;103;0
WireConnection;72;1;64;1
WireConnection;72;2;65;1
WireConnection;72;3;66;1
WireConnection;128;0;104;0
WireConnection;128;1;129;0
WireConnection;93;0;72;0
WireConnection;93;1;128;0
WireConnection;93;2;113;0
WireConnection;1;0;2;0
WireConnection;1;1;17;0
WireConnection;133;0;1;0
WireConnection;116;0;93;0
WireConnection;132;0;131;0
WireConnection;132;1;117;0
WireConnection;18;0;133;0
WireConnection;18;1;19;0
WireConnection;18;2;117;0
WireConnection;115;0;19;0
WireConnection;115;1;117;0
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
WireConnection;37;1;38;0
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
//CHKSM=81F733E475F77B88443FEA0E26FFDC382E99D273