using AssetManagement.Domain.Enums;

namespace AssetManagement.Domain.Tests.Enums
{
    public class AssetStateTypeTests
    {
        [Theory]
        [InlineData(1, AssetStateType.Available)]
        [InlineData(2, AssetStateType.NotAvailable)]
        [InlineData(3, AssetStateType.Assigned)]
        [InlineData(4, AssetStateType.WaitingForRecycling)]
        [InlineData(5, AssetStateType.Recycled)]
        public void AssetStateType_ShouldHaveExpectedValues(int expectedValue, AssetStateType assetStateType)
        {
            // Assert
            Assert.Equal(expectedValue, (int)assetStateType);
        }

        [Fact]
        public void AssetStateType_ShouldContainFiveMembers()
        {
            // Arrange
            var enumValues = Enum.GetValues(typeof(AssetStateType));

            // Assert
            Assert.Equal(5, enumValues.Length);
        }
    }
}
