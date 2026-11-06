using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Isaev.Network {
    public class FakeApiClient : IApiClient {
        readonly ApiError? _error;

        public FakeApiClient(ApiError? error = null) {
            _error = error; // null — отвечаем данными, иначе бросаем эту ошибку
        }

        public async UniTask<PostDto> GetPostAsync(int id, CancellationToken ct) {
            await SimulateNetworkAsync(ct);
            return new PostDto { id = id, userId = 1, title = $"Fake post {id}", body = "Fake body" };
        }

        public async UniTask<List<PostDto>> GetPostsAsync(CancellationToken ct) {
            await SimulateNetworkAsync(ct);
            return new List<PostDto>{
            new PostDto { id = 1, userId = 1, title = $"Fake post 1", body = "Fake body 1"  },
            new PostDto { id = 2, userId = 2, title = $"Fake post 2", body = "Fake body 2"  }
        };
        }

        public async UniTask<PostDto> CreatePostAsync(RequestPostDto post, CancellationToken ct) {
            await SimulateNetworkAsync(ct);
            return new PostDto { id = 101, userId = post.userId, title = post.title, body = post.body };
        }

        async UniTask SimulateNetworkAsync(CancellationToken ct) {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5), cancellationToken: ct);

            if (_error.HasValue)
                throw new ApiException(_error.Value, $"Fake error: {_error.Value}");
        }
    }
}