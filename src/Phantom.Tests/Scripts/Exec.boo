target clean_exit:
	exec("..\\..\\Scripts\\Support\\ExitTest.exe", "0")

target non_zero_exit:
	exec("..\\..\\Scripts\\Support\\ExitTest.exe", "99")
	
target non_zero_exit_ignore:
	exec("..\\..\\Scripts\\Support\\ExitTest.exe", "99", { @IgnoreNonZeroExitCode: true })
	
target foo:
	print "foo"
	
target clean_exit_singlestr:
	exec("..\\..\\Scripts\\Support\\ExitTest.exe 0")

target non_zero_exit_singlestr:
	exec("..\\..\\Scripts\\Support\\ExitTest.exe 99")

target non_zero_exit_ignore_singlestr:
	exec("..\\..\\Scripts\\Support\\ExitTest.exe 99", { @IgnoreNonZeroExitCode: true })