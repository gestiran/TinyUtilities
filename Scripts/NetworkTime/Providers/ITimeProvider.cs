using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinyUtilities.NetworkTime.Providers {
    internal interface ITimeProvider {
        public UniTask<TimeResult> GetTime(CancellationToken cancellation);
    }
}