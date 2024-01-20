using System.Net;
using System.Net.Http.Json;
using GymManagement.Api.IntegrationTests.Common;
using GymManagement.Contracts.Subscriptions;
using TestCommon.TestConstants;
using FluentAssertions;

namespace GymManagement.Api.IntegrationTests.Features.Subscriptions;

[Collection(GymManagementApiFactoryCollection.CollectionName)]
public class CreateSubscriptionTests
{
    private readonly HttpClient _httpClient;
    
    public CreateSubscriptionTests(GymManagementApiFactory apiFactory)
    {
        _httpClient = apiFactory.HttpClient;
        apiFactory.ResetDatabase();
    }
    
    [Theory]
    [MemberData(nameof(ListSubscriptionTypes))]
    public async Task CreateSubscription_WhenValidSubscription_ShouldReturnCreatedSubscription(
        SubscriptionType subscriptionType)
    {
        // Arrange
        var createSubscriptionRequest = new CreateSubscriptionRequest(subscriptionType, Constants.Admin.Id);
        
        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/subscriptions", createSubscriptionRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var subscriptionResponse = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();
        subscriptionResponse.Should().NotBeNull();
        subscriptionResponse!.SubscriptionType.Should().Be(subscriptionType);
        
        // TODO: Implement a proper Created response.
        // response.Headers.Location.Should().NotBeNull();
        // response.Headers.Location!.PathAndQuery.Should().Be($"/api/subscriptions/{subscriptionResponse.Id}");
    }
    
    public static TheoryData<SubscriptionType> ListSubscriptionTypes()
    {
        var subscriptionTypes = Enum.GetValues<SubscriptionType>();
        
        var theoryData = new TheoryData<SubscriptionType>();

        foreach (var subscriptionType in subscriptionTypes)
        {
            theoryData.Add(subscriptionType);
        }
        
        return theoryData;
    }
}