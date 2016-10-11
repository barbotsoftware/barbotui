using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BarBot.Model;

namespace BarBot.WebSocket
{
    public class Message : JsonModelObject
    {
        public string Type { get; set; }
        public string Command { get; set; }
        public Dictionary<String, Object> Data { get; set; }

        public Message() { }

        public Message(string type, string command, Dictionary<String, Object> data)
        {
            Type = type;
            Command = command;
            Data = data;
        }

        public Message(string json)
        {
            var m = (Message)parseJSON(json, typeof(Message));
            Type = m.Type;
            Command = m.Command;
            Data = m.Data;
        }
    }
}
