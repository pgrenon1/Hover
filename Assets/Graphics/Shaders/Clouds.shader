// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Clouds"
{
	Properties
	{
		_NoiseMasterScale("Noise Master Scale", Float) = 0.18
		_Noise1Scale("Noise 1 Scale", Float) = 0.53
		_Noise2Scale("Noise 2 Scale", Float) = 1.32
		_NoiseTexture("Noise Texture", 2D) = "white" {}
		_CloudSpeed("Cloud Speed", Float) = 0.5
		_CloudSoftness("Cloud Softness", Range( 0 , 1)) = 0.17
		_CloudCutoff("Cloud Cutoff", Range( -0.5 , 1)) = 0.1
		_midYValue("midYValue", Float) = 0
		_TaperPower("Taper Power", Float) = 1
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		AlphaToMask On
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _midYValue;
		uniform float _TaperPower;
		uniform sampler2D _NoiseTexture;
		uniform float _CloudSpeed;
		uniform float _Noise1Scale;
		uniform float _NoiseMasterScale;
		uniform float _Noise2Scale;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float _CloudCutoff;
		uniform float _CloudSoftness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float temp_output_46_0 = ( 1.0 - pow( saturate( ( abs( ( _midYValue - ase_worldPos.y ) ) / 1.44 ) ) , _TaperPower ) );
			float2 appendResult6 = (float2(ase_worldPos.x , ase_worldPos.z));
			float mulTime2 = _Time.y * _CloudSpeed;
			float2 appendResult3 = (float2(mulTime2 , mulTime2));
			float temp_output_34_0 = ( tex2D( _NoiseTexture, ( ( appendResult6 - appendResult3 ) * _Noise1Scale * _NoiseMasterScale ) ).r * tex2D( _NoiseTexture, ( ( appendResult6 + appendResult3 ) * _NoiseMasterScale * _Noise2Scale ) ).r );
			float3 temp_cast_0 = (( 1.0 - ( temp_output_46_0 - ( temp_output_34_0 * temp_output_46_0 ) ) )).xxx;
			o.Emission = temp_cast_0;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode51 = tex2D( _TextureSample2, uv_TextureSample2 );
			o.Alpha = pow( saturate( (0.0 + (( temp_output_34_0 * temp_output_46_0 * tex2DNode51.r ) - _CloudCutoff) * (1.0 - 0.0) / (1.0 - _CloudCutoff)) ) , _CloudSoftness );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1921;1;1918;1017;79.60846;-258.5613;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;1;-1167.709,323.2121;Float;False;Property;_CloudSpeed;Cloud Speed;6;0;Create;True;0;0;False;0;0.5;0.18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-431.8022,847.1887;Float;False;Property;_midYValue;midYValue;9;0;Create;True;0;0;False;0;0;-20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;36;-508.1106,995.0781;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;2;-955.7858,334.3432;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;37;-223.1107,931.0781;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;5;-1061.53,144.0035;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;41;-240.4107,1178.078;Float;False;Constant;_cloudHeight;cloudHeight;10;0;Create;True;0;0;False;0;1.44;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;38;-29.71069,809.5781;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;3;-719.8091,377.7539;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-815.5358,190.7536;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-252.3099,299.8372;Float;False;Property;_NoiseMasterScale;Noise Master Scale;1;0;Create;True;0;0;False;0;0.18;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-242.292,605.939;Float;False;Property;_Noise2Scale;Noise 2 Scale;3;0;Create;True;0;0;False;0;1.32;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-498.3026,357.7182;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;-511.6601,206.337;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;39;166.7893,966.4781;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-264.554,46.05092;Float;False;Property;_Noise1Scale;Noise 1 Scale;2;0;Create;True;0;0;False;0;0.53;0.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;15;74.94052,268.6704;Float;True;Property;_NoiseTexture;Noise Texture;5;0;Create;True;0;0;False;0;None;94a0a2ad4c629284ea569c3080ce5a45;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;43;335.0893,1029.978;Float;False;Property;_TaperPower;Taper Power;10;0;Create;True;0;0;False;0;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;28.19066,539.1531;Float;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;42;351.1894,915.378;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;30.41683,47.16401;Float;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;44;530.8895,960.0781;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;20;392.1735,507.9864;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;388.8341,58.29499;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;46;786.2067,953.4396;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;744.9622,379.1093;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;504.1428,740.7181;Float;True;Property;_TextureSample2;Texture Sample 2;11;0;Create;True;0;0;False;0;None;31890676c5b178840848afa665cb5a2f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;1099.7,718.6059;Float;False;Property;_CloudCutoff;Cloud Cutoff;8;0;Create;True;0;0;False;0;0.1;0.028;-0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;941.309,565.0161;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;29;1407.7,590.6059;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;1052.143,848.7181;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;1353.4,476.9059;Float;False;Property;_CloudSoftness;Cloud Softness;7;0;Create;True;0;0;False;0;0.17;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;30;1594.7,620.6059;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;48;1321.143,905.7181;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;52;1699.156,415.9241;Float;False;Constant;_Color0;Color 0;12;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;31;1742.7,585.6059;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;37.18929,1202.578;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;1577.7,-37.3941;Float;False;Property;_Color;Color;4;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;49;1514.143,930.7181;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;27;2062.039,503.9681;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Clouds;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;37;0;35;0
WireConnection;37;1;36;2
WireConnection;38;0;37;0
WireConnection;3;0;2;0
WireConnection;3;1;2;0
WireConnection;6;0;5;1
WireConnection;6;1;5;3
WireConnection;4;0;6;0
WireConnection;4;1;3;0
WireConnection;7;0;6;0
WireConnection;7;1;3;0
WireConnection;39;0;38;0
WireConnection;39;1;41;0
WireConnection;14;0;4;0
WireConnection;14;1;9;0
WireConnection;14;2;10;0
WireConnection;42;0;39;0
WireConnection;11;0;7;0
WireConnection;11;1;8;0
WireConnection;11;2;9;0
WireConnection;44;0;42;0
WireConnection;44;1;43;0
WireConnection;20;0;15;0
WireConnection;20;1;14;0
WireConnection;19;0;15;0
WireConnection;19;1;11;0
WireConnection;46;0;44;0
WireConnection;34;0;19;1
WireConnection;34;1;20;1
WireConnection;45;0;34;0
WireConnection;45;1;46;0
WireConnection;45;2;51;1
WireConnection;29;0;45;0
WireConnection;29;1;28;0
WireConnection;47;0;34;0
WireConnection;47;1;46;0
WireConnection;30;0;29;0
WireConnection;48;0;46;0
WireConnection;48;1;47;0
WireConnection;31;0;30;0
WireConnection;31;1;32;0
WireConnection;40;0;41;0
WireConnection;49;0;48;0
WireConnection;27;2;49;0
WireConnection;27;9;31;0
ASEEND*/
//CHKSM=3F16B74EE3C9FA26F2876E69EF17BD44342F950E