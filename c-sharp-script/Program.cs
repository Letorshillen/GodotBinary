using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using MessagePack;
using System.Diagnostics;

namespace PruningTest
{
	static class Program
	{
		static void Main(string[] args)
		{
			var watch = new Stopwatch();
			var nGramsTrie = LoadEvents();
			watch.Start();
			SerializeToFile(nGramsTrie, "test.bin");
			Console.WriteLine($"Time Creating binary File: {watch.ElapsedMilliseconds} ms");

			watch.Restart();
			Event[] data = DeserializeFromFile("test.bin");
			Console.WriteLine(data[0].Id);
			Console.WriteLine($"Time Loading from binary File: {watch.ElapsedMilliseconds} ms");
		}

		public static void SerializeToFile(Event[] data, string filePath)
		{
			byte[] bytes = MessagePackSerializer.Serialize(data);
			File.WriteAllBytes(filePath, bytes);
		}

		public static Event[] DeserializeFromFile(string filePath)
		{
			byte[] bytes = File.ReadAllBytes(filePath);
			var data = MessagePackSerializer.Deserialize<Event[]>(bytes);
			return data;
		}

		private static Event[] LoadEvents()
		{
			var watch = new Stopwatch();
			watch.Start();

			string filePath = $"test.json";
			StreamReader reader = new StreamReader(filePath);
			var json = reader.ReadToEnd();

			var events = JsonConvert.DeserializeObject<Event[]>(json);
			Console.WriteLine($"Time Loading Json: {watch.ElapsedMilliseconds} ms");
			return events;
		}
	}
}

[MessagePackObject]
public class Event
{
	[Key(0)]
	public string Id { get; set; }

	[Key(1)]
	public string Type { get; set; }

	[Key(2)]
	public Actor Actor { get; set; }

	[Key(3)]
	public Repo Repo { get; set; }

	[Key(4)]
	public Payload Payload { get; set; }

	[Key(5)]
	public bool Public { get; set; }

	[Key(6)]
	public DateTime CreatedAt { get; set; }
}

[MessagePackObject]
public class Actor
{
	[Key(0)]
	public int Id { get; set; }

	[Key(1)]
	public string Login { get; set; }

	[Key(2)]
	public string GravatarId { get; set; }

	[Key(3)]
	public string Url { get; set; }

	[Key(4)]
	public string AvatarUrl { get; set; }
}

[MessagePackObject]
public class Repo
{
	[Key(0)]
	public int Id { get; set; }

	[Key(1)]
	public string Name { get; set; }

	[Key(2)]
	public string Url { get; set; }
}

[MessagePackObject]
public class Payload
{
	[Key(0)]
	public string Ref { get; set; }

	[Key(1)]
	public string RefType { get; set; }

	[Key(2)]
	public string MasterBranch { get; set; }

	[Key(3)]
	public string Description { get; set; }

	[Key(4)]
	public string PusherType { get; set; }
}
