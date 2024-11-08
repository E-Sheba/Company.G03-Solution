﻿using Company.G03.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G03.DAL.Data.Configurations
{
    internal class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(E => E.Id);
            builder.Property(e => e.Salary).HasColumnType("decimal(18,2)");
            builder.Property(e => e.DateOfCreation).HasDefaultValueSql("GETDate()");
        }
    }
}
