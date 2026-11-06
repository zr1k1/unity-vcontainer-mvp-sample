using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Isaev.Network {
    public interface IApiClient {
        UniTask<PostDto> GetPostAsync(int id, CancellationToken ct);
        UniTask<List<PostDto>> GetPostsAsync(CancellationToken ct);
        UniTask<PostDto> CreatePostAsync(RequestPostDto post, CancellationToken ct);
    }
}