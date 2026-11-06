using Isaev.Network;
using UnityEngine;

namespace Isaev.UI {
    [CreateAssetMenu(fileName = "errorMessagesConfig", menuName = "Configs/Error Messages Config", order = 1)]
    public class ErrorMessagesConfig : ScriptableObject {
        [SerializeField] SerializableDictionary<ApiError, string> _messages = default;
        [SerializeField] string _fallbackMessage = "Что-то пошло не так. Попробуйте позже";

        public string GetMessage(ApiError error) {
            if (_messages.TryGetValue(error, out var message))
                return message;

            Debug.LogError($"Не задан текст для ошибки {error}");
            return _fallbackMessage;
        }
    }
}