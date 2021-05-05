using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace XmlConverter
{
	class Program
	{
		static string xml = @"
<module id=""1"" >
	<name>Super Secret Prop Plane</name>
	<part pid=""1"">
		<name>Fuselage</name>
	</part>
	<part pid=""2"">
		<name>Cockpit</name>
	</part>
	<part pid=""3"">
		<name>Left Wing</name>
		<associatedParts>
			<part pid=""7"">
				<name>Outer Left Prop</name>
			</part>
			<part pid=""8"">
				<name>Inner Left Prop</name>
			</part>
		</associatedParts>
	</part>
	<part pid=""4"">
		<name>Right Wing</name>
		<associatedParts>
			<part pid=""5"">
				<name>Outer Right Prop</name>
			</part>
			<part pid=""6"">
				<name>Inner Right Prop</name>
			</part>
		</associatedParts>
	</part>

	
</module>
";
		static void Main(string[] args)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Module));
			Module module = null;
			using (StringReader reader = new StringReader(xml))
			{
				var test = (Module)serializer.Deserialize(reader);
				if (test != null)
				{
					module = test;
				}
				Console.WriteLine(test.Name);
				Console.WriteLine("Parts:");
				foreach (Part p in test.Parts)
				{
					Console.WriteLine(p.Name);
					if (p.AssociatedParts != null)
					{
						if (p.AssociatedParts.Count > 0)
						{
							Console.WriteLine("Associated Parts:");
							foreach (Part p2 in p.AssociatedParts)
							{
								Console.WriteLine(p2.Name);
							}
							Console.WriteLine("==============");
						}
					}
				}
				Console.WriteLine("==============");
			}
			if (module != null)
			{
				StringWriter writer = new StringWriter(new StringBuilder());
				serializer.Serialize(writer, module);
				Console.WriteLine(writer.ToString());		
			}
			Console.Read();
		}
	}

	[XmlRoot(ElementName = "module")]
	public class Module
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "name")]

		public string Name { get; set; }

		[XmlElement(ElementName = "part")]
		//these parts are directly in the module
		public List<Part> Parts { get; set; }


	}



	[XmlRoot(ElementName = "part")]
	public class Part
	{
		[XmlAttribute(AttributeName = "pid")]
		public string PartId { get; set; }

		[XmlElement(ElementName = "name")]
		public string Name { get; set; }

		[XmlArray(ElementName = "associatedParts")]
		[XmlArrayItem("part")]
		//Theses parts are in a separate "associatedParts" element
		public List<Part> AssociatedParts { get; set; }
	}
}
