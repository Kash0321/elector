using Kash.CrossCutting.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector
{
    public class District
    {
        public Election Election { get; protected set; }

        public int Id { get; protected set; }

        public string Name { get; protected set; }

        public int Seats { get; protected set; }

        public District(Election election, int id, string name, int seats)
        {
            Check.NotEmpty(name, nameof(name));

            Election = election;
            Id = id;
            Name = name;
            Seats = seats;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
