using System;
using TMPro;
using UnityEngine;

namespace Blade.Test.TMP
{
    public class WobblyText : MonoBehaviour
    {
        [Range(0.5f, 4f), SerializeField] private float amplitude;
        [SerializeField] private TMP_Text tmpText;
        
        private void Update()
        {
            tmpText.ForceMeshUpdate();
            TMP_TextInfo textInfo = tmpText.textInfo; // 입력된 텍스트의 정보를 가지고 있는 클래스

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                // I번째 캐릭터의 정보를 가져온다.
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (charInfo.isVisible == false)
                {
                    continue;
                }
                
                Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                
                int v0Index = charInfo.vertexIndex;
                
                float v0X = vertices[v0Index].x;
                
                for (int j = 0; j < 4; j++)
                {
                    Vector3 origin = vertices[v0Index + j];
                    vertices[v0Index + j] = origin
                                            + new Vector3(0, amplitude * Mathf.Sin(Time.time * 2f + v0X), 0);
                }
            } // end of for
            
            tmpText.UpdateVertexData();
            // Draft => 우리가 작업하는 버텍스
            // Working => 실제 TMP가 그려주는 버텍스
            // for (int i = 0; i < textInfo.meshInfo.Length; i++)
            // {
            //     TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            //     meshInfo.mesh.vertices = meshInfo.vertices;
            //     tmpText.UpdateGeometry(meshInfo.mesh, i);
            // }
        }
    }
}