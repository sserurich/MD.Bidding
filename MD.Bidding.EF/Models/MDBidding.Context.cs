﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MD.Bidding.EF.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using MD.Bidding.EF.Context;

    public partial class BiddingEntities : DbContext, IDbContext
    {
        public BiddingEntities()
            : base("name=BiddingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Bid> Bids { get; set; }
        public virtual DbSet<ExtensionType> ExtensionTypes { get; set; }
        public virtual DbSet<MediaType> MediaTypes { get; set; }
        public virtual DbSet<Media> Media { get; set; }
    
        public virtual int AspNetUserRole_Create(string userId, string roleId)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(string));
    
            var roleIdParameter = roleId != null ?
                new ObjectParameter("RoleId", roleId) :
                new ObjectParameter("RoleId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AspNetUserRole_Create", userIdParameter, roleIdParameter);
        }
    
        public virtual int AspNetUserRole_Purge(string userId, string roleId)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(string));
    
            var roleIdParameter = roleId != null ?
                new ObjectParameter("RoleId", roleId) :
                new ObjectParameter("RoleId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AspNetUserRole_Purge", userIdParameter, roleIdParameter);
        }
    
        public virtual ObjectResult<Media_GetDescendants_Result> Media_GetDescendants(Nullable<long> mediaId)
        {
            var mediaIdParameter = mediaId.HasValue ?
                new ObjectParameter("MediaId", mediaId) :
                new ObjectParameter("MediaId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Media_GetDescendants_Result>("Media_GetDescendants", mediaIdParameter);
        }
    
        public virtual ObjectResult<string> Media_GetPath(Nullable<long> mediaId)
        {
            var mediaIdParameter = mediaId.HasValue ?
                new ObjectParameter("MediaId", mediaId) :
                new ObjectParameter("MediaId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("Media_GetPath", mediaIdParameter);
        }
    
        public virtual ObjectResult<string> Media_GetPathCsvMediaId(Nullable<long> mediaId)
        {
            var mediaIdParameter = mediaId.HasValue ?
                new ObjectParameter("MediaId", mediaId) :
                new ObjectParameter("MediaId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("Media_GetPathCsvMediaId", mediaIdParameter);
        }
    
        public virtual int Media_SetPath(Nullable<long> mediaId)
        {
            var mediaIdParameter = mediaId.HasValue ?
                new ObjectParameter("MediaId", mediaId) :
                new ObjectParameter("MediaId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Media_SetPath", mediaIdParameter);
        }
    }
}