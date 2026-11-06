using System;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace Isaev.Features {
    public class PostsCounterPresenter : IStartable, IDisposable {
        readonly PostsModel _postsModel;
        readonly PostsCounterView _postsCounterView;
        readonly CompositeDisposable _disposables = new CompositeDisposable();

        public PostsCounterPresenter(PostsModel postsModel, PostsCounterView postsCounterView) {
            _postsModel = postsModel;
            _postsCounterView = postsCounterView;
        }

        public void Start() {
            _postsModel.CreatedCount
                .Select(count => $"Создано постов: {count}")
                .Subscribe(_postsCounterView.SetText)
                .AddTo(_disposables);
        }

        public void Dispose() {
            _disposables.Dispose();
        }
    }
}