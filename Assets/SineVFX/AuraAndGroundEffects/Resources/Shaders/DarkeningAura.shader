// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/DarkeningAura"
{
	Properties
	{
		_MaskDistance("Mask Distance", Float) = 1
		_MaskMultiply("Mask Multiply", Range( 0 , 4)) = 1
		_MaskExp("Mask Exp", Range( 0.2 , 10)) = 1
		[Toggle]_Noise01Enabled("Noise 01 Enabled", Int) = 1
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01Tiling("Noise 01 Tiling", Float) = 1
		_Noise01ScrollSpeed("Noise 01 Scroll Speed", Float) = 0.25
		_NoiseDistortionPower("Noise Distortion Power", Range( 0 , 10)) = 1
		[Toggle]_NoiseMaskDistortionEnabled("Noise Mask Distortion Enabled", Int) = 1
		_NoiseMaskDistortion("Noise Mask Distortion", 2D) = "white" {}
		_NoiseMaskDistortionTiling("Noise Mask Distortion Tiling", Float) = 0.5
		_NoiseMaskDistortionPower("Noise Mask Distortion Power", Range( 0 , 2)) = 0.5
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _NOISEMASKDISTORTIONENABLED_ON
		#pragma shader_feature _NOISE01ENABLED_ON
		#pragma surface surf Unlit keepalpha noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float4 _AuraSourcePosition;
		uniform float _MaskDistance;
		uniform sampler2D _Noise01;
		uniform float _Noise01Tiling;
		uniform float _Noise01ScrollSpeed;
		uniform sampler2D _NoiseMaskDistortion;
		uniform float _NoiseMaskDistortionTiling;
		uniform float _NoiseMaskDistortionPower;
		uniform float _NoiseDistortionPower;
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
			float3 temp_output_64_0 = abs( ase_worldNormal );
			float4 triplanar41 = TriplanarSamplingSF( _NoiseMaskDistortion, ase_worldPos, ase_worldNormal, 1.0, _NoiseMaskDistortionTiling, 0 );
			float4 temp_cast_1 = (0.0).xxxx;
			#ifdef _NOISEMASKDISTORTIONENABLED_ON
				float4 staticSwitch49 = ( triplanar41 * _NoiseMaskDistortionPower );
			#else
				float4 staticSwitch49 = temp_cast_1;
			#endif
			float2 appendResult71 = (float2(( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch49 ).y , ( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch49 ).z));
			float2 appendResult72 = (float2(( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch49 ).z , ( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch49 ).x));
			float2 appendResult74 = (float2(( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch49 ).x , ( float4( ( ase_worldPos * _Noise01Tiling ) , 0.0 ) + ( _Time.y * _Noise01ScrollSpeed ) + staticSwitch49 ).y));
			float3 weightedBlendVar80 = ( temp_output_64_0 * temp_output_64_0 );
			float weightedBlend80 = ( weightedBlendVar80.x*tex2D( _Noise01, appendResult71 ).r + weightedBlendVar80.y*tex2D( _Noise01, appendResult72 ).r + weightedBlendVar80.z*tex2D( _Noise01, appendResult74 ).r );
			float NoiseResult84 = ( weightedBlend80 * _NoiseDistortionPower );
			#ifdef _NOISE01ENABLED_ON
				float staticSwitch87 = NoiseResult84;
			#else
				float staticSwitch87 = 0.0;
			#endif
			float clampResult6 = clamp( (0.0 + (( -distance( ase_worldPos , appendResult17 ) + _MaskDistance + staticSwitch87 ) - 0.0) * (1.0 - 0.0) / (( _MaskDistance + staticSwitch87 ) - 0.0)) , 0.0 , 1.0 );
			float clampResult28 = clamp( ( ( 1.0 - pow( ( 1.0 - clampResult6 ) , _MaskExp ) ) * _MaskMultiply ) , 0.0 , 1.0 );
			o.Alpha = clampResult28;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;2344.296;-61.7359;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;39;-4433.417,1138.223;Float;False;Property;_NoiseMaskDistortionTiling;Noise Mask Distortion Tiling;11;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;38;-4376.596,936.8953;Float;True;Property;_NoiseMaskDistortion;Noise Mask Distortion;10;0;None;False;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.RangedFloatNode;40;-3979.013,1180.48;Float;False;Property;_NoiseMaskDistortionPower;Noise Mask Distortion Power;12;0;0.5;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.TriplanarNode;41;-4039.492,985.2265;Float;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;0;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;8;0;SAMPLER2D;;False;5;FLOAT;1.0;False;1;SAMPLER2D;;False;6;FLOAT;0.0;False;2;SAMPLER2D;;False;7;FLOAT;0.0;False;3;FLOAT;1.0;False;4;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;51;-3533.939,616.2167;Float;False;Property;_Noise01Tiling;Noise 01 Tiling;6;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;52;-3536.506,717.0533;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;53;-3536.464,473.0835;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-3624.013,1088.48;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;43;-3625.219,1190.788;Float;False;Constant;_Float2;Float 2;23;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;54;-3561.465,857.5535;Float;False;Property;_Noise01ScrollSpeed;Noise 01 Scroll Speed;7;0;0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-3222.417,532.8093;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.StaticSwitch;49;-3433.082,1123.053;Float;False;Property;_NoiseMaskDistortionEnabled;Noise Mask Distortion Enabled;9;0;0;True;True;;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-3218.465,774.553;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-2967.103,861.4202;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0,0,0;False;2;FLOAT4;0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.BreakToComponentsNode;65;-2819.807,866.8344;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;59;-2304.679,570.9031;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;72;-2315.161,896.4531;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.AbsOpNode;64;-2081.033,572.1701;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.DynamicAppendNode;74;-2313.78,1049.409;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.DynamicAppendNode;71;-2316.54,742.1198;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.TexturePropertyNode;73;-2390.726,1148.818;Float;True;Property;_Noise01;Noise 01;5;0;None;False;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.SamplerNode;77;-2046.051,1106.671;Float;True;Property;_TextureSample3;Texture Sample 3;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-1913.331,566.9702;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;79;-2051.249,706.2714;Float;True;Property;_TextureSample5;Texture Sample 5;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;78;-2051.251,901.2714;Float;True;Property;_TextureSample4;Texture Sample 4;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector4Node;16;-1891.659,86.89233;Float;False;Global;_AuraSourcePosition;_AuraSourcePosition;0;0;0,0,0,0;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SummedBlendNode;80;-1437.65,893.4714;Float;False;5;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;81;-1528.266,1065.667;Float;False;Property;_NoiseDistortionPower;Noise Distortion Power;8;0;1;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;17;-1618.658,113.8923;Float;False;FLOAT3;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1669.591,-40.50447;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1166.953,954.7391;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;84;-1006.533,952.0466;Float;False;NoiseResult;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;1;-1374.591,20.49551;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;88;-1513.288,285.7684;Float;False;Constant;_Float0;Float 0;13;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;85;-1573.412,372.3523;Float;False;84;0;1;FLOAT
Node;AmplifyShaderEditor.NegateNode;37;-1218.37,21.55704;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;19;-1256.96,121.2901;Float;False;Property;_MaskDistance;Mask Distance;1;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;87;-1316.288,322.7684;Float;False;Property;_Noise01Enabled;Noise 01 Enabled;4;0;0;True;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-1008.579,180.485;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-1007.495,49.41796;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;22;-864.2352,62.98032;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;6;-692.5444,64.24938;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;35;-553.6145,72.19872;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-686.6122,199.7073;Float;False;Property;_MaskExp;Mask Exp;3;0;1;0.2;10;0;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;23;-382.0311,168.0194;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;26;-367.5374,276.0275;Float;False;Property;_MaskMultiply;Mask Multiply;2;0;1;0;4;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;36;-231.0002,181.0546;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-38.88911,224.8188;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;28;113.1131,220.2716;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;33;395.3895,-33.23037;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/DarkeningAura;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Custom;0.5;True;False;0;True;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;41;0;38;0
WireConnection;41;3;39;0
WireConnection;45;0;41;0
WireConnection;45;1;40;0
WireConnection;57;0;53;0
WireConnection;57;1;51;0
WireConnection;49;0;45;0
WireConnection;49;1;43;0
WireConnection;56;0;52;2
WireConnection;56;1;54;0
WireConnection;60;0;57;0
WireConnection;60;1;56;0
WireConnection;60;2;49;0
WireConnection;65;0;60;0
WireConnection;72;0;65;2
WireConnection;72;1;65;0
WireConnection;64;0;59;0
WireConnection;74;0;65;0
WireConnection;74;1;65;1
WireConnection;71;0;65;1
WireConnection;71;1;65;2
WireConnection;77;0;73;0
WireConnection;77;1;74;0
WireConnection;67;0;64;0
WireConnection;67;1;64;0
WireConnection;79;0;73;0
WireConnection;79;1;71;0
WireConnection;78;0;73;0
WireConnection;78;1;72;0
WireConnection;80;0;67;0
WireConnection;80;1;79;1
WireConnection;80;2;78;1
WireConnection;80;3;77;1
WireConnection;17;0;16;1
WireConnection;17;1;16;2
WireConnection;17;2;16;3
WireConnection;83;0;80;0
WireConnection;83;1;81;0
WireConnection;84;0;83;0
WireConnection;1;0;2;0
WireConnection;1;1;17;0
WireConnection;37;0;1;0
WireConnection;87;0;85;0
WireConnection;87;1;88;0
WireConnection;86;0;19;0
WireConnection;86;1;87;0
WireConnection;18;0;37;0
WireConnection;18;1;19;0
WireConnection;18;2;87;0
WireConnection;22;0;18;0
WireConnection;22;2;86;0
WireConnection;6;0;22;0
WireConnection;35;0;6;0
WireConnection;23;0;35;0
WireConnection;23;1;24;0
WireConnection;36;0;23;0
WireConnection;25;0;36;0
WireConnection;25;1;26;0
WireConnection;28;0;25;0
WireConnection;33;9;28;0
ASEEND*/
//CHKSM=01E90279CAA247F73A228E065358F8F3E8DC1842