// See https://aka.ms/new-console-template for more information
Console.WriteLine("Tests to the primitive map");


var map = new PrimitiveMap(0);

int res;

//test put
map.Put("a", 1);
if (!map.TryGet("a", out res)) Console.WriteLine("Fail: cant get what was put");
if (res != 1) Console.WriteLine("Fail: wrong value at put");
if (map.Count != 1) Console.WriteLine("Fail: wrong count at put");

//test not-get
if (map.TryGet("b", out res)) Console.WriteLine("Fail: get what was not put");
if (res != -1) Console.WriteLine("Fail: invalid res from not-get");

//test override
map.Put("a", 2);
if (!map.TryGet("a", out res)) Console.WriteLine("Fail: cant get override");
if (res != 2) Console.WriteLine("Fail: wrong value at override");
if (map.Count != 1) Console.WriteLine("Fail: wrong count at override");

//remove
if (!map.Remove("a")) Console.WriteLine("Fail: remove");
if (map.TryGet("a", out res)) Console.WriteLine("Fail: get what was removed");
if (res != -1) Console.WriteLine("Fail: invalid res from remove");
if (map.Count != 0) Console.WriteLine("Fail: wrong count at remove");

//remove not present
if (map.Remove("a")) Console.WriteLine("Fail: remove already removed");
if (map.Remove("b")) Console.WriteLine("Fail: remove never present");

//reinstance
map.Put("a", 1);
if (!map.TryGet("a", out res)) Console.WriteLine("Fail: cant get reinstance");
if (res != 1) Console.WriteLine("Fail: wrong value at reinstance");
if (map.Count != 1) Console.WriteLine("Fail: wrong count at reinstance");

//put many
map.Put("b", 1);
map.Put("c", 1);
map.Put("d", 1);
map.Put("e", 1);
map.Put("f", 1);
if (!map.TryGet("f", out res)) Console.WriteLine("Fail: cant get when put many");
if (res != 1) Console.WriteLine("Fail: wrong value at put many");
if (map.Count != 6) Console.WriteLine("Fail: wrong count at put many");

//remove many
map.Remove("a");
map.Remove("b");
map.Remove("c");
map.Remove("d");
map.Remove("e");
map.Remove("f");
if (map.TryGet("f", out res)) Console.WriteLine("Fail: get when remove many");
if (res != -1) Console.WriteLine("Fail: wrong value at remove many");
if (map.Count != 0) Console.WriteLine("Fail: wrong count at remove many");

Console.WriteLine("Tests to the primitive map ends here");
Console.ReadLine();