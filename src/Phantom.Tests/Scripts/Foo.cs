using System;

public class Foo : Phantom.Core.BuildScript {
	Target @default = () => {
		Console.WriteLine("I am in yr default");
	};
}