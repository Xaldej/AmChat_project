﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands.FromServerToClient
{
    public class CorrectLogin : Command
    {
        public override string Name => nameof(CorrectLogin).ToLower();
    }
}