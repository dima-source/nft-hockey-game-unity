using NearClientUnity.Utilities;

namespace Near
{
    public static class NearUtils
    {
        public static readonly ulong GasMakeAvailable = 300_000_000_000_000;
        public static readonly ulong GasMove = 100_000_000_000_000;
        
        public static readonly int MinterRoyaltyCap = 2000;
        public static readonly ulong Gas = 200000000000000;
        
        private static readonly UInt128 NearNominationExp = UInt128.Parse("1000000000000000000000000");

        public static UInt128 ParseNearAmount(string amount)
        {
            return UInt128.Parse(amount) * NearNominationExp;
        }
        
        public static double FormatNearAmount(UInt128 amount)
        {
            int accuracy = 1000000000;
            
            UInt128 accuracyUInt128 = new UInt128(accuracy);
            
            amount *= accuracyUInt128;
            double result = double.Parse((amount / NearNominationExp).ToString()) / accuracy;
            return result;
        }
    }
}