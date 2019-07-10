using Importador.Classes;
using System.ServiceProcess;
using System.Timers;

namespace Importador
{
    public partial class Service1 : ServiceBase
    {
        Timer timer;
        
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = 120000;
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            var arquivo = new Arquivo();
            arquivo.VerificarDiretorio();
        }

        protected override void OnStop()
        {
            
        }

    }
}
