using AssetManagement.Domain.Common.Models;

namespace AssetManagement.Domain.Tests.Common.Models
{
    public class BaseEntityTests
    {
        [Fact]
        public void BaseEntity_ShouldInitializeWithNewGuid()
        {
            // Arrange & Act
            var entity = new BaseEntity();

            // Assert
            Assert.NotEqual(Guid.Empty, entity.Id);
        }

        [Fact]
        public void BaseEntity_ShouldAllowSettingAndGettingProperties()
        {
            // Arrange
            var entity = new BaseEntity();
            var now = DateTimeOffset.UtcNow;
            var userId = Guid.NewGuid().ToString();

            // Act
            entity.CreatedBy = userId;
            entity.CreatedOn = now;
            entity.LastModifiedBy = userId;
            entity.LastModifiedOn = now;
            entity.IsDeleted = true;

            // Assert
            Assert.Equal(userId, entity.CreatedBy);
            Assert.Equal(now, entity.CreatedOn);
            Assert.Equal(userId, entity.LastModifiedBy);
            Assert.Equal(now, entity.LastModifiedOn);
            Assert.True(entity.IsDeleted);
        }

        [Fact]
        public void BaseEntity_ShouldSetDefaultValues()
        {
            // Arrange & Act
            var entity = new BaseEntity();

            // Assert
            Assert.Null(entity.CreatedBy);
            Assert.Equal(default(DateTimeOffset), entity.CreatedOn);
            Assert.Null(entity.LastModifiedBy);
            Assert.Null(entity.LastModifiedOn);
            Assert.False(entity.IsDeleted);
        }
    }
}
