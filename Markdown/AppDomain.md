# Note of AppDomain
* A process contains one or more `AppDomain`.
* An `AppDomain` contains one or more threads.
* A thread in an `AppDomain` cannot access another `AppDomain.`

# When to use AppDomain
* You want to simultaneously run different versions of an assembly.
* You have a long-running process, and you want to load and unload an assembly like a plugin before the process ends.
* You want to run an unstable assembly but don't want to crash a process when the assembly crashes.
