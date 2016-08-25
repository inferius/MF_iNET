using System;
using System.Threading.Tasks;
using INetCore.Core.Language.Javascript.Tokens;

namespace INetCore.Core.Language.Javascript.Actions
{
    public class AsyncAction : IAsyncActions
    {
        public Variable ReturnValue { get; private set; } = null;

        public bool Execute()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
