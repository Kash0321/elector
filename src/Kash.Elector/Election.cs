﻿using Kash.CrossCutting.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector
{
    public class Election
    {
        public int Id { get; protected set; }

        public string Name { get; protected set; }

        public Election(int id, string name)
        {
            Check.NotEmpty(name, nameof(name));

            Id = id;
            Name = name;
        }
    }
}