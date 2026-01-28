using Application.ModelHandling.WorkoutExercise.Commands.UpdateWorkoutExercise;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.ModelHandling.WorkoutExercise.Commands
{
    public class UpdateWorkoutExerciseCommandHandlerTests
    {

        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateWorkoutExerciseCommandHandler _handler;
        private readonly Mock<IWorkoutExerciseRepository> _workoutExerciseRepoMock;
        private readonly Mock<IWorkoutRepository> _workoutRepoMock;
        private readonly Mock<IExerciseRepository> _exerciseRepoMock;



        public UpdateWorkoutExerciseCommandHandlerTests()
        {
            _workoutExerciseRepoMock = new Mock<IWorkoutExerciseRepository>();
            _workoutRepoMock = new Mock<IWorkoutRepository>();
            _exerciseRepoMock = new Mock<IExerciseRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateWorkoutExerciseCommandHandler(
                _workoutExerciseRepoMock.Object,
                _mapperMock.Object
            );
        }


        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenWorkoutExerciseIsUpdated()
        {
            var command = new UpdateWorkoutExerciseCommand
            {
                Id = 1,
                Sets = 3,
                WorkoutId = 1,
                ExerciseId = 2
            };

            var existing = new Domain.Models.WorkoutExercise { Id = 1 };

            var dto = new WorkoutExerciseDto
            {
                Id = 1,
                Sets = 3,
                WorkoutId = 1,
                ExerciseId = 2
            };

            _workoutExerciseRepoMock
                .Setup(r => r.GetWorkoutExerciseByIdAsync(command.Id))
                .ReturnsAsync(existing);

            _workoutExerciseRepoMock
                .Setup(r => r.WorkoutExistsAsync(command.WorkoutId))
                .ReturnsAsync(true);

            _workoutExerciseRepoMock
                .Setup(r => r.ExerciseExistsAsync(command.ExerciseId))
                .ReturnsAsync(true);

            _workoutExerciseRepoMock
                .Setup(r => r.ExerciseAlreadyAddedToWorkoutUpdateAsync(
                    command.WorkoutId,
                    command.ExerciseId,
                    command.Id))
                .ReturnsAsync(false);

            _workoutExerciseRepoMock
                .Setup(r => r.UpdateWorkoutExerciseAsync(It.IsAny<Domain.Models.WorkoutExercise>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<WorkoutExerciseDto>(It.IsAny<Domain.Models.WorkoutExercise>()))
                .Returns(dto);

            var handler = new UpdateWorkoutExerciseCommandHandler(
                _workoutExerciseRepoMock.Object,
                _mapperMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(dto);

            _workoutExerciseRepoMock.Verify(
                r => r.UpdateWorkoutExerciseAsync(It.IsAny<Domain.Models.WorkoutExercise>()),
                Times.Once);
        }


        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenWorkoutExerciseDoesNotExist()
        {
            // Arrange
            var command = new UpdateWorkoutExerciseCommand
            {
                Id = 99,
                WorkoutId = 1,
                ExerciseId = 2
            };

            _workoutExerciseRepoMock.Setup(r => r.GetWorkoutExerciseByIdAsync(command.Id))
            .ReturnsAsync((Domain.Models.WorkoutExercise)null);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("does not exist");

            _workoutExerciseRepoMock.Verify(
                r => r.UpdateWorkoutExerciseAsync(It.IsAny<Domain.Models.WorkoutExercise>()),
                Times.Never
            );
        }
    }
}
