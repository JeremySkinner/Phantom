import file from Imported.boo

target default:
	print version()
	
target printVersion:
	#calls PrintVersion from the imported script
	PrintVersion()