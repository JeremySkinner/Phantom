target copyFile:
  rm("copy_output")
  
  with FileList("SubDirectory/SubDirectory2"):
    .Include("*.txt")
    .ForEach def(file):
      print file
      file.CopyToDirectory("copy_output")
      
target copySubDirectories:
  rm("copy_output")
  
  with FileList("SubDirectory"):
    .Include("**/*")
    .ForEach def(file):
      print file
      file.CopyToDirectory("copy_output")

target copyViews:
  rm("copy_Views")
  
  with FileList("Views"):
    .Include("**/*")
    .ForEach def(file):
      print file
      file.CopyToDirectory("copy_Views")

target copyFlattened:
  rm("copy_output")
  
  with FileList("SubDirectory"):
    .Include("**/*.*")
    .Flatten(true)
    .ForEach def(file):
      file.CopyToDirectory("copy_output")