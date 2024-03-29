﻿// <auto-generated />
using System;
using Demonstrativo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Demonstrativo.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20211118184244_CorrecaoTabProvisoesDepreciacao")]
    partial class CorrecaoTabProvisoesDepreciacao
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Demonstrativo.Models.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Demonstrativo.Models.Competencia", b =>
                {
                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.HasKey("Data");

                    b.ToTable("Competencias");
                });

            modelBuilder.Entity("Demonstrativo.Models.Conta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<int?>("Codigo")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(70)");

                    b.Property<int?>("LancamentoCredito")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int?>("LancamentoDebito")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int?>("LancamentoHistorico")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Contas");
                });

            modelBuilder.Entity("Demonstrativo.Models.Empresa", b =>
                {
                    b.Property<int>("Codigo")
                        .HasColumnType("int");

                    b.Property<string>("Apelido")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasColumnType("varchar(14)");

                    b.Property<string>("RazaoSocial")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Codigo");

                    b.ToTable("Empresas");
                });

            modelBuilder.Entity("Demonstrativo.Models.Lancamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataCompetencia")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("EmpresaId")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(11,2)");

                    b.HasKey("Id");

                    b.HasIndex("ContaId");

                    b.HasIndex("DataCompetencia");

                    b.HasIndex("EmpresaId");

                    b.ToTable("Lancamentos");
                });

            modelBuilder.Entity("Demonstrativo.Models.ProvisoesDepreciacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataCompetencia")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("DecimoTerceiro")
                        .IsRequired()
                        .HasColumnType("decimal(11,2)");

                    b.Property<decimal?>("Depreciacao")
                        .IsRequired()
                        .HasColumnType("decimal(11,2)");

                    b.Property<int>("EmpresaId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Ferias")
                        .IsRequired()
                        .HasColumnType("decimal(11,2)");

                    b.Property<decimal?>("SaldoPrejuizo")
                        .IsRequired()
                        .HasColumnType("decimal(11,2)");

                    b.HasKey("Id");

                    b.HasIndex("DataCompetencia");

                    b.HasIndex("EmpresaId");

                    b.ToTable("ProvisoesDepreciacoes");
                });

            modelBuilder.Entity("Demonstrativo.Models.Conta", b =>
                {
                    b.HasOne("Demonstrativo.Models.Categoria", "Categoria")
                        .WithMany("Conta")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("Demonstrativo.Models.Lancamento", b =>
                {
                    b.HasOne("Demonstrativo.Models.Conta", "Conta")
                        .WithMany("Lancamentos")
                        .HasForeignKey("ContaId");

                    b.HasOne("Demonstrativo.Models.Competencia", "Competencia")
                        .WithMany("Lancamentos")
                        .HasForeignKey("DataCompetencia")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Demonstrativo.Models.Empresa", "Empresa")
                        .WithMany("Lancamentos")
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competencia");

                    b.Navigation("Conta");

                    b.Navigation("Empresa");
                });

            modelBuilder.Entity("Demonstrativo.Models.ProvisoesDepreciacao", b =>
                {
                    b.HasOne("Demonstrativo.Models.Competencia", "Competencia")
                        .WithMany("ProvisoesDepreciacoes")
                        .HasForeignKey("DataCompetencia")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Demonstrativo.Models.Empresa", "Empresa")
                        .WithMany("ProvisoesDepreciacoes")
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competencia");

                    b.Navigation("Empresa");
                });

            modelBuilder.Entity("Demonstrativo.Models.Categoria", b =>
                {
                    b.Navigation("Conta");
                });

            modelBuilder.Entity("Demonstrativo.Models.Competencia", b =>
                {
                    b.Navigation("Lancamentos");

                    b.Navigation("ProvisoesDepreciacoes");
                });

            modelBuilder.Entity("Demonstrativo.Models.Conta", b =>
                {
                    b.Navigation("Lancamentos");
                });

            modelBuilder.Entity("Demonstrativo.Models.Empresa", b =>
                {
                    b.Navigation("Lancamentos");

                    b.Navigation("ProvisoesDepreciacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
