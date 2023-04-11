// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ImageEffectShader"
{
	SubShader
	{
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// VS2015�̃O���t�B�b�N�f�o�b�NON
			#pragma enable_d3d11_debug_symbols

			struct VertexInput {
				float4 pos:  POSITION;    // 3D��ԍ��W
				float2 uv:   TEXCOORD0;   // �e�N�X�`�����W
			};

			struct VertexOutput {
				float4 v:    SV_POSITION; // 2D���W
				float2 uv:   TEXCOORD0;   // �e�N�X�`�����W
			};

			// ���_ shader
			VertexOutput vert(VertexInput input)
			{
				VertexOutput output;
				output.v = UnityObjectToClipPos(input.pos);
				output.uv = input.uv;

				return output;
			}

			// �s�N�Z�� shader
			fixed4 frag(VertexOutput output) : SV_Target
			{
				float2 tex = output.uv;
				// ���F�����F�̃O���f�[�V����
				return fixed4(1.0, 1.0, 1.0 - tex.y, 1.0);
			}

			ENDCG
		}
	}
}
