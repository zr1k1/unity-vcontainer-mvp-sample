using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System;

namespace Isaev.Network {
    public class ApiClient : IApiClient {
        readonly ApiConfig _config;

        public ApiClient(ApiConfig config) {
            _config = config;
        }

        public UniTask<PostDto> GetPostAsync(int id, CancellationToken ct)
            => GetAsync<PostDto>($"posts/{id}", ct);

        public UniTask<List<PostDto>> GetPostsAsync(CancellationToken ct)
            => GetAsync<List<PostDto>>("posts", ct);

        public UniTask<PostDto> CreatePostAsync(RequestPostDto post, CancellationToken ct)
            => PostAsync<RequestPostDto, PostDto>("posts", post, ct);

        async UniTask<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest body, CancellationToken ct) {
            var json = JsonConvert.SerializeObject(body);

            using var request = new UnityWebRequest($"{_config.BaseUrl}/{path}", "POST");
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return await SendAsync<TResponse>(request, ct);
        }

        async UniTask<T> GetAsync<T>(string path, CancellationToken ct) {
            using var request = UnityWebRequest.Get($"{_config.BaseUrl}/{path}");
            return await SendAsync<T>(request, ct);
        }

        async UniTask<T> SendAsync<T>(UnityWebRequest request, CancellationToken ct) {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(_config.TimeoutSeconds));

            try {
                await request.SendWebRequest().ToUniTask(cancellationToken: timeoutCts.Token);
            }
            catch (UnityWebRequestException e) when (e.Result == UnityWebRequest.Result.ConnectionError) {
                throw new ApiException(ApiError.NoConnection, $"Connection error: {request.url}");
            }
            catch (UnityWebRequestException e) {
                var error = e.ResponseCode == 404 ? ApiError.NotFound : ApiError.ServerError;
                throw new ApiException(error, $"HTTP {e.ResponseCode}: {e.Message} {request.url}");
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested) {
                throw new ApiException(ApiError.Timeout, $"Connection timeout: {request.url}");
            }

            return DeserializeResponse<T>(request);
        }

        T DeserializeResponse<T>(UnityWebRequest request) {
            try {
                var result = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                if (result == null)
                    throw new ApiException(ApiError.InvalidResponse, $"Empty response: {request.url}");

                return result;
            }
            catch (JsonException e) {
                throw new ApiException(ApiError.InvalidResponse, $"Invalid Response: {e.Message}");
            }
        }

    }
}
