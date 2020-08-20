using Microsoft.EntityFrameworkCore;
using MVC_Personel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Personel.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("tblEmployee");
            modelBuilder.Entity<Employee>().Property(e => e.LastName).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.FirstName).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.IdentityNumber).IsRequired();
            modelBuilder.Entity<Employee>().HasIndex(e => e.IdentityNumber).IsUnique().HasName("uk_tblEmployee_identity");
            modelBuilder.Entity<Employee>().Property(e => e.PhoneNumber).IsRequired();
            modelBuilder.Entity<Employee>().HasIndex(e => e.PhoneNumber).IsUnique().IsUnique().HasName("uk_tblEmployee_phone");
            modelBuilder.Entity<Employee>().Property(e => e.Gender).HasMaxLength(1);
            modelBuilder.Entity<Employee>().Property(e => e.Username).IsRequired();
            modelBuilder.Entity<Employee>().HasIndex(e => e.Username).IsUnique().IsUnique().HasName("uk_tblEmployee_username");
            modelBuilder.Entity<Employee>().Property(e => e.Password).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.Address).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.RegistirationNumber).IsRequired();
            modelBuilder.Entity<Employee>().HasIndex(e => e.RegistirationNumber).IsUnique().HasName("uk_tblEmployee_reg");
            modelBuilder.Entity<Employee>().Property(e => e.DateOfStart).HasColumnType("date");
            modelBuilder.Entity<Employee>().Property(e => e.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Employee>().Property(e => e.DateOfLeave).HasColumnType("date");

            modelBuilder.Entity<Department>().ToTable("tblDepartment");
            modelBuilder.Entity<Department>().Property(d => d.Name).IsRequired();
            modelBuilder.Entity<Department>().HasIndex(d => d.Name).IsUnique().HasName("uk_tblDepartment_name");

            modelBuilder.Entity<Position>().ToTable("tblPosition");
            modelBuilder.Entity<Position>().Property(d => d.Name).IsRequired();
            modelBuilder.Entity<Position>().HasIndex(d => d.Name).IsUnique().HasName("uk_tblPosition_name");
        }
    }
}
