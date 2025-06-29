using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    class Step
    {
        private List<List<Input>> stage;
        private int count;

        public Step()
        {
            this.stage = new List<List<Input>>();
            count = 0;
        }
        public Step(List<Input> newRow)
        {
            stage.Add(newRow);
            //count++;
        }
        public void addRow(List<Input> newRow)
        {
            stage.Add(newRow);
            count++;
        }
        public List<Input> stepLists() //returns elements one by one
        {
            //int i = stage.Count - count;
            //System.Windows.Forms.MessageBox.Show("Count : " + getCount());
            List<Input> temp = new List<Input>(); 
            if (getCount() >= 1)
            {
                temp = stage.First();
                stage.Remove(stage.First());
                count--;
            }
            return temp;
        }
        public List<Input> getList()
        {
            List<Input> list = new List<Input>();
            if(stage.Count () > 0) list = stage.First();
            return list;
        }
        public int getCount()
        {
            return count;
        }
        public int getStageCount()
        {
            return stage.Count;
        }
    }
}
