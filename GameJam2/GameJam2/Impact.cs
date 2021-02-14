using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2
{
    public class Impact
    {
        private int[] data;

        public Impact(string id, string type)
        {
            this.data = ParseDataList(id, type);
        }

        public int[] ParseDataList(string id, string type)
        {
            string tempStr = String.Concat(TraitementConsole.LireFichier("card" + id + "impact" + type + ".txt"));
            int[] intArray = tempStr.Split(',').Select(int.Parse).ToArray();
            return intArray;
        }

        public int[] ImpactData
        {
            get { return this.data; }
        }
    }
}