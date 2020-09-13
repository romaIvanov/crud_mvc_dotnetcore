using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace UniversityMVC.Tests.IntegrationTests
{
    public class GroupsControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public GroupsControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Index_ReturnsIndexPage()
        {

            var response = await _client.GetAsync("/Groups/Index");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("SR-01", responseString);
            Assert.Contains("SR-02", responseString);
        }

        [Fact]
        public async Task Get_GroupsView_ReturnsGroupsViewPage()
        {

            var response = await _client.GetAsync("/Groups/GroupsView/1");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("SR-01", responseString);
        }

        [Fact]
        public async Task Get_Details_ReturnsDetailsPage()
        {
            var response = await _client.GetAsync("Groups/Details/2");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("SR-02", responseString);
        }

        [Fact]
        public async Task Get_Create_ReturnsCreateForm()
        {
            var response = await _client.GetAsync("Groups/Create");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Please provide a new group data", responseString);
        }

        [Fact]
        public async Task Post_Create_SentWrongForm_ReturnsViewWithWarning()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Groups/Create");

            var formModel = new Dictionary<string, string> { };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name cannot be empty", responseString);
            
        }

        [Fact]
        public async Task Post_Create_SentCorrectlyForm_ReturnsToIndexWithCreatedGroup()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Groups/Create");
            var formModel = new Dictionary<string, string>
            {
                { "CourseId", "1" },
                { "Name", "Created Group" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);
            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Created Group", responseString);

        }

        [Fact]
        public async Task Get_EditForm_ReturnsEditForm()
        {
            var response = await _client.GetAsync("Groups/Edit/5");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Please provide a new group data", responseString);
        }

        [Fact]
        public async Task Post_Edit_SentWrongForm_ReturnsViewWIthWarning()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Groups/Edit/5");
            var formModel = new Dictionary<string, string>
            {
                { "GroupId", "5" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);
            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name cannot be empty", responseString);

        }

        [Fact]
        public async Task Post_Edit_SentCorrectlyForm_ReturnsToIndexWithEditedGroup()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Groups/Edit/5");
            var formModel = new Dictionary<string, string>
            {
                { "GroupId", "5" },
                { "Name", "Edited Name" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Edited Name", responseString);
        }

        [Fact]
        public async Task Get_DeleteForm_ReturnsDeleteForm()
        {
            var response = await _client.GetAsync("Groups/Delete/5");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Are you sure you want to delete this", responseString);
        }

        [Fact]
        public async Task Get_Delete_SentWrong_ReturnsViewWIthWarning()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Groups/Delete/5");
            var formModel = new Dictionary<string, string>
            {
                { "GroupId", "5" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Cannot delete a group", responseString);
        }

        [Fact]
        public async Task Post_Delete_SentCorrectlyForm_ReturnsToIndexWithDeletedGroup()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Groups/Delete/6");
            var formModel = new Dictionary<string, string>
            {
                { "GroupId", "6" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);
            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("has been deleted", responseString);
        }
    }
}
