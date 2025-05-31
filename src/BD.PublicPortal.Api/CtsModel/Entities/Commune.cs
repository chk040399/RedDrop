namespace BD.PublicPortal.Api.CtsModel.Entities;

  public class Commune
  {
      public int Id { get; private set; }
      public string Name { get; private set; } = string.Empty;
      public int WilayaId { get; private set; } // Foreign key
      
      // Navigation property
      public Wilaya Wilaya { get; private set; } = null!;
      
      private Commune() { } // For EF Core

  }
