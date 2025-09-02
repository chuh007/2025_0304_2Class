using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Test.TMP
{
    public class TypeWriterEffect : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        [SerializeField] private float typeDuration = 0.2f;
        [SerializeField] private Color startColor, endColor;

        private int textIndex = 0; //이건 나중에 과제할 때 필요해.
        private CancellationToken tkn;
        private bool _isTyping = false;

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (_isTyping) SkipEffect();
                else StartEffect("Hello this is GGM!");
            }
        }

        private void SkipEffect()
        {
            tmpText.maxVisibleCharacters = 100;
            textIndex = 100;
        }

        private async void StartEffect(string msg)
        {
            _isTyping = true;
            tmpText.SetText(msg);
            tmpText.color = endColor;
            tmpText.ForceMeshUpdate();
            textIndex = 0;
            tmpText.maxVisibleCharacters = 0; //모든 텍스트 감춰주고

            TMP_TextInfo textInfo = tmpText.textInfo;
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if(textIndex == 100) return;
                tmpText.maxVisibleCharacters = i + 1;
                tmpText.ForceMeshUpdate();

                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (charInfo.isVisible == false)
                    await UniTask.WaitForSeconds(typeDuration, cancellationToken: tkn);
                else
                {
                    Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                    Color32[] vertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

                    int v0 = charInfo.vertexIndex;
                    int v1 = v0 + 1;
                    int v2 = v0 + 2;

                    Vector3 v1Origin = vertices[v1]; //좌측상단 
                    Vector3 v2Origin = vertices[v2]; //우측상단

                    float currentTime = 0;
                    float percent = 0;
                    while (percent < 1)
                    {
                        currentTime += Time.deltaTime;
                        percent = currentTime / typeDuration;

                        float yDelta = Mathf.Lerp(2f, 0, percent);

                        vertices[v1] = v1Origin + new Vector3(0, yDelta);
                        vertices[v2] = v2Origin + new Vector3(0, yDelta);

                        for (int j = 0; j < 4; j++)
                        {
                            vertexColors[v0 + j] = Color.Lerp(startColor, endColor, percent);
                        }

                        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices
                                                 | TMP_VertexDataUpdateFlags.Colors32);
                        await UniTask.NextFrame(cancellationToken: tkn);
                    }

                    vertices[v1] = v1Origin;
                    vertices[v2] = v2Origin;
                    for (int j = 0; j < 4; j++)
                    {
                        vertexColors[v0 + j] = endColor;
                    }

                    tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices | TMP_VertexDataUpdateFlags.Colors32);
                }

            }
            _isTyping = false;
        }
        
    }
}