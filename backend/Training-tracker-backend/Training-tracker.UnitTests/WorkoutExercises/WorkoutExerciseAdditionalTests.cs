using Application.ModelHandling.WorkoutExercise.Commands.UpdateWorkoutExercise;
using Application.ModelHandling.WorkoutExercise.Commands.DeleteWorkoutExercise;
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
    public class WorkoutExerciseAdditionalTests
    {
        private readonly Mock<IWorkoutExerciseRepository> _workoutExerciseRepoMock;
        private readonly Mock<IWorkoutRepository> _workoutRepoMock;
        private readonly Mock<IExerciseRepository> _exerciseRepoMock;
        private readonly Mock<IMapper> _mapperMock;


        public WorkoutExerciseAdditionalTests()
        {
            _workoutExerciseRepoMock = new Mock<IWorkoutExerciseRepository>();
            _workoutRepoMock = new Mock<IWorkoutRepository>();
            _exerciseRepoMock = new Mock<IExerciseRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task UpdateWorkoutExercise_ShouldReturnSuccess_WhenValidUpdate()
        {
            // Arrange
            var command = new UpdateWorkoutExerciseCommand
            {
                Id = 1,
                Sets = 5,
                ExerciseId = 2,
                WorkoutId = 3
            };

            _workoutExerciseRepoMock
                .Setup(r => r.GetWorkoutExerciseByIdAsync(command.Id))
                .ReturnsAsync(new Domain.Models.WorkoutExercise { Id = command.Id });

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



            var handler = new UpdateWorkoutExerciseCommandHandler(
                 _workoutExerciseRepoMock.Object,
                 _mapperMock.Object
             );


            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _workoutExerciseRepoMock.Verify(r => r.UpdateWorkoutExerciseAsync(It.IsAny<Domain.Models.WorkoutExercise>()), Times.Once);
        }

        [Fact]
        public async Task UpdateWorkoutExercise_ShouldReturnFailure_WhenWorkoutExerciseDoesNotExist()
        {
            var command = new UpdateWorkoutExerciseCommand { Id = 99 };

            _workoutExerciseRepoMock
                .Setup(r => r.GetWorkoutExerciseByIdAsync(command.Id))
                .ReturnsAsync((Domain.Models.WorkoutExercise)null);

            var handler = new UpdateWorkoutExerciseCommandHandler(
                _workoutExerciseRepoMock.Object,
                _mapperMock.Object
            );

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("does not exist");

            _workoutExerciseRepoMock.Verify(
                r => r.UpdateWorkoutExerciseAsync(It.IsAny<Domain.Models.WorkoutExercise>()),
                Times.Never);
        }
 
        [Fact]
        public async Task DeleteWorkoutExercise_ShouldReturnSuccess_WhenExists()
        {
            // Arrange
            var command = new DeleteWorkoutExerciseCommand { Id = 1 };

            var workoutExercise = new Domain.Models.WorkoutExercise
            {
                Id = 1
            };

            _workoutExerciseRepoMock
                .Setup(r => r.GetWorkoutExerciseByIdAsync(command.Id))
                .ReturnsAsync(workoutExercise);

            _workoutExerciseRepoMock
                .Setup(r => r.DeleteWorkoutExerciseAsync(workoutExercise))
                .Returns(Task.CompletedTask);

            var handler = new DeleteWorkoutExerciseCommandHandler(
                _workoutExerciseRepoMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();

            _workoutExerciseRepoMock.Verify(
                r => r.DeleteWorkoutExerciseAsync(workoutExercise),
                Times.Once
            );
        }


        [Fact]
        public async Task DeleteWorkoutExercise_ShouldReturnFailure_WhenNotExists()
        {
            // Arrange
            var command = new DeleteWorkoutExerciseCommand { Id = 99 };

            _workoutExerciseRepoMock
                .Setup(r => r.GetWorkoutExerciseByIdAsync(command.Id))
                .ReturnsAsync((Domain.Models.WorkoutExercise)null);

            var handler = new DeleteWorkoutExerciseCommandHandler(
                _workoutExerciseRepoMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();

            _workoutExerciseRepoMock.Verify(
                r => r.DeleteWorkoutExerciseAsync(It.IsAny<Domain.Models.WorkoutExercise>()),
                Times.Never
            );
        }

    }
}
