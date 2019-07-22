// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ToonFire"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 1.86
		_Texture0("Texture 0", 2D) = "white" {}
		_NoiseSSpeed1("NoiseS Speed 1", Float) = -1.5
		_NoiseScale1("Noise Scale 1", Float) = 1.22
		_NoiseScale2("Noise Scale 2", Float) = 0.54
		_NoiseSpeed2("Noise Speed 2", Float) = -1
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_OpacityStep("Opacity Step", Range( 0 , 1)) = 0.1276471
		_InnerFlameStep("Inner Flame Step", Range( 0 , 1)) = 0.4082353
		_InnerColor("Inner Color", Color) = (1,0.9289225,0,1)
		_OuterColor("Outer Color", Color) = (1,0.6664481,0,1)
		_OuterColorBlend("Outer Color Blend", Range( 0 , 1)) = 1
		_OuterColorTop("Outer Color Top", Color) = (0.9528302,0.1878373,0,0)
		_ShadingAmount("Shading Amount", Range( 0 , 1)) = 0.6342486
		_FlameStrength("Flame Strength", Float) = 8
		_Brightness("Brightness", Float) = 1.93
		_PushForward("Push Forward", Float) = 0.23
		_FlameIntensity("Flame Intensity", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _PushForward;
		uniform float _ShadingAmount;
		uniform sampler2D _Texture0;
		uniform float _NoiseSSpeed1;
		uniform float _NoiseScale1;
		uniform float _NoiseSpeed2;
		uniform float _NoiseScale2;
		uniform float _FlameStrength;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float _FlameIntensity;
		uniform float4 _InnerColor;
		uniform float _InnerFlameStep;
		uniform float _OpacityStep;
		uniform float4 _OuterColor;
		uniform float _OuterColorBlend;
		uniform float4 _OuterColorTop;
		uniform float _Brightness;
		uniform float _Cutoff = 1.86;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			//Calculate new billboard vertex position and normal;
			float3 upCamVec = normalize ( UNITY_MATRIX_V._m10_m11_m12 );
			float3 forwardCamVec = -normalize ( UNITY_MATRIX_V._m20_m21_m22 );
			float3 rightCamVec = normalize( UNITY_MATRIX_V._m00_m01_m02 );
			float4x4 rotationCamMatrix = float4x4( rightCamVec, 0, upCamVec, 0, forwardCamVec, 0, 0, 0, 0, 1 );
			v.normal = normalize( mul( float4( v.normal , 0 ), rotationCamMatrix )).xyz;
			//This unfortunately must be made to take non-uniform scaling into account;
			//Transform to world coords, apply rotation and transform back to local;
			v.vertex = mul( v.vertex , unity_ObjectToWorld );
			v.vertex = mul( v.vertex , rotationCamMatrix );
			v.vertex = mul( v.vertex , unity_WorldToObject );
			float4 transform73 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float4 normalizeResult76 = normalize( ( float4( _WorldSpaceCameraPos , 0.0 ) - transform73 ) );
			float3 ase_vertex3Pos = v.vertex.xyz;
			v.vertex.xyz += ( ( _PushForward * normalizeResult76 ) + float4( ( 0.1 * ( 0 + ase_vertex3Pos ) ) , 0.0 ) ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 appendResult8 = (float4(0.0 , _NoiseSSpeed1 , 0.0 , 0.0));
			float2 uv_TexCoord5 = i.uv_texcoord * float2( 1.5,1 );
			float2 panner10 = ( 1.0 * _Time.y * appendResult8.xy + ( _NoiseScale1 * uv_TexCoord5 ));
			float4 appendResult9 = (float4(0.0 , _NoiseSpeed2 , 0.0 , 0.0));
			float2 panner11 = ( 1.0 * _Time.y * appendResult9.xy + ( uv_TexCoord5 * _NoiseScale2 ));
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode18 = tex2D( _TextureSample2, uv_TextureSample2 );
			float4 temp_output_16_0 = ( ( ( tex2D( _Texture0, panner10 ) * tex2D( _Texture0, panner11 ) * _FlameStrength ) * tex2DNode18 ) + ( tex2DNode18 * _FlameIntensity ) );
			float4 temp_cast_2 = (_InnerFlameStep).xxxx;
			float4 temp_output_23_0 = step( temp_cast_2 , temp_output_16_0 );
			float4 temp_cast_3 = (_OpacityStep).xxxx;
			float4 temp_cast_4 = (_InnerFlameStep).xxxx;
			float smoothstepResult45 = smoothstep( 0.0 , _OuterColorBlend , i.uv_texcoord.y);
			float4 temp_output_59_0 = ( ( ( 1.0 - _ShadingAmount ) + temp_output_16_0 ) * ( ( _InnerColor * temp_output_23_0 ) + ( ( step( temp_cast_3 , temp_output_16_0 ) - temp_output_23_0 ) * ( ( _OuterColor * ( 1.0 - smoothstepResult45 ) ) + ( smoothstepResult45 * _OuterColorTop ) ) ) ) * _Brightness );
			o.Emission = temp_output_59_0.rgb;
			o.Alpha = 1;
			clip( temp_output_59_0.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
789;73;598;651;-2256.349;682.9821;2.625695;False;False
Node;AmplifyShaderEditor.RangedFloatNode;1;-431,-167;Float;False;Property;_NoiseSSpeed1;NoiseS Speed 1;2;0;Create;True;0;0;False;0;-1.5;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-466,2;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.5,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-424,-74;Float;False;Property;_NoiseScale1;Noise Scale 1;3;0;Create;True;0;0;False;0;1.22;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-428,125;Float;False;Property;_NoiseScale2;Noise Scale 2;4;0;Create;True;0;0;False;0;0.54;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-433,221;Float;False;Property;_NoiseSpeed2;Noise Speed 2;5;0;Create;True;0;0;False;0;-1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-169,111;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-165,249;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-166,-76;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-162,-222;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;10;24,-156;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;11;26,195;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;12;-7,-19;Float;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;False;0;150f5063c89986b4da523da2545520bc;8e30dd1efdf479c4b87cd6d4c6a4d0d5;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;53;576.0456,149.2839;Float;False;Property;_FlameStrength;Flame Strength;14;0;Create;True;0;0;False;0;8;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;265,-124;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;262,90;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;1263.98,645.6993;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;467.6002,323;Float;True;Property;_TextureSample2;Texture Sample 2;6;0;Create;True;0;0;False;0;163dd21d99eb0ae4c8d751b78b48507f;c3d0b28705093ab4cb848a8f6bb22aea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;1223.679,792.5994;Float;False;Property;_OuterColorBlend;Outer Color Blend;11;0;Create;True;0;0;False;0;1;0.427;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;740.0456,-71.71609;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;84;609.3073,542.9215;Float;False;Property;_FlameIntensity;Flame Intensity;17;0;Create;True;0;0;False;0;1;1.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;45;1575.98,735.3994;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;912.3073,384.9215;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;908.8999,13.40001;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;50;1266.58,914.7994;Float;False;Property;_OuterColorTop;Outer Color Top;12;0;Create;True;0;0;False;0;0.9528302,0.1878373,0,0;1,0.5691917,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;46;1798.279,719.7993;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;1131.801,59.59998;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;21;1393.441,-30.83661;Float;False;Property;_InnerFlameStep;Inner Flame Step;8;0;Create;True;0;0;False;0;0.4082353;0.401;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;1388.441,261.1634;Float;False;Property;_OpacityStep;Opacity Step;7;0;Create;True;0;0;False;0;0.1276471;0.026;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;27;1664.441,360.1634;Float;False;Property;_OuterColor;Outer Color;10;0;Create;True;0;0;False;0;1,0.6664481,0,1;1,0.429066,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;73;2544.228,977.206;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceCameraPos;72;2489.528,813.2382;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;2017.98,719.7994;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;1817.779,874.4993;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;24;1750.441,169.1634;Float;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;23;1753.441,3.163391;Float;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BillboardNode;66;2631.171,1235.367;Float;False;Spherical;False;0;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;75;2817.228,896.206;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;1952.441,126.1634;Float;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;67;2622.434,1339.819;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;25;1678.441,-216.8366;Float;False;Property;_InnerColor;Inner Color;9;0;Create;True;0;0;False;0;1,0.9289225,0,1;1,0.9931969,0.1999997,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;1864.256,-410.3819;Float;False;Property;_ShadingAmount;Shading Amount;13;0;Create;True;0;0;False;0;0.6342486;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;49;2229.88,812.0993;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;56;1418.376,-246.3517;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;2142.441,-6.836609;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;58;2218.313,-394.3479;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;2568.528,727.2381;Float;False;Property;_PushForward;Push Forward;16;0;Create;True;0;0;False;0;0.23;0.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;2143.441,208.1634;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RelayNode;55;1779.651,-327.0929;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;70;2817.434,1473.819;Float;False;Constant;_Float0;Float 0;15;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;76;2965.228,889.206;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;2904.434,1277.819;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;3095.434,1311.819;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;3127.228,753.206;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;2411.641,71.83614;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;60;2483.72,237.6308;Float;False;Property;_Brightness;Brightness;15;0;Create;True;0;0;False;0;1.93;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;2387.317,-307.4657;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;3147.07,1034.751;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;63;2585.64,-431.7219;Float;False;Constant;_DepthFade;Depth Fade;15;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;64;3050.64,-403.7219;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;2676.529,-2.782972;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;61;2775.223,-446.3214;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;41;3463.833,102.7013;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ToonFire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;1.86;True;True;0;True;Transparent;;AlphaTest;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexScale;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;5;0
WireConnection;7;1;4;0
WireConnection;9;1;3;0
WireConnection;6;0;2;0
WireConnection;6;1;5;0
WireConnection;8;1;1;0
WireConnection;10;0;6;0
WireConnection;10;2;8;0
WireConnection;11;0;7;0
WireConnection;11;2;9;0
WireConnection;13;0;12;0
WireConnection;13;1;10;0
WireConnection;14;0;12;0
WireConnection;14;1;11;0
WireConnection;52;0;13;0
WireConnection;52;1;14;0
WireConnection;52;2;53;0
WireConnection;45;0;42;2
WireConnection;45;2;43;0
WireConnection;86;0;18;0
WireConnection;86;1;84;0
WireConnection;15;0;52;0
WireConnection;15;1;18;0
WireConnection;46;0;45;0
WireConnection;16;0;15;0
WireConnection;16;1;86;0
WireConnection;48;0;27;0
WireConnection;48;1;46;0
WireConnection;47;0;45;0
WireConnection;47;1;50;0
WireConnection;24;0;22;0
WireConnection;24;1;16;0
WireConnection;23;0;21;0
WireConnection;23;1;16;0
WireConnection;75;0;72;0
WireConnection;75;1;73;0
WireConnection;28;0;24;0
WireConnection;28;1;23;0
WireConnection;49;0;48;0
WireConnection;49;1;47;0
WireConnection;56;0;16;0
WireConnection;30;0;25;0
WireConnection;30;1;23;0
WireConnection;58;0;54;0
WireConnection;31;0;28;0
WireConnection;31;1;49;0
WireConnection;55;0;56;0
WireConnection;76;0;75;0
WireConnection;68;0;66;0
WireConnection;68;1;67;0
WireConnection;69;0;70;0
WireConnection;69;1;68;0
WireConnection;74;0;71;0
WireConnection;74;1;76;0
WireConnection;34;0;30;0
WireConnection;34;1;31;0
WireConnection;57;0;58;0
WireConnection;57;1;55;0
WireConnection;77;0;74;0
WireConnection;77;1;69;0
WireConnection;64;0;61;0
WireConnection;59;0;57;0
WireConnection;59;1;34;0
WireConnection;59;2;60;0
WireConnection;61;0;63;0
WireConnection;41;2;59;0
WireConnection;41;10;59;0
WireConnection;41;11;77;0
ASEEND*/
//CHKSM=65E0062A9D88AA21E225F9791E58DF6873495CEF