
namespace USO.Core
{
    using USO.Core.Enums;

    public struct ComputedHash
    {
        public string ComputedHashCode;
        public HashingAlgorithm HashingAlgorithmUsed;

        public ComputedHash(string computedHashCode, HashingAlgorithm hashingAlgorithmUsed)
        {
            ComputedHashCode = computedHashCode;
            HashingAlgorithmUsed = hashingAlgorithmUsed;
        }
    }
}
