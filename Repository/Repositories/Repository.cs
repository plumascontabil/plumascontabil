using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class Repository
    {
        public PContext Context { get; set; }

        public Repository(PContext context)
        {
            Context = context;
        }
    }
}
