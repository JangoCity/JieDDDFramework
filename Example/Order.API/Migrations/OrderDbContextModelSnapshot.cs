﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Order.Domain.DbContexts;

namespace Order.API.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    partial class OrderDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Order.Domain.Aggregates.BuyerAggregate.Buyer", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("buyers");
                });

            modelBuilder.Entity("Order.Domain.Aggregates.BuyerAggregate.PaymentMethod", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64);

                    b.Property<string>("BuyerId")
                        .IsRequired();

                    b.Property<string>("FreeCode")
                        .HasMaxLength(32);

                    b.Property<int?>("PaymentTypeId");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("paymentmethods");
                });

            modelBuilder.Entity("Order.Domain.Aggregates.BuyerAggregate.PaymentType", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("paymenttypes");
                });

            modelBuilder.Entity("Order.Domain.Aggregates.OrderAggregate.Order", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64);

                    b.Property<string>("BuyerId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<int?>("OrderStatusId");

                    b.Property<string>("PaymentMethodId");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("Order.Domain.Aggregates.OrderAggregate.OrderItem", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64);

                    b.Property<string>("OrderId");

                    b.Property<int>("ProductCount");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<decimal>("ProductPrice");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("orderItems");
                });

            modelBuilder.Entity("Order.Domain.Aggregates.OrderAggregate.OrderStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("orderstatus");
                });

            modelBuilder.Entity("Order.Domain.Aggregates.BuyerAggregate.PaymentMethod", b =>
                {
                    b.HasOne("Order.Domain.Aggregates.BuyerAggregate.Buyer")
                        .WithMany("PaymentMethods")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Order.Domain.Aggregates.BuyerAggregate.PaymentType", "PaymentType")
                        .WithMany()
                        .HasForeignKey("PaymentTypeId");
                });

            modelBuilder.Entity("Order.Domain.Aggregates.OrderAggregate.Order", b =>
                {
                    b.HasOne("Order.Domain.Aggregates.BuyerAggregate.Buyer", "Buyer")
                        .WithMany("Orders")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Order.Domain.Aggregates.OrderAggregate.OrderStatus", "OrderStatus")
                        .WithMany()
                        .HasForeignKey("OrderStatusId");

                    b.HasOne("Order.Domain.Aggregates.BuyerAggregate.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId");

                    b.OwnsOne("Order.Domain.Aggregates.OrderAggregate.Address", "Address", b1 =>
                        {
                            b1.Property<string>("OrderId");

                            b1.Property<string>("City");

                            b1.Property<string>("Country");

                            b1.Property<string>("State");

                            b1.Property<string>("Street");

                            b1.Property<string>("ZipCode");

                            b1.HasKey("OrderId");

                            b1.ToTable("orders");

                            b1.HasOne("Order.Domain.Aggregates.OrderAggregate.Order")
                                .WithOne("Address")
                                .HasForeignKey("Order.Domain.Aggregates.OrderAggregate.Address", "OrderId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Order.Domain.Aggregates.OrderAggregate.OrderItem", b =>
                {
                    b.HasOne("Order.Domain.Aggregates.OrderAggregate.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");
                });
#pragma warning restore 612, 618
        }
    }
}
