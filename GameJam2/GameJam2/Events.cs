using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2
{
    public class Events
    {
        private string[] eventText;
        private string eventArt;
        private Impact[] eventImpacts;

        public Events(string id)
        {
            eventImpacts = new Impact[1000+Int32.Parse(id)];
            this.eventText = TraitementConsole.LireFichier("card"+id+"text.txt");
            this.eventArt = "card"+id+"art.art";
            this.eventImpacts[0] = new Impact(id, "yes");
            this.eventImpacts[1] = new Impact(id, "no");
        }

        public string Text
        {
            get { return String.Concat(eventText); }
        }
        public string Art
        {
            get { return eventArt; }
        }
        //Yes impact
        public int ImpactYesMoney
        {
            get { return eventImpacts[0].ImpactData[0]; }
        }
        public int ImpactYesStudents
        {
            get { return eventImpacts[0].ImpactData[1]; }
        }
        public int ImpactYesMood
        {
            get { return eventImpacts[0].ImpactData[2]; }
        }
        //No impact
        public int ImpactNoMoney
        {
            get { return eventImpacts[1].ImpactData[0]; }
        }
        public int ImpactNoStudents
        {
            get { return eventImpacts[1].ImpactData[1]; }
        }
        public int ImpactNoMood
        {
            get { return eventImpacts[1].ImpactData[2]; }
        }
    }
}
