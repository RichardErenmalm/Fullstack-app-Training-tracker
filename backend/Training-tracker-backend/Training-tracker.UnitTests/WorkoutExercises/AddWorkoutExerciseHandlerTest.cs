using Application.ModelHandling.WorkoutExercise.Commands.CreateWorkoutExercise;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Models;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.ModelHandling.WorkoutExercise.Commands
{
    public class CreateWorkoutExerciseCommandHandlerTests
    {
        private readonly Mock<IWorkoutExerciseRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateWorkoutExerciseCommandHandler _handler;

        public CreateWorkoutExerciseCommandHandlerTests()
        {
            _repoMock = new Mock<IWorkoutExerciseRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateWorkoutExerciseCommandHandler(
                _repoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenWorkoutExerciseIsCreated()
        {
            // Arrange
            var command = new CreateWorkoutExerciseCommand
            {
                WorkoutId = 1,
                ExerciseId = 2
            };

            var domainEntity = new Domain.Models.WorkoutExercise
            {
                WorkoutId = 1,
                ExerciseId = 2
            };

            var dto = new WorkoutExerciseDto
            {
                WorkoutId = 1,
                ExerciseId = 2
            };

            _repoMock.Setup(r => r.WorkoutExistsAsync(command.WorkoutId))
                     .ReturnsAsync(true);

            _repoMock.Setup(r => r.ExerciseExistsAsync(command.ExerciseId))
                     .ReturnsAsync(true);

            _repoMock.Setup(r => r.ExerciseAlreadyAddedToWorkoutAsync(
                    command.WorkoutId, command.ExerciseId))
                     .ReturnsAsync(false);

            _repoMock.Setup(r => r.AddWorkoutExerciseAsync(It.IsAny<Domain.Models.WorkoutExercise>()))
                    .ReturnsAsync(1);


            _mapperMock.Setup(m => m.Map<Domain.Models.WorkoutExercise>(command))
                       .Returns(domainEntity);

            _mapperMock.Setup(m => m.Map<WorkoutExerciseDto>(domainEntity))
                       .Returns(dto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(dto);

            _repoMock.Verify(r => r.AddWorkoutExerciseAsync(domainEntity), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenWorkoutDoesNotExist()
        {
            // Arrange
            var command = new CreateWorkoutExerciseCommand
            {
                WorkoutId = 99,
                ExerciseId = 1
            };

            _repoMock.Setup(r => r.WorkoutExistsAsync(command.WorkoutId))
                     .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("does not exist");

            _repoMock.Verify(
                r => r.AddWorkoutExerciseAsync(It.IsAny<Domain.Models.WorkoutExercise>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExerciseIsAlreadyAddedToWorkout()
        {
            // Arrange
            var command = new CreateWorkoutExerciseCommand
            {
                WorkoutId = 1,
                ExerciseId = 2
            };

            _repoMock.Setup(r => r.WorkoutExistsAsync(command.WorkoutId))
                     .ReturnsAsync(true);

            _repoMock.Setup(r => r.ExerciseExistsAsync(command.ExerciseId))
                     .ReturnsAsync(true);

            _repoMock.Setup(r => r.ExerciseAlreadyAddedToWorkoutAsync(
                    command.WorkoutId, command.ExerciseId))
                     .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("already added");

            _repoMock.Verify(
                r => r.AddWorkoutExerciseAsync(It.IsAny<Domain.Models.WorkoutExercise>()),
                Times.Never);
        }

    }
}
