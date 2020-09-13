using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace UniversityMVC.Tests.IntegrationTests
{
    public class StudentsControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        public StudentsControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Index_ReturnsIndexPage()
        {
            var response = await _client.GetAsync("/Students/Index");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Eddard", responseString);
            Assert.Contains("Daenerys", responseString);
        }

        [Fact]
        public async Task Get_StudentsView_ReturnsStudentsViewPage()
        {
            var response = await _client.GetAsync("/Students/StudentsView/5");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Tony", responseString);
            Assert.Contains("Stephen", responseString);

        }

        [Fact]
        public async Task Get_Details_ReturnsDetailsPage()
        {
            var response = await _client.GetAsync("Students/Details/6");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Details", responseString);
            Assert.Contains("Jon", responseString);
        }

        [Fact]
        public async Task Get_Create_ReturnsCreateForm()
        {
            var response = await _client.GetAsync("Students/Create");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Please provide a new student data", responseString);
        }

        [Fact]
        public async Task Post_Create_SentWrongForm_ReturnsViewWithWarning()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Students/Create");
            var formModel = new Dictionary<string, string>
            {
                { "GroupId", "2" }
            };
            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Firstname cannot be empty", responseString);
            Assert.Contains("Lastname cannot be empty", responseString);
        }

        [Fact]
        public async Task Post_Create_SentCorrectlyForm_ReturnsToIndexWithCreatedStudent()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Students/Create");
            var formModel = new Dictionary<string, string>
            {
                { "GroupId", "2" },
                { "FirstName", "Firstname Student" },
                { "LastName", "Lastname Student" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Firstname Student", responseString);
            Assert.Contains("Lastname Student", responseString);
        }

        [Fact]
        public async Task Get_Edit_ReturnsEditForm()
        {
            var response = await _client.GetAsync("Students/Edit/5");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Please provide a new student data", responseString);
        }

        [Fact]
        public async Task Post_Edit_SentWrongForm_ReturnsViewWithWarning()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Students/Edit/5");
            var formModel = new Dictionary<string, string>
            {
                { "StudentId", "5" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Firstname cannot be empty", responseString);
            Assert.Contains("Lastname cannot be empty", responseString);
        }

        [Fact]
        public async Task Post_Edit_SentCorrectrlyForm_ReturnsToIndexWithEditedStudent()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Students/Edit/5");
            var formModel = new Dictionary<string, string>
            {
                { "StudentId", "5" },
                { "FirstName", "Edited FirstName" },
                { "LastName", "Edited LastName" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Edited FirstName", responseString);
            Assert.Contains("Edited LastName", responseString);
        }

        [Fact]
        public async Task Get_Delete_ReturnsDeleteForm()
        {
            var response = await _client.GetAsync("Students/Delete/5");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Are you sure you want to delete this", responseString);
        }

        [Fact]
        public async Task Delete_SentCorrectlyForm_ReturnsToIndexWithDeletedStudent()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Students/Delete/5");
            var formModel = new Dictionary<string, string>
            {
                { "StudentId", "5" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("has been deleted", responseString);
        }
    }
}
