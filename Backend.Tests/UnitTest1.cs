using System.Net;
using System.Net.Http.Json;
using Backend.Dtos;
using Microsoft.AspNetCore.Http;

namespace Backend.Tests;

public class UnitTest1
{
    [Fact]
    public async void Test1()
    {
        HttpClient httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5057/api/register")
        {
            Content = JsonContent.Create(new RegistrationDto()
            {
                FirstName = "John",
                Lastname = "Doe",
                Email = "johndoe@email.com",
                Username = "johndoe",
                Password = "Letme!n1"
            })
        };

        var response = await httpClient.SendAsync(request);
        Assert.True(response.IsSuccessStatusCode);

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        Assert.NotNull(loginResponse);
    }

    [Fact]
    public async Task Test2()
    {
        var payload = new RegistrationDto()
        {
            FirstName = "John",
            Lastname = "Doe",
            Email = "johndoe1@email.com",
            Username = "johndoe",
            Password = "Letme!n1"
        };

        HttpClient httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5057/api/register")
        {
            Content = JsonContent.Create(payload)
        };

        var response = await httpClient.SendAsync(request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var requestMessage = await response.Content.ReadAsStringAsync();
        Assert.Equal($"Profile with username {payload.Username} already exists.", requestMessage);
    }

    [Fact]
    public async Task Test3()
    {
        var payload = new LoginCredentialsDto()
        {
            Username = "johndoe",
            Password = "Letme!n1"
        };

        HttpClient httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5057/api/login")
        {
            Content = JsonContent.Create(payload)
        };

        var response = await httpClient.SendAsync(request);
        Assert.True(response.IsSuccessStatusCode);

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        Assert.NotNull(loginResponse);
    }

    [Fact]
    public async Task Test4()
    {
        var payload = new LoginCredentialsDto()
        {
            Username = "johndoe1",
            Password = "Letme!n1"
        };

        HttpClient httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5057/api/login")
        {
            Content = JsonContent.Create(payload)
        };

        var response = await httpClient.SendAsync(request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var requestMessage = await response.Content.ReadAsStringAsync();
        Assert.Equal("Invalid credentials", requestMessage);
    }

    [Fact]
    public async Task Test5()
    {
        var payload = new LoginCredentialsDto()
        {
            Username = "johndoe",
            Password = "Letme!n2"
        };

        HttpClient httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5057/api/login")
        {
            Content = JsonContent.Create(payload)
        };

        var response = await httpClient.SendAsync(request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var requestMessage = await response.Content.ReadAsStringAsync();
        Assert.Equal("Invalid credentials", requestMessage);
    }

    [Fact]
    public async Task Test6()
    {
        var loginPayload = new LoginCredentialsDto()
        {
            Username = "johndoe",
            Password = "Letme!n1"
        };

        HttpClient httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5057/api/login")
        {
            Content = JsonContent.Create(loginPayload)
        };

        var response = await httpClient.SendAsync(request);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        var createTaskPayload = new CreateTaskDto()
        {
            Title = "Task 1",
            Description = "Task 1 Description"
        };

        request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5057/api/tasks")
        {
            Content = JsonContent.Create(createTaskPayload),
        };
        request.Headers.Authorization = new("bearer", loginResponse.JwtToken);

        response = await httpClient.SendAsync(request);
        Assert.True(response.IsSuccessStatusCode);

        var readTaskDto = await response.Content.ReadFromJsonAsync<ReadTaskDto>();
        Assert.NotNull(readTaskDto);

        Assert.Equal(readTaskDto.ProfileId, loginResponse.ProfileId);
        Assert.Equal(readTaskDto.Title, createTaskPayload.Title);
        Assert.Equal(readTaskDto.Description, createTaskPayload.Description);
        Assert.Equal(Models.TaskStatus.INCOMPLETE, readTaskDto.TaskStatus);
    }
}
