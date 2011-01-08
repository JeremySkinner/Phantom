[Desc("Description")]
Target @default =()=> {
	Console.WriteLine("Hello");
};

[Depends("default")]
Target depends =() => {
	Console.WriteLine("Depends");
};