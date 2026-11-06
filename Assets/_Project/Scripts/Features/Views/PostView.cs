using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UniRx;

namespace Isaev.Features {
    public class PostView : MonoBehaviour {
        public IObservable<Unit> SendClicked =>
            _sendPostButton.OnClickAsObservable()
                .Merge(_repeatButton.OnClickAsObservable());

        // Outlets
        [SerializeField] TMP_Text _dataLabel = default;
        [SerializeField] Button _sendPostButton = default;
        [SerializeField] Button _repeatButton = default;

        public void SetLoading(bool isLoading) {
            if (isLoading) {
                ShowLoading();
            }
        }

        void ShowLoading() {
            _repeatButton.gameObject.SetActive(false);
            _dataLabel.text = "Загрузка поста на сервер...";
        }

        public void ShowCreated(int id) {
            _dataLabel.text = $"Пост создан, id: {id}";
        }

        public void ShowError(string message, bool canRetry) {
            _dataLabel.text = message;
            _repeatButton.gameObject.SetActive(canRetry);
        }

        public void SetSendEnabled(bool enable) {
            _sendPostButton.interactable = enable;
        }

        public void ShowLimitReached() {
            _dataLabel.text = $"Лимит постов исчерпан";
        }
    }
}