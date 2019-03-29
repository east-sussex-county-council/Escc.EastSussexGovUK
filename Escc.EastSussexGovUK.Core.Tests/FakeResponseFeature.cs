using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Escc.EastSussexGovUK.Core.Tests
{
    /// <remarks>
    /// From https://stackoverflow.com/a/49741221/3169017
    /// </remarks>
    internal class FakeResponseFeature : IHttpResponseFeature
    {
        public Stream Body { get; set; }

        public bool HasStarted { get { return hasStarted; } }

        public IHeaderDictionary Headers { get; set; }

        public string ReasonPhrase { get; set; }

        public int StatusCode { get; set; }

        public void OnCompleted(Func<object, Task> callback, object state)
        {
            //...No-op
        }

        public void OnStarting(Func<object, Task> callback, object state)
        {
            this.callback = callback;
            this.state = state;
        }

        bool hasStarted = false;
        Func<object, Task> callback;
        object state;

        public Task InvokeCallBack()
        {
            hasStarted = true;
            return callback(state);
        }
    }
}
