using System;
using System.Collections.Generic;
using BarBot.Core.Model;
using Newtonsoft.Json;

namespace BarBot.Core.WebSocket
{
    public class Message : JsonModelObject
    {
        public string Type { get; set; }
        public string Command { get; set; }
        public string Result { get; set; }

        [JsonConverter(typeof(DictionaryOrEmptyArrayConverter<String, Object>))]
        public Dictionary<String, Object> Data { get; set; }

        public Message() { }

        public Message(string result, Dictionary<String, Object> data)
        {
            Result = result;
            Data = data;
        }

        public Message(string type, string command, Dictionary<String, Object> data)
        {
            Type = type;
            Command = command;
            Data = data;
        }

        public Message(string type, string command, string result, Dictionary<String, Object> data)
        {
            Type = type;
            Command = command;
            Result = result;
            Data = data;
        }

        public Message(string json)
        {
            var m = (Message)parseJSON(json, typeof(Message));
            Type = m.Type;
            Command = m.Command;
            Result = m.Result;
            Data = m.Data;
        }
    }
}
