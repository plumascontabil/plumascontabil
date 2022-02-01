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
    [Migration("20220126170021_ImportacaoOfx")]
    partial class ImportacaoOfx
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
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

            modelBuilder.Entity("Demonstrativo.Models.ContaContabil", b =>
                {
                    b.Property<int>("Codigo")
                        .HasColumnType("int");

                    b.Property<int>("Classificacao")
                        .HasColumnType("int");

                    b.Property<string>("Historico")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Codigo");

                    b.ToTable("ContasContabeis");
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
                        .HasColumnType("varchar(18)");

                    b.Property<string>("RazaoSocial")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Situacao")
                        .HasColumnType("varchar(2)");

                    b.HasKey("Codigo");

                    b.ToTable("Empresas");
                });

            modelBuilder.Entity("Demonstrativo.Models.HistoricoOfx", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContaCreditoId")
                        .HasColumnType("int");

                    b.Property<int>("ContaDebitoId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContaCreditoId");

                    b.HasIndex("ContaDebitoId");

                    b.ToTable("HistoricosOfx");
                });

            modelBuilder.Entity("Demonstrativo.Models.ImportacaoOfx", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<int>("HistoricoOfxId")
                        .HasColumnType("int");

                    b.Property<string>("TipoLancamento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ValorOfx")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("HistoricoOfxId");

                    b.ToTable("Ofxs");
                });

            modelBuilder.Entity("Demonstrativo.Models.ItemVenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantidade")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("VendaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.HasIndex("VendaId");

                    b.ToTable("ItensVendas");
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

            modelBuilder.Entity("Demonstrativo.Models.LancamentoPadrao", b =>
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

                    b.Property<int?>("TipoContaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("TipoContaId");

                    b.ToTable("Contas");
                });

            modelBuilder.Entity("Demonstrativo.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("Demonstrativo.Models.ProvisoesDepreciacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Apurar")
                        .HasColumnType("bit");

                    b.Property<bool>("CalcularCompensacao")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataCompetencia")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("DecimoTerceiro")
                        .HasColumnType("decimal(11,2)");

                    b.Property<decimal?>("Depreciacao")
                        .HasColumnType("decimal(11,2)");

                    b.Property<int>("EmpresaId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Ferias")
                        .HasColumnType("decimal(11,2)");

                    b.Property<decimal?>("SaldoPrejuizo")
                        .HasColumnType("decimal(11,2)");

                    b.HasKey("Id");

                    b.HasIndex("DataCompetencia");

                    b.HasIndex("EmpresaId");

                    b.ToTable("ProvisoesDepreciacoes");
                });

            modelBuilder.Entity("Demonstrativo.Models.TipoConta", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TiposContas");
                });

            modelBuilder.Entity("Demonstrativo.Models.Venda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataCompetencia")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmpresaId")
                        .HasColumnType("int");

                    b.Property<string>("Observacao")
                        .HasColumnType("varchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DataCompetencia");

                    b.HasIndex("EmpresaId");

                    b.ToTable("Vendas");
                });

            modelBuilder.Entity("Demonstrativo.Models.HistoricoOfx", b =>
                {
                    b.HasOne("Demonstrativo.Models.ContaContabil", "ContaCredito")
                        .WithMany("HistoricosCreditosOfx")
                        .HasForeignKey("ContaCreditoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Demonstrativo.Models.ContaContabil", "ContaDebito")
                        .WithMany("HistoricosDebitosOfx")
                        .HasForeignKey("ContaDebitoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ContaCredito");

                    b.Navigation("ContaDebito");
                });

            modelBuilder.Entity("Demonstrativo.Models.ImportacaoOfx", b =>
                {
                    b.HasOne("Demonstrativo.Models.HistoricoOfx", "HistoricoOfx")
                        .WithMany("ImportacoesOfx")
                        .HasForeignKey("HistoricoOfxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HistoricoOfx");
                });

            modelBuilder.Entity("Demonstrativo.Models.ItemVenda", b =>
                {
                    b.HasOne("Demonstrativo.Models.Produto", "Produto")
                        .WithMany("ItemVendas")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Demonstrativo.Models.Venda", "Venda")
                        .WithMany("ItemVendas")
                        .HasForeignKey("VendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");

                    b.Navigation("Venda");
                });

            modelBuilder.Entity("Demonstrativo.Models.Lancamento", b =>
                {
                    b.HasOne("Demonstrativo.Models.LancamentoPadrao", "Conta")
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

            modelBuilder.Entity("Demonstrativo.Models.LancamentoPadrao", b =>
                {
                    b.HasOne("Demonstrativo.Models.Categoria", "Categoria")
                        .WithMany("Conta")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Demonstrativo.Models.TipoConta", "Tipo")
                        .WithMany("Contas")
                        .HasForeignKey("TipoContaId");

                    b.Navigation("Categoria");

                    b.Navigation("Tipo");
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

            modelBuilder.Entity("Demonstrativo.Models.Venda", b =>
                {
                    b.HasOne("Demonstrativo.Models.Competencia", "Competencia")
                        .WithMany("Vendas")
                        .HasForeignKey("DataCompetencia")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Demonstrativo.Models.Empresa", "Empresa")
                        .WithMany("Vendas")
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

                    b.Navigation("Vendas");
                });

            modelBuilder.Entity("Demonstrativo.Models.ContaContabil", b =>
                {
                    b.Navigation("HistoricosCreditosOfx");

                    b.Navigation("HistoricosDebitosOfx");
                });

            modelBuilder.Entity("Demonstrativo.Models.Empresa", b =>
                {
                    b.Navigation("Lancamentos");

                    b.Navigation("ProvisoesDepreciacoes");

                    b.Navigation("Vendas");
                });

            modelBuilder.Entity("Demonstrativo.Models.HistoricoOfx", b =>
                {
                    b.Navigation("ImportacoesOfx");
                });

            modelBuilder.Entity("Demonstrativo.Models.LancamentoPadrao", b =>
                {
                    b.Navigation("Lancamentos");
                });

            modelBuilder.Entity("Demonstrativo.Models.Produto", b =>
                {
                    b.Navigation("ItemVendas");
                });

            modelBuilder.Entity("Demonstrativo.Models.TipoConta", b =>
                {
                    b.Navigation("Contas");
                });

            modelBuilder.Entity("Demonstrativo.Models.Venda", b =>
                {
                    b.Navigation("ItemVendas");
                });
#pragma warning restore 612, 618
        }
    }
}
