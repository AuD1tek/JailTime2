using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Logger = Rocket.Core.Logging.Logger;

namespace JailTime2.Core
{
    public class XMLDatabase
    {
        public string DatabasePath = JailTimePlugin.Instance.Configuration.Instance.DatabasePath.Replace("%rocket%", System.Environment.CurrentDirectory);


        public XMLDatabase()
        {
            if (!File.Exists(DatabasePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath));
                using (XmlTextWriter writter = new XmlTextWriter(DatabasePath, Encoding.UTF8))
                {
                    writter.WriteStartDocument();
                    writter.WriteStartElement("Prisoners");
                    writter.WriteEndElement();
                }
            }
        }



        public bool AddPrisoner(Player player)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(DatabasePath);

                XmlNode node = document.CreateElement("Prisoner");
                document.DocumentElement.AppendChild(node);

                XmlAttribute steamIdAttribute = document.CreateAttribute("SteamID");
                steamIdAttribute.Value = player.SteamId.ToString();
                node.Attributes.Append(steamIdAttribute);


                XmlAttribute cellIdAttribute = document.CreateAttribute("CellID");
                cellIdAttribute.Value = player.CellId.ToString();
                node.Attributes.Append(steamIdAttribute);

                XmlAttribute durationAttribute = document.CreateAttribute("Duration");
                durationAttribute.Value = player.Duration.ToString();
                node.Attributes.Append(durationAttribute);


                XmlAttribute xAttribute = document.CreateAttribute("X");
                xAttribute.Value = player.Position.X.ToString();
                node.Attributes.Append(xAttribute);

                XmlAttribute yAttribute = document.CreateAttribute("Y");
                xAttribute.Value = player.Position.Y.ToString();
                node.Attributes.Append(yAttribute);

                XmlAttribute zAttribute = document.CreateAttribute("Z");
                xAttribute.Value = player.Position.Z.ToString();
                node.Attributes.Append(zAttribute);

                XmlAttribute dateAttribute = document.CreateAttribute("Date");
                dateAttribute.Value = player.Date.ToString();
                node.Attributes.Append(dateAttribute);

                document.Save(DatabasePath);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "AddPrisoner >> ");
            }
            return false;
        }

        public bool RemovePrisoner(CSteamID steamID)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(DatabasePath);

                foreach (XmlNode node in document.DocumentElement.ChildNodes.Cast<XmlNode>())
                {
                    if (node.Attributes.GetNamedItem("SteamID").Value == steamID.ToString())
                    {
                        document.DocumentElement.RemoveChild(node);
                    }
                }
                document.Save(DatabasePath);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "RemovePrisoner >>");
            }
            return false;
        }

        public Player GetPrisoner(CSteamID steamID)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(DatabasePath);

                foreach (XmlNode node in document.DocumentElement.ChildNodes.Cast<XmlNode>())
                {
                    if (node.Attributes.GetNamedItem("SteamID").Value == steamID.ToString())
                    {
                        Vector3SE position = new Vector3SE(
                            x: float.Parse(node.Attributes.GetNamedItem("X").Value),
                            y: float.Parse(node.Attributes.GetNamedItem("Y").Value),
                            z: float.Parse(node.Attributes.GetNamedItem("Z").Value));
                        
                        return new Player(
                            steamId:    new CSteamID(ulong.Parse(node.Attributes.GetNamedItem("SteamID").Value)),
                            cellId:     int.Parse(node.Attributes.GetNamedItem("CellID").Value),
                            duration:   int.Parse(node.Attributes.GetNamedItem("Duration").Value),
                            position:   position,
                            date:       DateTime.Parse(node.Attributes.GetNamedItem("Date").Value));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "GetPrisoner >>");
            }
            return null;
        }


        public bool SetParameter(CSteamID steamID, string parameter, string value)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(DatabasePath);

                foreach (XmlNode node in document.DocumentElement.ChildNodes.Cast<XmlNode>())
                {
                    if (node.Attributes.GetNamedItem("SteamID").Value == steamID.ToString())
                    {
                        foreach (XmlNode node2 in node.ChildNodes.Cast<XmlNode>())
                            node.Attributes.SetNamedItem(node2.Attributes.GetNamedItem(parameter)).Value = value;
                    }
                }
                document.Save(DatabasePath);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "SetParameter >> ");
            }
            return false;
        }
    }
}
