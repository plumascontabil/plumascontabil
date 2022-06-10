﻿using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class LancamentoRepository : Repository, ILancamentoRepository
    {
        private readonly DbSet<Lancamento> _lancamento;

        public LancamentoRepository(PContext context) : base(context)
        {

            _lancamento = Context.Set<Lancamento>();
        }

        public async Task<Lancamento> GetById(int? id)
        {
            var lancamento = await _lancamento
                .FirstOrDefaultAsync(m => m.Id == id);
            return lancamento;
        }

        public bool GetByDataCompetencia(DateTime competencia)
        {
            var lancamento = _lancamento
                .Any(l => l.DataCompetencia == competencia);
            return lancamento;
        }

        public List<Lancamento> GetByEmpresaIdDataCompetencia(int? empresasId, DateTime? competenciasId)
        {
            var lancamento = _lancamento
                .Where(x => x.EmpresaId == empresasId && x.DataCompetencia == competenciasId)
                    .ToList();
            return lancamento;
        }
        
        public async Task<bool> Adicionar(Lancamento lancamento)
        {
            _lancamento.Add(lancamento);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(Lancamento lancamento)
        {
            _lancamento.Update(lancamento);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var lancamento = await _lancamento.FindAsync(id);
            _lancamento.Remove(lancamento);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}