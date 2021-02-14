﻿using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Aggregates
{
  public class Room : BaseEntity<int>, IAggregateRoot
  {
    public Room(int id, string name)
    {
      Id = id;
      Name = name;
    }

    private Room() // required for EF
    {
    }
    public string Name { get; private set; }

    public override string ToString()
    {
      return Name.ToString();
    }

    public Room UpdateName(string name)
    {
      Name = name;
      return this;
    }
  }
}
