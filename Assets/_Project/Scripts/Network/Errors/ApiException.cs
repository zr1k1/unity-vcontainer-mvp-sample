using System;

namespace Isaev.Network {
    public class ApiException : Exception {
        public ApiError Error { get; }

        public ApiException(ApiError error, string message) : base(message) {
            Error = error;
        }
    }
}