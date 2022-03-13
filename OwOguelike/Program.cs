namespace OwOguelike;

internal class Program
{
    internal static readonly string ConsoleWarning =
@"
   _,gggggg,_                      _,gggggg,_
 ,d8P""""d8P""Y8b,                  ,d8P""""d8P""Y8b,
,d8'   Y8   ""8b,dP              ,d8'   Y8   ""8b,dP
d8'    `Ybaaad88P'              d8'    `Ybaaad88P'
8P       `""""""""Y8                8P       `""""""""Y8
8b            d8gg    gg    gg  8b            d8
Y8,          ,8PI8    I8    88bgY8,          ,8P
`Y8,        ,8P'I8    I8    8I  `Y8,        ,8P'
 `Y8b,,__,,d8P',d8,  ,d8,  ,8I   `Y8b,,__,,d8P'
   `""Y8888P""'  P""""Y88P""""Y88P""      `""Y8888P""'     
This console and anything you do inside it has no warranty!
If you open this console and something breaks, don't complain!
";

    public static void Main(string[] args) => new GameCore().Run();
}
