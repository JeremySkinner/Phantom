class Foo:
	_count as int
	
	def increment():
		_count++
		
	def output():
		print _count

target withInstance:
	f = Foo()
	with f:
		.increment()
		.output()

target withCtor:
	with Foo():
		.increment()
		.output()

target withAssignment:
	with f = Foo():
		.increment()
		.output()
		
	# we should still be able to access the 'f' variable after with with block
	f.increment()
	f.output()