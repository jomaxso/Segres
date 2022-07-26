// See https://aka.ms/new-console-template for more information

using System.Collections;

var list = new AsyncList<int>();

var x = await list.ToArrayAsync();

Console.WriteLine("Hello, World!");