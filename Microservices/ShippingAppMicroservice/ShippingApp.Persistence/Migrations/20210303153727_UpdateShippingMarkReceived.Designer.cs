﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShippingApp.Persistence.DBContext;

namespace ShippingApp.Persistence.Migrations
{
    [DbContext(typeof(ShippingAppDbContext))]
    [Migration("20210303153727_UpdateShippingMarkReceived")]
    partial class UpdateShippingMarkReceived
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ShippingApp.Domain.Entities.Config", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Descriptions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.Country", b =>
                {
                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryCode");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.MovementRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("MMRQ");

                    b.HasKey("Id");

                    b.ToTable("MovementRequests");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.MovementRequestDetail", b =>
                {
                    b.Property<int>("WorkOrderId")
                        .HasColumnType("int");

                    b.Property<int>("MovementRequestId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("WorkOrderId", "MovementRequestId", "ProductId");

                    b.HasIndex("MovementRequestId");

                    b.HasIndex("ProductId");

                    b.ToTable("MovementRequestDetails");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("PROD");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QtyPerPackage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ReceivedMark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("REMARK");

                    b.HasKey("Id");

                    b.ToTable("ReceivedMarks");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ReceivedMarkMovement", b =>
                {
                    b.Property<int>("ReceivedMarkId")
                        .HasColumnType("int");

                    b.Property<int>("MovementRequestId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ReceivedMarkId", "MovementRequestId", "ProductId");

                    b.HasIndex("MovementRequestId");

                    b.HasIndex("ProductId");

                    b.ToTable("ReceivedMarkMovements");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ReceivedMarkPrinting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("PrintCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("PrintingBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PrintingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("RePrintingBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RePrintingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceivedMarkId")
                        .HasColumnType("int");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("New");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ReceivedMarkId");

                    b.ToTable("ReceivedMarkPrintings");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ReceivedMarkSummary", b =>
                {
                    b.Property<int>("ReceivedMarkId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalPackage")
                        .HasColumnType("int");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("int");

                    b.HasKey("ReceivedMarkId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ReceivedMarkSummaries");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("SHIPMARK");

                    b.HasKey("Id");

                    b.ToTable("ShippingMarks");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMarkHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActionType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ShippingMarkHistory");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMarkPrinting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PrintCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("PrintingBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PrintingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("RePrintingBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RePrintingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Revision")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.Property<int>("ShippingMarkId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("New");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShippingMarkId");

                    b.ToTable("ShippingMarkPrintings");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMarkReceived", b =>
                {
                    b.Property<int>("ShippingMarkId")
                        .HasColumnType("int");

                    b.Property<int>("ReceivedMarkId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ShippingMarkId", "ReceivedMarkId");

                    b.HasIndex("ReceivedMarkId");

                    b.ToTable("ShippingMarkReceived");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMarkShipping", b =>
                {
                    b.Property<int>("ShippingMarkId")
                        .HasColumnType("int");

                    b.Property<int>("ShippingRequestId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ShippingMarkId", "ShippingRequestId", "ProductId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShippingRequestId");

                    b.ToTable("ShippingMarkShippings");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMarkSummary", b =>
                {
                    b.Property<int>("ShippingMarkId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalPackage")
                        .HasColumnType("int");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("int");

                    b.HasKey("ShippingMarkId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ShippingMarkSummaries");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("SHIPPL");

                    b.Property<string>("PurchaseOrder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SalesID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SemlineNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("ShippingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ShippingPlans");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingPlanDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("ShippingMode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShippingPlanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShippingPlanId");

                    b.ToTable("ShippingPlanDetails");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("SHIPRQ");

                    b.Property<string>("PurchaseOrder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SalesID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SemlineNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("ShippingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ShippingRequestLogisticId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ShippingRequests");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingRequestDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("ShippingMode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShippingRequestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShippingRequestId");

                    b.ToTable("ShippingRequestDetails");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingRequestLogistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BillToCustomer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomDeclarationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("GrossWeight")
                        .HasColumnType("real");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverCustomer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShippingRequestId")
                        .HasColumnType("int");

                    b.Property<string>("TrackingNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ShippingRequestId")
                        .IsUnique();

                    b.ToTable("ShippingRequestLogistics");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.WorkOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("WO");

                    b.Property<string>("RefId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WorkOrders");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.WorkOrderDetail", b =>
                {
                    b.Property<int>("WorkOrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("WorkOrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("WorkOrderDetails");
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.MovementRequestDetail", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.MovementRequest", "MovementRequest")
                        .WithMany("MovementRequestDetails")
                        .HasForeignKey("MovementRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("MovementRequestDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.WorkOrder", "WorkOrder")
                        .WithMany("MovementRequestDetails")
                        .HasForeignKey("WorkOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ReceivedMarkMovement", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.MovementRequest", "MovementRequest")
                        .WithMany("ReceivedMarkMovements")
                        .HasForeignKey("MovementRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("ReceivedMarkMovements")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ReceivedMark", "ReceivedMark")
                        .WithMany("ReceivedMarkMovements")
                        .HasForeignKey("ReceivedMarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ReceivedMarkPrinting", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("ReceivedMarkPrintings")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ReceivedMark", "ReceivedMark")
                        .WithMany("ReceivedMarkPrintings")
                        .HasForeignKey("ReceivedMarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ReceivedMarkSummary", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("ReceivedMarkSummaries")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ReceivedMark", "ReceivedMark")
                        .WithMany("ReceivedMarkSummaries")
                        .HasForeignKey("ReceivedMarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMarkPrinting", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("ShippingMarkPrintings")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ShippingMark", "ShippingMark")
                        .WithMany("ShippingMarkPrintings")
                        .HasForeignKey("ShippingMarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMarkReceived", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.ReceivedMark", "ReceivedMark")
                        .WithMany("ShippingMarkReceiveds")
                        .HasForeignKey("ReceivedMarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ShippingMark", "ShippingMark")
                        .WithMany("ShippingMarkReceiveds")
                        .HasForeignKey("ShippingMarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMarkShipping", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("ShippingMarkShippings")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ShippingMark", "ShippingMark")
                        .WithMany("ShippingMarkShippings")
                        .HasForeignKey("ShippingMarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ShippingRequest", "ShippingRequest")
                        .WithMany("ShippingMarkShippings")
                        .HasForeignKey("ShippingRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingMarkSummary", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("ShippingMarkSummaries")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ShippingMark", "ShippingMark")
                        .WithMany("ShippingMarkSummaries")
                        .HasForeignKey("ShippingMarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingPlanDetail", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("ShippingPlanDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ShippingPlan", "ShippingPlan")
                        .WithMany("ShippingPlanDetails")
                        .HasForeignKey("ShippingPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingRequestDetail", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("ShippingRequestDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.ShippingRequest", "ShippingRequest")
                        .WithMany("ShippingRequestDetails")
                        .HasForeignKey("ShippingRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.ShippingRequestLogistic", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.ShippingRequest", "ShippingRequest")
                        .WithOne("ShippingRequestLogistic")
                        .HasForeignKey("ShippingApp.Domain.Entities.ShippingRequestLogistic", "ShippingRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShippingApp.Domain.Entities.WorkOrderDetail", b =>
                {
                    b.HasOne("ShippingApp.Domain.Entities.Product", "Product")
                        .WithMany("WorkOrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShippingApp.Domain.Entities.WorkOrder", "WorkOrder")
                        .WithMany("WorkOrderDetails")
                        .HasForeignKey("WorkOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
