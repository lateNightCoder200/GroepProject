using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using API.DataModel.DTO;
using API.DataModel.Models;
using API.Repository.UserRepo;

[TestClass]
public class UserControllerTests
{

    //Unit tests
    private Mock<IUserRepository> _userRepositoryMock;
    private UserController _controller;

    [TestInitialize]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _controller = new UserController(_userRepositoryMock.Object);
    }

    [TestMethod]
    public async Task SetUserName_UsernameAlreadyExists_ReturnsBadRequest()
    {
        var userNameDto = new UserNameDTO { NewUserName = "User", email = "user@gmail.com" };
        _userRepositoryMock.Setup(repo => repo.getUserNameByUserName(userNameDto.NewUserName))
                           .ReturnsAsync("existingUser");

        var result = await _controller.SetUserName(userNameDto);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task SetPatientInfo_UserDoesNotExist_ReturnsNotFound()
    {
        var patientInfoDto = new PatientInfoDTO { email = "none@gmail.com" };
        _userRepositoryMock.Setup(repo => repo.getUserId(patientInfoDto.email))
                           .ReturnsAsync((string)null);

        var result = await _controller.SetPatientInfo(patientInfoDto);

        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task getUserInfo_UserDoesNotExist_ReturnsNotFound()
    {
        _userRepositoryMock.Setup(repo => repo.getUserId("user@gmail.com"))
                           .ReturnsAsync((string)null);

        var result = await _controller.getUserInfo("user@gmail.com");

        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task AddNote_NoteAlreadyExists_ReturnsBadRequest()
    {
        var noteDto = new NoteDTO { Name = "ExistingNote", userEmail = "user@gmail.com" };
        _userRepositoryMock.Setup(repo => repo.getUserId(noteDto.userEmail))
                           .ReturnsAsync("user123");
        _userRepositoryMock.Setup(repo => repo.getNoteByName(noteDto.Name, "user123"))
                           .ReturnsAsync(new Notes());

        var result = await _controller.AddNote(noteDto);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
}
