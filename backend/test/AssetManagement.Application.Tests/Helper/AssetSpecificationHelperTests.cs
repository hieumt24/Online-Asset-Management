using AssetManagement.Application.Filter;
using AssetManagement.Application.Helper;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AssetManagement.Domain.Specifications;
using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace AssetManagement.Application.Tests.Helper
{
    public class AssetSpecificationHelperTests
    {
        [Fact]
        public void AssetSpecificationWithCategory_ShouldApplyOrderingAndPaging()
        {
            // Arrange
            var paginationFilter = new PaginationFilter { PageIndex = 1, PageSize = 10 };
            string orderBy = "AssetName";
            bool isDescending = true;

            // Act
            var spec = AssetSpecificationHelper.AssetSpecificationWithCategory(paginationFilter, orderBy, isDescending);

            // Assert
            spec.Includes.Should().ContainSingle(include => include.ToString().Contains("Category"));
            spec.OrderByDescending.Should().NotBeNull();
            spec.OrderByDescending.Compile()(new Asset()).Should().BeAssignableTo<IComparable>();
            spec.Skip.Should().Be(paginationFilter.PageSize * (paginationFilter.PageIndex - 1));
            spec.Take.Should().Be(paginationFilter.PageSize);
        }

        [Fact]
        public void GetOrderByExpression_ShouldReturnCorrectExpression()
        {
            // Arrange & Act
            var orderByExpressions = new[]
            {
                AssetSpecificationHelper.GetOrderByExpression("assetcode"),
                AssetSpecificationHelper.GetOrderByExpression("assetname"),
                AssetSpecificationHelper.GetOrderByExpression("installeddate"),
                AssetSpecificationHelper.GetOrderByExpression("state"),
                AssetSpecificationHelper.GetOrderByExpression("assetlocation"),
                AssetSpecificationHelper.GetOrderByExpression("categoryname"),
                AssetSpecificationHelper.GetOrderByExpression("createdon"),
                AssetSpecificationHelper.GetOrderByExpression("lastmodifiedon")
            };

            // Assert
            orderByExpressions.Should().AllBeAssignableTo<Expression<Func<Asset, object>>>();
        }

        [Fact]
        public void GetAssetByAssetCode_ShouldReturnCorrectSpecification()
        {
            // Arrange
            string assetCode = "A123";

            // Act
            var spec = AssetSpecificationHelper.GetAssetByAssetCode(assetCode);

            // Assert
            spec.Criteria.Compile()(new Asset { AssetCode = assetCode, IsDeleted = false }).Should().BeTrue();
            spec.Criteria.Compile()(new Asset { AssetCode = "B456", IsDeleted = false }).Should().BeFalse();
            spec.Includes.Should().ContainSingle(include => include.ToString().Contains("Category"));
        }

        [Fact]
        public void GetAssetById_ShouldReturnCorrectSpecification()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            var spec = AssetSpecificationHelper.GetAssetById(id);

            // Assert
            spec.Criteria.Compile()(new Asset { Id = id, IsDeleted = false }).Should().BeTrue();
            spec.Criteria.Compile()(new Asset { Id = Guid.NewGuid(), IsDeleted = false }).Should().BeFalse();
            spec.Includes.Should().ContainSingle(include => include.ToString().Contains("Category"));
        }

        [Fact]
        public void GetAllAssets_ShouldReturnCorrectSpecification()
        {
            // Arrange
            var location = EnumLocation.HaNoi;

            // Act
            var spec = AssetSpecificationHelper.GetAllAssets(location);

            // Assert
            spec.Criteria.Compile()(new Asset { AssetLocation = location, IsDeleted = false }).Should().BeTrue();
            spec.Criteria.Compile()(new Asset { AssetLocation = EnumLocation.HoChiMinh, IsDeleted = false }).Should().BeFalse();
        }
    }
}
