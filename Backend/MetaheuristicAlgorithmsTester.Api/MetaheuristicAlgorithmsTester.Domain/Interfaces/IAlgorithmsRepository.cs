﻿using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IAlgorithmsRepository
    {
        Task<Algorithm?> AddAlgorithm(Algorithm algorithm);
        Task<Algorithm?> GetAlgorithmById(int id);
        Task<IEnumerable<Algorithm?>> GetAllAlgorithms();
    }
}
