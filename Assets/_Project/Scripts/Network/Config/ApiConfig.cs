using UnityEngine;

namespace Isaev.Network {
    [CreateAssetMenu(fileName = "apiConfig", menuName = "Configs/Api Config", order = 1)]
    public class ApiConfig : ScriptableObject {
        public string BaseUrl => _baseUrl;
        public float TimeoutSeconds => _timeoutSeconds;

        [SerializeField] string _baseUrl = "https://jsonplaceholder.typicode.com";
        [SerializeField] float _timeoutSeconds = 10f;

    }
}