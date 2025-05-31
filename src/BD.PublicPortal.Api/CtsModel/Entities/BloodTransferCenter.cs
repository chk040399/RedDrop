﻿using System;
using System.Collections.Generic;

namespace BD.PublicPortal.Api.CtsModel.Entities;

  public class BloodTransferCenter
  {
      public Guid Id { get; private set; }
      public string Name { get; private set; } = string.Empty;
      public string Address { get; private set; } = string.Empty;
      public string Email { get; private set; } = string.Empty;
      public string PhoneNumber { get; private set; } = string.Empty;
      
      // Foreign key for Wilaya
      public int WilayaId { get; private set; }
      
      // Navigation property
      public Wilaya Wilaya { get; private set; } = null!;
      
      private BloodTransferCenter() { } // For EF Core
      
      public BloodTransferCenter(
          string name,
          string address,
          string email,
          string phoneNumber,
          int wilayaId)
      {
          Id = Guid.NewGuid();
          Name = name;
          Address = address;
          Email = email;
          PhoneNumber = phoneNumber;
          WilayaId = wilayaId;
      }
      
      public void UpdateDetails(
          string? name = null,
          string? address = null,
          string? email = null,
          string? phoneNumber = null,
          int? wilayaId = null)
      {
          if (name != null) Name = name;
          if (address != null) Address = address;
          if (email != null) Email = email;
          if (phoneNumber != null) PhoneNumber = phoneNumber;
          if (wilayaId != null) WilayaId = wilayaId.Value;
      }
  }
