using Application.Dtos;
using Application.ModelHandling.WorkoutHistory.Commands.CreateWorkoutHistory;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings
{
    public class WorkoutHistoryProfile : Profile
    {
        public WorkoutHistoryProfile()
        {
            CreateMap<WorkoutHistory, WorkoutHistoryDto>();
            CreateMap<CreateWorkoutHistoryCommand, WorkoutHistory>();
        }
    }
}
