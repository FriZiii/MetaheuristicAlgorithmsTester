using System.ComponentModel.DataAnnotations.Schema;

namespace MetaheuristicAlgorithmsTester.Domain.Entities
{
    public class FitnessFunction
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FileName { get; set; } = default!;

        [NotMapped]
        public byte[]? DllFileBytes { get; set; }
    }
}
