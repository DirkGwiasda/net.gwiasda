﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public interface IDeleteCategoryWorkflow
    {
        Task DeleteAsync(FinanceCategory category);
    }
}