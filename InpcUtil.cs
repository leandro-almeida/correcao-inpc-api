namespace CorrecaoApi
{
    public class InpcUtil
    {
        public const int AnoInicioSerieInpc = 1979;
        public const int MesInicioSerieInpc = 3;

        private static DateTime DataMinimaInpc = new DateTime(InpcUtil.AnoInicioSerieInpc, InpcUtil.MesInicioSerieInpc, 1);

        public static bool PossuiIndiceInpc(int ano, int mes)
        {
            var dataInformada = new DateTime(ano, mes, 1);
            return dataInformada >= DataMinimaInpc;
        }
    }
}
