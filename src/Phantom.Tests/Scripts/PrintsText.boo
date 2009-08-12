target default:
	print "executing"
	
target hello:
	print "hello"

target helloWorld:
	runTarget hello
	print "world"