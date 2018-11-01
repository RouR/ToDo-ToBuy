using OpenTracing;
using OpenTracing.Propagation;

namespace CustomTracing
{
    public class NoTracer : ITracer
    {
        public ISpanBuilder BuildSpan(string operationName)
        {
            return null;
        }

        public void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier)
        {
        }

        public ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier)
        {
            return null;
        }

        public IScopeManager ScopeManager { get; } = null;
        public ISpan ActiveSpan { get; } = null;
    }
}
