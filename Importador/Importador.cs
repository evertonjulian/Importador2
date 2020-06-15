using Importador.Classes;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace Importador
{
    public partial class Importador : ServiceBase
    {
        readonly FileSystemWatcher watcher;
        static readonly string filtro = "*.dat";
        static readonly string diretorioRaiz = Directory.GetDirectoryRoot(Assembly.GetExecutingAssembly().Location);
        static readonly string diretorioEntrada = Path.Combine(diretorioRaiz, ConfigurationManager.AppSettings["DiretorioEntrada"]);
        static readonly string diretorioSaida = Path.Combine(diretorioRaiz, ConfigurationManager.AppSettings["DiretorioSaida"]);
        static readonly string diretorioLidos = Path.Combine(diretorioRaiz, ConfigurationManager.AppSettings["DiretorioLidos"]);

        public Importador()
        {
            InitializeComponent();
            VerificarDiretorios();

            watcher = new FileSystemWatcher(diretorioEntrada, filtro)
            {
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName
            };
            watcher.Created += Watcher_Created;
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            var caminhoArquivo = e.FullPath;
            var conteudoArquivo = Arquivo.LerArquivo(caminhoArquivo);
            var processamento = Arquivo.ProcessarArquivo(conteudoArquivo);
            
            var nomeArquivo = Path.GetFileNameWithoutExtension(caminhoArquivo);
            var caminhoSaida = $"{diretorioSaida}\\{nomeArquivo}.result.dat";

            processamento.EscreverResultado(caminhoSaida);

            File.Move(caminhoArquivo, $"{diretorioLidos}\\{Path.GetFileName(caminhoArquivo)}");
        }

        private void VerificarDiretorios()
        {
            if (!Directory.Exists(diretorioEntrada))
            {
                Directory.CreateDirectory(diretorioEntrada);
            }

            if (!Directory.Exists(diretorioSaida))
            {
                Directory.CreateDirectory(diretorioSaida);
            }

            if (!Directory.Exists(diretorioLidos))
            {
                Directory.CreateDirectory(diretorioLidos);
            }
        }

        protected override void OnStart(string[] args)
        {
        }
        
        protected override void OnStop()
        {
            watcher.Dispose();
        }

    }
}
