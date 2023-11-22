﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransporteWeb.Models;

#nullable disable

namespace TransporteWeb.Migrations
{
    [DbContext(typeof(Contexto))]
    [Migration("20231122001553_Iniciandov93")]
    partial class Iniciandov93
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TransporteWeb.Models.Agendamento", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("IdEstudante")
                        .HasColumnType("int");

                    b.Property<int>("IdVeiculo")
                        .HasColumnType("int");

                    b.Property<DateTime>("data")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("horario")
                        .HasColumnType("time");

                    b.HasKey("id");

                    b.HasIndex("IdEstudante");

                    b.HasIndex("IdVeiculo");

                    b.ToTable("Agendamentos");
                });

            modelBuilder.Entity("TransporteWeb.Models.Curso", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("id");

                    b.ToTable("Cursos");
                });

            modelBuilder.Entity("TransporteWeb.Models.Estudante", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("IdCurso")
                        .HasColumnType("int");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("telefone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("id");

                    b.HasIndex("IdCurso");

                    b.ToTable("Estudantes");
                });

            modelBuilder.Entity("TransporteWeb.Models.Veiculo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("nomeveiculo")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("placa")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<int>("vagas")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Veiculos");
                });

            modelBuilder.Entity("TransporteWeb.Models.Agendamento", b =>
                {
                    b.HasOne("TransporteWeb.Models.Estudante", "estudante")
                        .WithMany()
                        .HasForeignKey("IdEstudante")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransporteWeb.Models.Veiculo", "veiculo")
                        .WithMany()
                        .HasForeignKey("IdVeiculo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("estudante");

                    b.Navigation("veiculo");
                });

            modelBuilder.Entity("TransporteWeb.Models.Estudante", b =>
                {
                    b.HasOne("TransporteWeb.Models.Curso", "curso")
                        .WithMany()
                        .HasForeignKey("IdCurso")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("curso");
                });
#pragma warning restore 612, 618
        }
    }
}
