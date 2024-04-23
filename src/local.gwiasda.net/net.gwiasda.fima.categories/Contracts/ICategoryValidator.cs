using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public interface ICategoryValidator
    {
        void ValidateCostCategory(CostCategory costCategory);
        void ValidateIncomeCategory(IncomeCategory incomeCategory);
    }

    
}