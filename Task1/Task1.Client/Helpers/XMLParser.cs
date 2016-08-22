using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Task1.Client.Models;
using Task1.Common;

namespace Task1.Client.Helpers
{
    public static class XMLParser
    {
        public static IEnumerable<GroupModel> GetGroups()
        {
            var xml = File.ReadAllText("settings.xml", Encoding.UTF8);
            var doc = XDocument.Parse(xml);
            var elemtents = doc.Element("groups")?.Elements("group");

            return elemtents?.Select(s => new GroupModel
            {
                Name = s.Attribute("name").Value,
                Ranges = s.Elements("job").Select(a =>
                                 new NumberRange
                                 {
                                     Id = Convert.ToInt32(a.Attribute("id")?.Value),
                                     End = Convert.ToInt32(a.Attribute("end")?.Value),
                                     Start = Convert.ToInt32(a.Attribute("start")?.Value),
                                     Count = Convert.ToInt32(a.Attribute("count")?.Value)
                                 })
            });
        }
    }
}