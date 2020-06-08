using System;
using System.Collections.Generic;
using System.Text;
using ILOG.CPLEX;
using ILOG.Concert;
using System.Linq;
using System.Threading.Tasks;
using Core_Form;

namespace MLMDelivery.Classes
{
    public class CallBack_Status: Cplex.MIPCallback
    {
        private double gap = new double();
        private System.Windows.Forms.ProgressBar pbar = new System.Windows.Forms.ProgressBar();
        public CallBack_Status(System.Windows.Forms.ProgressBar progressBar1)
        {
            pbar = progressBar1;
        }
        public async override void Main()
        {
            if (HasIncumbent())
            {
                gap = GetMIPRelativeGap();
                await Task.Run(()=> DoWork());
            }
        }
        public void DoWork()
        {
            /* (pbar.InvokeRequired)
            {
                var bilmemne = new Network_Design_Form_Core.SafeCallDelegate(DoWork);
                pbar.Invoke(bilmemne, new object[] { gap });
            }
            else
            {
                if (gap < 100) pbar.Value = Convert.ToInt32(100.01 - Convert.ToInt32(Math.Ceiling(gap)));
            }*/
            
        }
    }
}
