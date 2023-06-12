using Godot;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Control : Godot.Control
{
	private readonly string modelDir = "res://";

	public override void _Ready()
	{
		var watch = new Stopwatch();
		watch.Start();
		LoadEvents();
		GD.Print("Time", watch.ElapsedMilliseconds);
	}

	public void LoadEvents()
	{
		var events = DeserializeFromFile();
		GD.Print(events[0].Actor);
	}

	private Event[] DeserializeFromFile()
	{
		string filePath = modelDir + "test.bin";

		File nGramFile = new File();
		var ngramFileOpened = nGramFile.Open(filePath, File.ModeFlags.Read);

		if (ngramFileOpened == Error.FileNotFound)
		{
			GD.Print("Could not find file " + filePath);
			return default;
		}

		byte[] bytes = nGramFile.GetBuffer((int)nGramFile.GetLen());
		var events = MessagePackSerializer.Deserialize<Event[]>(bytes);
		nGramFile.Close();
		return events;
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
