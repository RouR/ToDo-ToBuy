using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace Web
{
    public static class MetricsRegistry
    {
        public static CounterOptions Sample1Counter => new CounterOptions
        {
            Name = "Sample1 Counter",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions Sample2Counter => new CounterOptions
        {
            Name = "Sample2 Counter",
            MeasurementUnit = Unit.Requests
        };

        public static TimerOptions Sample1Timer => new TimerOptions
        {
            Name = "Sample1 Timer",
            MeasurementUnit = Unit.Requests,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Milliseconds
        };


        /// <summary>
        /// Rates that are measured are the mean, one minute, five minute and fifteen minute. The mean rate is an average rate of events for the lifetime of the application which does not provide a sense of recency, x-minute rates use an Exponential Weighted Moving Average (EWMA) for their calculations which do provide a sense of recency.
        /// </summary>
        public static MeterOptions Sample1Meter => new MeterOptions
        {
            Name = "Sample1 Meter",
            MeasurementUnit = Unit.Requests
        };
    }
}