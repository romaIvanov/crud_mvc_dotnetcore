using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UniversityMVC.Tests;
using Xunit;

namespace UniversityMVC.Tests.IntegrationTests
{
    public class CoursesControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CoursesControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Index_ReturnsIndexPage()
        {
             
            var response = await _client.GetAsync("/Courses/Index");

            response.EnsureSuccessStatusCode();
            
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("JAVA", responseString);
            Assert.Contains("C#", responseString);
        }

        [Fact]
        public async Task Get_Details_ReturnsDetailsPage()
        {
            var response = await _client.GetAsync("/Courses/Details/2");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("JAVA", responseString);
        }

        [Fact]
        public async Task Get_CreateForm_ReturnsCreateForm()
        {
            var response = await _client.GetAsync("/Courses/Create");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Please provide a new course data", responseString);
        }

        [Fact]
        public async Task Post_Create_SentWrongForm_ReturnsViewWithWarning()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Courses/Create");

            var formModel = new Dictionary<string, string>
            {
                {"Description", "course description" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Name cannot be empty", responseString);
        }

        [Fact]
        public async Task Post_Create_SentCorrectlyForm_ReturnsToIndexViewWithCreatedCourse()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Courses/Create");

            var formModel = new Dictionary<string, string>
                {
                    { "Name", "New Course" },
                    { "Description", "Course Description" }
                };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("New Course", responseString);
            Assert.Contains("Course Description", responseString);
        }

        [Fact]
        public async Task Get_EditForm_ReturnsEditForm()
        {
            var response = await _client.GetAsync("/Courses/Edit/3");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Please provide a new course data", responseString);
        }

        [Fact]
        public async Task Post_Edit_SentWrongForm_ReturnsViewWithWarning()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Courses/Edit/3");

            var formModel = new Dictionary<string, string>
            {
                { "CourseId", "3" },
                { "Name", " " },
                {"Description", "course description" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Name cannot be empty", responseString);
        }

        [Fact]
        public async Task Post_Edit_SentCorrectlyForm_ReturnsToIndexViewWithEditedCourse()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Courses/Edit/3");

            var formModel = new Dictionary<string, string>
                {
                    { "CourseId", "3" },
                    { "Name", "Edited Course" },
                    { "Description", "Edited Course Description" }
                };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Edited Course", responseString);
            Assert.Contains("Edited Course Description", responseString);
        }

        [Fact]
        public async Task Get_DeleteForm_ReturnsDeleteForm()
        {
            var response = await _client.GetAsync("/Courses/Delete/5");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Are you sure you want to delete this?", responseString);
        }

        [Fact]
        public async Task Post_Delete_SentWrong_ReturnsToIndexViewWithWarning()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Courses/Delete/2");

            var formModel = new Dictionary<string, string>
            {
                { "CourseId", "2" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Cannot delete a course", responseString);
        }

        [Fact]
        public async Task Post_Delete_SentCorrectlyForm_ReturnsToIndexViewWithDeletedCourse()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Courses/Delete/6");

            var formModel = new Dictionary<string, string>
                {
                    { "CourseId", "6" }
                };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("has been deleted", responseString);
        }
    }
}
