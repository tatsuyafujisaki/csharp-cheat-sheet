# Analogy
* `AutoResetEvent` is like a turnstile, which automatically gets closed everytime one passes by.
* `ManualResetEvent` is like a door, which keeps open unless manually closed.

# AutoResetEvent
* `AutoResetEvent(false)` ... Starts with red.
* `AutoResetEvent(true)` ... Starts with green.
* `Set()` ... Turn green. If `WaitOne()` was not called yet, keep green. If `WaitOne()` was already called, let the waiting thread go, and turn red again.
* `Reset()` ... Turn red.

# ManualResetEvent vs ManualResetEventSlim
`ManualResetEvent` waits using wait handles while `ManualResetEventSlim` busy-waits, which is efficient for a short time.

# References
* [ManualResetEvent and ManualResetEventSlim](https://docs.microsoft.com/en-us/dotnet/standard/threading/manualresetevent-and-manualreseteventslim)
*  [Semaphore and SemaphoreSlim](https://docs.microsoft.com/en-us/dotnet/standard/threading/semaphore-and-semaphoreslim)
