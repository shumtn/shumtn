module shu.net.zeromq.util;

extern (C)
{
	/*Handle DSO symbol visibility */
	/++
	#if defined _WIN32
	# if defined DLL_EXPORT
	# define ZMQ_EXPORT __declspec(dllexport)
	# else
	# define ZMQ_EXPORT __declspec(dllimport)
	# endif
	#else
	# if defined __SUNPRO_C || defined __SUNPRO_CC
	# define ZMQ_EXPORT __global
	# elif (defined __GNUC__ && __GNUC__ >= 4) || defined __INTEL_COMPILER
	# define ZMQ_EXPORT __attribute__ ((visibility("default")))
	# else
	# define ZMQ_EXPORT
	# endif
	#endif
	++/
	
	/* Helper functions are used by perf tests so that they don't have to care */
	/* about minutiae of time-related functions on different OS platforms. */
	
	/* Starts the stopwatch. Returns the handle to the watch. */
	void* zmq_stopwatch_start();
	
	/* Stops the stopwatch. Returns the number of microseconds elapsed since */
	/* the stopwatch was started. */
	ulong zmq_stopwatch_stop(void* watch_);
	
	/* Sleeps for specified number of seconds. */
	void zmq_sleep(int seconds_);
}

