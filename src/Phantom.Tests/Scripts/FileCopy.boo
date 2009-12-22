target copyFile:
  rmdir("copy_output")
  
  with FileList("SubDirectory/SubDirectory2"):
    .Include("*.txt")
    .ForEach def(file):
      print file
      file.CopyToDirectory("copy_output")
      
target copySubDirectories:
  rmdir("copy_output")
  
  with FileList("SubDirectory"):
    .Include("**/*")
    .ForEach def(file):
      print file
      file.CopyToDirectory("copy_output")