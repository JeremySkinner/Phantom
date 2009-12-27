import Phantom.Tests.ForAutoRunTests

target autoRuns:
  FooRunnable()
 
target autoRunWith:
  with FooRunnable():
    .SetMessage("foo ")