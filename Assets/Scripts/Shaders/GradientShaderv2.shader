Shader "RimLighting"
{
	Properties 
	{
_Bottom("_Bottom", Color) = (1,1,1,1)
_Top("_Top", Color) = (1,1,1,1)
_EmissionMult("_EmissionMult", Float) = 0
_HeightBlur("HeightBlur", Float) = 0
_BaseHeight("BaseHeight", Float) = 0
_TopRimBlur("TopRimBlur", Float) = 0
_TopRimOffset("TopRimOffset", Float) = 0
_RimColor("RimColor", Color) = (1,1,1,1)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


float4 _Bottom;
float4 _Top;
float _EmissionMult;
float _HeightBlur;
float _BaseHeight;
float _TopRimBlur;
float _TopRimOffset;
float4 _RimColor;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float4 fullMeshUV1;
float3 viewDir;
float4 color : COLOR;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);

o.fullMeshUV1 = v.texcoord;

			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Split0=(IN.fullMeshUV1);
float4 Multiply0=float4( Split0.y, Split0.y, Split0.y, Split0.y) * _HeightBlur.xxxx;
float4 Add0=Multiply0 + _BaseHeight.xxxx;
float4 Clamp0=clamp(Add0,float4( 0.0, 0.0, 0.0, 0.0 ),float4( 1.0, 1.0, 1.0, 1.0 ));
float4 Lerp0=lerp(_Bottom,_Top,Clamp0);
float4 Multiply1=float4( Split0.y, Split0.y, Split0.y, Split0.y) * _TopRimBlur.xxxx;
float4 Add1=Multiply1 + _TopRimOffset.xxxx;
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
float4 Multiply2=Add1 * Fresnel0;
float4 Clamp1=clamp(Multiply2,float4( 0.0, 0.0, 0.0, 0.0 ),float4( 1.0, 1.0, 1.0, 1.0 ));
float4 Lerp1=lerp(Lerp0,_RimColor,Clamp1);
float4 Divide0=Lerp1 / _EmissionMult.xxxx;
float4 Multiply3=Divide0 * IN.color;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Lerp0;
o.Emission = Multiply3;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}