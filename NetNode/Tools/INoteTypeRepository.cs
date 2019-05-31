using NetNode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetNode.Tools
{
    public interface INoteTypeRepository
    {
        Task<List<NoteType>> ListAsync();
    }
}
