using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Features.Category.Commands.CreateCategory;
using inventory_aplication.Application.Features.Category.Commands.DeleteCategory;
using inventory_aplication.Application.Features.Category.Commands.UpdateCategory;
using inventory_aplication.Application.Features.Category.Queries.GetAllCategories;
using inventory_aplication.Application.Features.Category.Queries.GetCategoriesBy;
using inventory_aplication.Application.Features.Category.Queries.GetCategoryById;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new CategoryController(_mediatorMock.Object);
        }

        #region GET All

        [Fact]
        public async Task Get_ReturnsOk_WithPagedCategories()
        {
            // Arrange
            var expectedResult = Result<PagedResult<CategoryResponseDto>>.Ok(
                new PagedResult<CategoryResponseDto>(
                    totalItems: 1,
                    pageNumber: 1,
                    pageSize: 10,
                    items: new List<CategoryResponseDto>
                    {
                    new CategoryResponseDto
                    {
                        Id = 1,
                        Name = "Electronics",
                        Description = "Electrónica"
                    }
                    }
                )
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Get(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<PagedResult<CategoryResponseDto>>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal(1, value.Data.TotalItems);
            Assert.Equal("Electronics", value.Data.Items[0].Name);
        }

        #endregion

        #region GET by ID

        [Fact]
        public async Task GetById_ReturnsOk_WithCategory()
        {
            // Arrange
            int categoryId = 1;

            var expectedResult = Result<CategoryResponseDto>.Ok(
                new CategoryResponseDto
                {
                    Id = categoryId,
                    Name = "Electronics",
                    Description = "Electrónica"
                }
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Get(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<CategoryResponseDto>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal(categoryId, value.Data.Id);
            Assert.Equal("Electronics", value.Data.Name);
        }

        #endregion

        #region GET filter

        [Fact]
        public async Task GetByFilter_ReturnsOk_WithFilteredCategories()
        {
            // Arrange
            var query = new GetCategoriesByQuery(Name: "Electronics");

            var expectedResult = Result<PagedResult<CategoryResponseDto>>.Ok(
                new PagedResult<CategoryResponseDto>(
                    totalItems: 1,
                    pageNumber: 1,
                    pageSize: 10,
                    items: new List<CategoryResponseDto>
                    {
                    new CategoryResponseDto
                    {
                        Id = 1,
                        Name = "Electronics",
                        Description = "Electrónica"
                    }
                    }
                )
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCategoriesByQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetBy(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<PagedResult<CategoryResponseDto>>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal(1, value.Data.TotalItems);
            Assert.Equal("Electronics", value.Data.Items[0].Name);
        }

        #endregion

        #region POST - CreateCategory

        [Fact]
        public async Task PostAsync_ReturnsOk_WhenCategoryCreated()
        {
            // Arrange
            var dto = new CategoryCreateDto
            {
                Name = "Electronics",
                Description = "Electrónica"
            };

            var expectedResult = Result<string>.Ok("Categoría creada correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.PostAsync(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal("Categoría creada correctamente", value.Data);
        }

        #endregion

        #region PUT - UpdateCategory

        [Fact]
        public async Task PutAsync_ReturnsOk_WhenCategoryUpdated()
        {
            // Arrange
            int categoryId = 1;
            var dto = new CategoryUpdateDto
            {
                Name = "Electronics Updated",
                Description = "Electrónica actualizada"
            };

            var expectedResult = Result<string>.Ok("Categoría actualizada correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.PutAsync(categoryId, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal("Categoría actualizada correctamente", value.Data);
        }

        #endregion

        #region DELETE - DeleteCategory

        [Fact]
        public async Task Delete_ReturnsOk_WhenCategoryDeleted()
        {
            // Arrange
            int categoryId = 1;

            var expectedResult = Result<string>.Ok("Categoría eliminada correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Delete(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal("Categoría eliminada correctamente", value.Data);
        }

        #endregion
    }
}
