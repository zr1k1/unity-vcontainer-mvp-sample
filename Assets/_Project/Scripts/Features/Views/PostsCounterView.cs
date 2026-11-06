using TMPro;
using UnityEngine;

namespace Isaev.Features {
    public class PostsCounterView : MonoBehaviour {
        [SerializeField] TMP_Text _postsCountLabel = default;

        public void SetText(string text) {
            _postsCountLabel.text = text;
        }
    }
}