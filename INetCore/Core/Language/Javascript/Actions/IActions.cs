using System.Threading.Tasks;
using INetCore.Core.Language.Javascript.Tokens;

namespace INetCore.Core.Language.Javascript.Actions
{
    public interface IActions
    {
        Variable ReturnValue { get; }
        bool Execute();
    }

    public interface IAsyncActions : IActions
    {
        new Task<bool> ExecuteAsync();
    }
}
