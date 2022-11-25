// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/AuraEffectsPro/RingSprite"
{
	Properties
	{
		_FinalPower("Final Power", Range( 0 , 10)) = 2
		_FinalOpacityPower("Final Opacity Power", Range( 0 , 4)) = 1
		_MaskTex("Mask Tex", 2D) = "white" {}
		_MaskDistortionPower("Mask Distortion Power", Range( 0 , 1)) = 0.25
		_MaskMofidyAdd("Mask Mofidy Add", Range( 0 , 1)) = 1
		_Ramp("Ramp", 2D) = "white" {}
		_RampMask("Ramp Mask", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		_RampTilingMultiply("Ramp Tiling Multiply", Float) = 1
		[Toggle]_RampFlip("Ramp Flip", Int) = 0
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01ScrollSpeed("Noise 01 Scroll Speed", Float) = -0.25
		_Noise01TilingU("Noise 01 Tiling U", Float) = 1
		_Noise01TilingV("Noise 01 Tiling V", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
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
		uniform float _RampTilingMultiply;
		uniform sampler2D _Noise01;
		uniform float _Noise01ScrollSpeed;
		uniform float _Noise01TilingU;
		uniform float _Noise01TilingV;
		uniform sampler2D _MaskTex;
		uniform float _MaskDistortionPower;
		uniform sampler2D _RampMask;
		uniform float _FinalOpacityPower;
		uniform float _MaskMofidyAdd;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord2 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 temp_output_3_0 = (float2( -1,-1 ) + (uv_TexCoord2 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
			float2 appendResult10 = (float2(( ( _Time.y * _Noise01ScrollSpeed ) + ( _Noise01TilingU * length( temp_output_3_0 ) ) ) , (0.0 + (atan2( temp_output_3_0.x , temp_output_3_0.y ) - ( -1.0 * UNITY_PI )) * (_Noise01TilingV - 0.0) / (UNITY_PI - ( -1.0 * UNITY_PI )))));
			float4 tex2DNode11 = tex2D( _Noise01, appendResult10 );
			float2 uv_TexCoord41 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 normalizeResult43 = normalize( (float2( -1,-1 ) + (uv_TexCoord41 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 ))) );
			float4 tex2DNode1 = tex2D( _MaskTex, ( uv_TexCoord41 + ( tex2DNode11.r * normalizeResult43 * _MaskDistortionPower ) ) );
			float ResultMask38 = ( tex2DNode11.r * tex2DNode1.r );
			float clampResult28 = clamp( ( _RampTilingMultiply * ResultMask38 ) , 0.0 , 1.0 );
			#ifdef _RAMPFLIP_ON
				float staticSwitch30 = ( 1.0 - clampResult28 );
			#else
				float staticSwitch30 = clampResult28;
			#endif
			float2 appendResult32 = (float2(staticSwitch30 , 0.0));
			o.Emission = ( _RampColorTint * _FinalPower * tex2D( _Ramp, appendResult32 ) * tex2D( _RampMask, appendResult32 ).r * i.vertexColor ).rgb;
			float clampResult50 = clamp( ( tex2DNode1.b + _MaskMofidyAdd ) , 0.0 , 1.0 );
			float clampResult19 = clamp( ( tex2DNode1.g * _FinalOpacityPower * i.vertexColor.a * clampResult50 ) , 0.0 , 1.0 );
			o.Alpha = clampResult19;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13903
7;29;1906;1004;637.2995;290.1791;1.095163;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1355,-25;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;3;-1122,-26;Float;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;13;-1104.871,-363.9518;Float;False;Property;_Noise01ScrollSpeed;Noise 01 Scroll Speed;11;0;-0.25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;14;-1082.86,-506.4909;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;23;-910.9648,-273.815;Float;False;Property;_Noise01TilingU;Noise 01 Tiling U;12;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;4;-854,81;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LengthOpNode;9;-843,-188;Float;False;1;0;FLOAT2;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-678.2924,-243.4211;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-743.282,-432.0769;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PiNode;7;-634,181;Float;False;1;0;FLOAT;-1.0;False;1;FLOAT
Node;AmplifyShaderEditor.PiNode;8;-635,256;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;21;-654.1849,326.7333;Float;False;Property;_Noise01TilingV;Noise 01 Tiling V;13;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ATan2OpNode;5;-595,80;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-1508.496,745.2929;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;6;-392,124;Float;False;5;0;FLOAT;0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-498.0308,-368.1438;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;10;-145,-178;Float;True;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.TFHCRemapNode;42;-1114.359,654.9128;Float;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2
Node;AmplifyShaderEditor.NormalizeNode;43;-850.902,655.6933;Float;True;1;0;FLOAT2;0,0,0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;47;-823.4082,501.328;Float;False;Property;_MaskDistortionPower;Mask Distortion Power;3;0;0.25;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;11;125.5752,-205.6923;Float;True;Property;_Noise01;Noise 01;10;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Noises/NoiseClassic01.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-533.2149,612.9395;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT2;0;False;2;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-295.8651,767.6201;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;1;128.7883,16.99011;Float;True;Property;_MaskTex;Mask Tex;2;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Masks/Mask02.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;715.0133,-97.71907;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;856.706,-97.75404;Float;False;ResultMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;25;-566.7303,-816.6979;Float;False;Property;_RampTilingMultiply;Ramp Tiling Multiply;8;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;26;-539.9053,-688.4603;Float;False;38;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-302.7303,-754.6979;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;28;-150.7305,-759.6979;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;49;135.8855,396.4883;Float;False;Property;_MaskMofidyAdd;Mask Mofidy Add;4;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;29;10.34666,-866.9009;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;30;186.9265,-789.1898;Float;False;Property;_RampFlip;Ramp Flip;9;0;0;False;True;;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;48;469.9105,334.064;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;31;263.6957,-683.2791;Float;False;Constant;_Float1;Float 1;12;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;18;237.5854,-384.4397;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;17;153.8802,215.6371;Float;False;Property;_FinalOpacityPower;Final Opacity Power;1;0;1;0;4;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;50;591.4739,334.0642;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;32;460.0307,-747.344;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;34;647.5452,-778.0755;Float;True;Property;_Ramp;Ramp;5;0;Assets/SineVFX/AuraAndGroundEffects/Resources/VFXTextures/Ramps/Ramp14.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;756.4606,136.4665;Float;False;4;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;24;730.1425,-1073.744;Float;False;Property;_RampColorTint;Ramp Color Tint;7;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;35;664.8001,-869.6262;Float;False;Property;_FinalPower;Final Power;0;0;2;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.WireNode;40;928.623,-328.9799;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;36;650.359,-586.5851;Float;True;Property;_RampMask;Ramp Mask;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;19;898.9983,134.3703;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;1013.802,-888.4263;Float;False;5;5;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1582.19,-92.24621;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SineVFX/AuraEffectsPro/RingSprite;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;4;0;3;0
WireConnection;9;0;3;0
WireConnection;22;0;23;0
WireConnection;22;1;9;0
WireConnection;15;0;14;2
WireConnection;15;1;13;0
WireConnection;5;0;4;0
WireConnection;5;1;4;1
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;6;2;8;0
WireConnection;6;4;21;0
WireConnection;12;0;15;0
WireConnection;12;1;22;0
WireConnection;10;0;12;0
WireConnection;10;1;6;0
WireConnection;42;0;41;0
WireConnection;43;0;42;0
WireConnection;11;1;10;0
WireConnection;45;0;11;1
WireConnection;45;1;43;0
WireConnection;45;2;47;0
WireConnection;44;0;41;0
WireConnection;44;1;45;0
WireConnection;1;1;44;0
WireConnection;39;0;11;1
WireConnection;39;1;1;1
WireConnection;38;0;39;0
WireConnection;27;0;25;0
WireConnection;27;1;26;0
WireConnection;28;0;27;0
WireConnection;29;0;28;0
WireConnection;30;0;29;0
WireConnection;30;1;28;0
WireConnection;48;0;1;3
WireConnection;48;1;49;0
WireConnection;50;0;48;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;34;1;32;0
WireConnection;16;0;1;2
WireConnection;16;1;17;0
WireConnection;16;2;18;4
WireConnection;16;3;50;0
WireConnection;40;0;18;0
WireConnection;36;1;32;0
WireConnection;19;0;16;0
WireConnection;37;0;24;0
WireConnection;37;1;35;0
WireConnection;37;2;34;0
WireConnection;37;3;36;1
WireConnection;37;4;40;0
WireConnection;0;2;37;0
WireConnection;0;9;19;0
ASEEND*/
//CHKSM=F0F4C356D52BFD5E41F3122EA79DE441D2F3E7A5