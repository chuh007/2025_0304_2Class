using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Blade.Test.ThreadTest
{
    public class CompletionSourceTest : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private bool _isOpen = false;
        private AwaitableCompletionSource<string> _completionSource;

        private void Start()
        {
            SetOpen(false);
            _completionSource = new AwaitableCompletionSource<string>();
            button.onClick.AddListener(()=>_completionSource.SetResult(inputField.text));
        }

        private void SetOpen(bool isOpen)
        {
            _isOpen = isOpen;
            canvasGroup.alpha = isOpen ? 1 : 0;
            canvasGroup.interactable = isOpen;
            canvasGroup.blocksRaycasts = isOpen;
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                OpenWindowAndRecordIt();
            }
        }

        private async void OpenWindowAndRecordIt()
        {
            if(_isOpen) return;
            SetOpen(true);

            string userName = await _completionSource.Awaitable;
            Debug.Log($"입력된 이름 : {userName}");
            SetOpen(false);
        }
    }
}