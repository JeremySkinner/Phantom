target default:
	print "executing"
	
target hello:
	print "hello"

target helloWorld:
	call hello
	print "world"

target helloWorldWithMultipleCalls:
	call hello, world
	
target world:
	print "world"