using System;
using System.Collections.Generic;

namespace BD.PublicPortal.Api.CtsModel.Entities;

  public class Wilaya
  {
      public int Id { get; private set; }
      public string Name { get; private set; } = string.Empty;
      // Other properties...

      // Navigation properties for one-to-many relationships
      public ICollection<Commune> Communes { get; private set; } = new List<Commune>();
      public ICollection<BloodTransferCenter> BloodTransferCenters { get; private set; } = new List<BloodTransferCenter>();

      public Wilaya(int id, string name){Id = id;Name = name;} 
      private Wilaya() { } // For EF Core
      
  }
