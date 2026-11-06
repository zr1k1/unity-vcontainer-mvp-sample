using UnityEngine;

namespace Isaev.Features {
    [CreateAssetMenu(fileName = "postsConfig", menuName = "Configs/Posts Config")]
    public class PostsConfig : ScriptableObject {
        [SerializeField, Min(1)] int _maxPosts = 3;

        public int MaxPosts => _maxPosts;
    }
}