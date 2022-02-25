using System;
using System.Threading.Tasks;

namespace INachalnicUtilities
{
    public interface IRetrier
    {
        T Retry<T>(Func<T> func);
        Task<T> RetryAsync<T>(Func<Task<T>> func);
    }

    public class Retrier : IRetrier
    {
        private int MaxRetries { get; }
        private TimeSpan WaitAfterRetry { get; }
        private Func<Exception, bool> ShouldRetry { get; }

        public Retrier(int maxRetries, TimeSpan waitAfterRetry, Func<Exception, bool> shouldRetry)
        {
            MaxRetries = maxRetries;
            WaitAfterRetry = waitAfterRetry;
            ShouldRetry = shouldRetry;
        }

        public T Retry<T>(Func<T> func)
        {
            Exception? ex = null;

            int retries = 0;
            while (retries < MaxRetries)
            {
                try
                {
                    return func();
                }
                catch (Exception e) when (ShouldRetry(e))
                {
                    retries++;
                    ex = e;
                    //Logger.Log(new LogMessage(LogType.Debug, $"Failed executing func for {retries} times out of {MaxRetries} (Exception Message: {e.Message}), retrying") { Module = nameof(Retrier) });
                    Task.Delay(WaitAfterRetry).Wait();
                }
            }
            ex ??= new Exception();
            var message = $"Too many retries. Retried for {MaxRetries} times without any success";
            //Logger.Log(new LogMessage(LogType.Debug, message, ex) { Module = nameof(Retrier) });
            throw ex;
        }
        public async Task RetryAsync(Func<Task> func)
        {
            Func<Task<bool>> wrapedFunc = async () =>
            {
                await func();
                return true;
            };
            await RetryAsync(wrapedFunc);
        }
        public async Task<T> RetryAsync<T>(Func<Task<T>> func)
        {
            Exception? ex = null;

            int retries = 0;
            while (retries < MaxRetries)
            {
                try
                {
                    return await func();
                }
                catch (Exception e) when (ShouldRetry(e))
                {
                    retries++;
                    ex = e;
                    //Logger.Log(new LogMessage(LogType.Debug, $"Failed executing func for {retries} times out of {MaxRetries} (Exception Message: {e.Message}), retrying") { Module = nameof(Retrier) });
                    await Task.Delay(WaitAfterRetry);
                }
            }
            ex ??= new Exception();
            var message = $"Too many retries. Retried for {MaxRetries} times without any success";
            //Logger.Log(new LogMessage(LogType.Debug, message, ex) { Module = nameof(Retrier) });
            throw ex;
        }
    }
}
