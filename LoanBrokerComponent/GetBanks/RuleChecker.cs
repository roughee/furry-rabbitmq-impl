using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetBanks
{
    class RuleChecker
    {
        public List<String> GetBankList(int creditScore)
        {
            var bank1 = "bank1";
            var bank2 = "bank2";
            var bank3 = "bank3";
            var banksList = new List<String>();
            if (creditScore > 600)
            {
                banksList.Add(bank1);
                banksList.Add(bank2);
                banksList.Add(bank3);
            }
            if (creditScore > 300 && creditScore < 600)
            {
                banksList.Add(bank1);
                banksList.Add(bank2);
            }
            if (creditScore < 300)
            {
                banksList.Add(bank1);
            }
            return banksList;
        }
    }
}
