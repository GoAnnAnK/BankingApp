using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoFixture;
using Contracts.Models.Request;
using Domain.Clients.Firebase.Models;
using Persistence.Models.WriteModels;
using Domain.Clients.Firebase;
using Persistence.Repositories;
using Domain.Services;
using AutoFixture.Xunit2;
using Persistence.Models.ReadModels;
using FluentAssertions;
using AutoFixture.AutoMoq;

namespace Domain.UnitTests.Services
{
    public class AuthService_Should
    {
        [Theory]
        [AutoData]
        public async Task RegisterAsync_With_RegisterRequest_ReturnsRegisterResponse(
                        RegisterRequest registerRequest,
                        FirebaseRegisterResponse firebaseRegisterResponse,
                        UserWriteModel userWriteModel,
                        [Frozen] Mock<IFirebaseClient> firebaseClientMock,
                        [Frozen] Mock<IUsersRepository> userRepositoryMock,
                        AuthService sut)
        {
            //Arrange

            firebaseRegisterResponse.Email = registerRequest.Email;

            firebaseClientMock
                .Setup(firebaseClient => firebaseClient
                .RegisterAsync(registerRequest.Email, registerRequest.Password))
                .ReturnsAsync(firebaseRegisterResponse);

            // Act

            var result = await sut.RegisterAsync(registerRequest);

            //Assert
            result.IdToken.Should().BeEquivalentTo(firebaseRegisterResponse.IdToken);
            result.Email.Should().BeEquivalentTo(firebaseRegisterResponse.Email);
            result.Email.Should().BeEquivalentTo(registerRequest.Email);
            result.Username.Should().BeEquivalentTo(registerRequest.Username);
            

            firebaseClientMock
               .Verify(firebaseClient => firebaseClient
               .RegisterAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            userRepositoryMock
                .Verify(userRepository => userRepository
                .CreateUserAsync(It.Is<UserWriteModel>(model =>
                model.FirebaseId.Equals(firebaseRegisterResponse.FirebaseId) &&
                model.Email.Equals(firebaseRegisterResponse.Email))), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task LoginAsync_WithLoginRequest_ReturnsLoginResponse(
           [Frozen] Mock<IFirebaseClient> firebaseClientMock,
           [Frozen] Mock<IUsersRepository> userRepositoryMock,
           LoginRequest loginRequest,
           FirebaseLoginResponse firebaseLoginResponse,
           UserReadModel userReadModel,
           AuthService sut)
        {
            // Arrange
            firebaseLoginResponse.Email = loginRequest.Email;
            userReadModel.FirebaseId = firebaseLoginResponse.FirebaseId;
            userReadModel.Email = firebaseLoginResponse.Email;


            firebaseClientMock
                .Setup(firebaseClient => firebaseClient
                .LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(firebaseLoginResponse);

            userRepositoryMock
                .Setup(userRepository => userRepository
                .GetUserByFirebaseIdAsync(firebaseLoginResponse.FirebaseId))
                .ReturnsAsync(userReadModel);

            // Act
            var result = await sut.LoginAsync(loginRequest);

            // Assert
            result.Email.Should().BeEquivalentTo(loginRequest.Email);
            result.IdToken.Should().BeEquivalentTo(firebaseLoginResponse.IdToken);
            result.Username.Should().BeEquivalentTo(userReadModel.Username);

            firebaseClientMock
                .Verify(firebaseClient => firebaseClient
                .LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            userRepositoryMock
                .Verify(userRepository => userRepository
                .GetUserByFirebaseIdAsync(firebaseLoginResponse.FirebaseId), Times.Once);
        }
    }
}
