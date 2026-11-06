using UnityEngine;
using System;
using VContainer.Unity;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using Isaev.Network;
using Isaev.UI;

namespace Isaev.Features {
    public class PostPresenter : IStartable, IDisposable {

        readonly CancellationTokenSource _cts = new CancellationTokenSource();
        readonly IApiClient _apiClient;
        readonly ErrorMessagesConfig _errorMessages;
        readonly PostView _view;
        readonly PostsModel _model;
        readonly ReactiveProperty<bool> _isLoading = new ReactiveProperty<bool>(false);
        readonly CompositeDisposable _disposables = new CompositeDisposable();
        IReadOnlyReactiveProperty<bool> _canSend;

        public PostPresenter(IApiClient apiClient, ErrorMessagesConfig errorMessages, PostView view, PostsModel model) {
            _apiClient = apiClient;
            _errorMessages = errorMessages;
            _view = view;
            _model = model;
        }

        public void Start() {
            _isLoading.AddTo(_disposables);
            _isLoading.Subscribe(_view.SetLoading).AddTo(_disposables);

            _canSend = _isLoading
                .CombineLatest(_model.CreatedCount, (isLoading, count) => !isLoading && count < _model.MaxPosts)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);
            _canSend.Subscribe(_view.SetSendEnabled).AddTo(_disposables);

            _view.SendClicked
                .Where(_ => _canSend.Value)
                .ThrottleFirst(TimeSpan.FromSeconds(1))
                .Subscribe(_ => SendPostAsync().Forget())
                .AddTo(_disposables);

            _model.CreatedCount
                .Where(count => count >= _model.MaxPosts)
                .Subscribe(_ => _view.ShowLimitReached())
                .AddTo(_disposables);
        }

        async UniTaskVoid SendPostAsync() {

            _isLoading.Value = true;

            RequestPostDto requestPostDto = new RequestPostDto {
                userId = 777,
                title = "new post",
                body = "new pst body"
            };

            try {
                var createdPost = await _apiClient.CreatePostAsync(requestPostDto, _cts.Token);
                _view.ShowCreated(createdPost.id);
                _model.RegisterCreated(createdPost);
                _isLoading.Value = false;
            }
            catch (ApiException e) {
                Debug.LogWarning(e.Message);
                _view.ShowError(_errorMessages.GetMessage(e.Error), IsRetryable(e.Error));
                _isLoading.Value = false;
            }
        }

        public void Dispose() {
            _disposables.Dispose();   // отписаться от всего разом
            _cts.Cancel();   // прервать запросы, которые ещё идут
            _cts.Dispose();
        }

        static bool IsRetryable(ApiError error) =>
            error is ApiError.NoConnection or ApiError.Timeout or ApiError.ServerError;
    }
}
