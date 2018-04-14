﻿using EncodedComparer.Shared.Commands;
using Newtonsoft.Json;

namespace EncodedComparer.Domain.Commands
{
    public class SetRightCommand : ICommand
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Base64EncodedData { get; set; }
    }
}
