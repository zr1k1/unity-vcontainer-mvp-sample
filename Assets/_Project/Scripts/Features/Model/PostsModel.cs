using System;
using Isaev.Network;
using UniRx;

namespace Isaev.Features {
    public class PostsModel : IDisposable {

        public int MaxPosts => _config.MaxPosts;
        public IReadOnlyReactiveProperty<int> CreatedCount => _createdCount;

        readonly PostsConfig _config;
        readonly ReactiveProperty<int> _createdCount = new ReactiveProperty<int>(0);

        public PostsModel(PostsConfig config) {
            _config = config;
        }

        public void RegisterCreated(PostDto post) {
            _createdCount.Value++;
        }

        public void Dispose() {
            _createdCount.Dispose();
        }
    }
}