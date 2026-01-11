using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.Product.Commands.CreateProduct;
using inventory_aplication.Application.Features.Product.Commands.DeleteProduct;
using inventory_aplication.Application.Features.Product.Commands.UpdateProduct;
using inventory_aplication.Application.Features.Product.Queries.GetAllProducts;
using inventory_aplication.Application.Features.Product.Queries.GetProductBy;
using inventory_aplication.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace inventory_aplication.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductController(_mediatorMock.Object);
        }

        #region Get - GetProducts

        [Fact]
        public async Task Get_ShouldReturnOk_WithPagedProducts()
        {
            var pagedResult = new PagedResult<ProductResponseDto>(
                    totalItems: 0,
                    pageNumber: 1,
                    pageSize: 10,
                    items: new List<ProductResponseDto>()
                );
            var expectedResult = Result<PagedResult<ProductResponseDto>>.Ok(pagedResult);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PagedResult<ProductResponseDto>>.Ok(
                    new PagedResult<ProductResponseDto>(
                        totalItems: 0,
                        pageNumber: 1,
                        pageSize: 10,
                        items: new List<ProductResponseDto>()
                    )
                ));

            // Act
            var result = await _controller.Get(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<PagedResult<ProductResponseDto>>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Null(value.Error);
            Assert.Null(value.Code);

            Assert.Equal(expectedResult.Data.TotalItems, value.Data.TotalItems);
            Assert.Equal(expectedResult.Data.PageNumber, value.Data.PageNumber);
            Assert.Equal(expectedResult.Data.PageSize, value.Data.PageSize);
            Assert.Equal(expectedResult.Data.Items.Count, value.Data.Items.Count);
        }

        [Fact]
        public async Task GetBy_ReturnsOk_WithFilteredProducts()
        {
            // Arrange
            var query = new GetProductByQuery(Name: "Laptop", CategoryId: null);

            var expectedPagedResult = new PagedResult<ProductResponseDto>(
                totalItems: 1,
                pageNumber: 1,
                pageSize: 10,
                items: new List<ProductResponseDto>
                {
                new ProductResponseDto
                {
                    Id = 1,
                    Name = "Laptop",
                    CategoryId = 2,
                    CategoryName = "Electronics",
                    Description = "Laptop XYZ",
                    Stock = 5,
                    Price = 1200
                }
                }
            );

            var expectedResult = Result<PagedResult<ProductResponseDto>>.Ok(expectedPagedResult);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetProductByQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetBy(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<PagedResult<ProductResponseDto>>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal(expectedResult.Data.TotalItems, value.Data.TotalItems);
            Assert.Equal(expectedResult.Data.PageNumber, value.Data.PageNumber);
            Assert.Equal(expectedResult.Data.PageSize, value.Data.PageSize);
            Assert.Equal(expectedResult.Data.Items.Count, value.Data.Items.Count);
            Assert.Equal(expectedResult.Data.Items[0].Name, value.Data.Items[0].Name);
        }

        #endregion

        #region POST - CreateProduct

        [Fact]
        public async Task Post_ShouldSendCreateProductCommand()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "Producto Test",
                Description = "Desc",
                CategoryId = 1,
                Stock = 10,
                Price = 50
            };

            var expected = Result<string>.Ok("Producto creado correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.Post(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }

        

        [Fact]
        public async Task Post_ReturnsOk_WhenProductCreated()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "Laptop",
                Description = "Laptop XYZ",
                CategoryId = 1,
                Stock = 5,
                Price = 1200
            };

            var expectedResult = Result<string>.Ok("Producto creado correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Post(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);
            Assert.True(value.Success);
            Assert.Equal(expectedResult.Data, value.Data);
        }

        #endregion

        #region PUT - UpdateProduct

        [Fact]
        public async Task Put_ReturnsOk_WhenProductUpdated()
        {
            // Arrange
            int productId = 1;
            var dto = new ProductUpdateDto
            {
                Name = "Laptop Updated",
                Description = "Updated description",
                CategoryId = 1,
                Price = 1500
            };

            var expectedResult = Result<string>.Ok("Producto actualizado correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Put(productId, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);
            Assert.True(value.Success);
            Assert.Equal(expectedResult.Data, value.Data);
        }

        #endregion

        #region DELETE - DeleteProduct

        [Fact]
        public async Task Delete_ReturnsOk_WhenProductDeleted()
        {
            // Arrange
            int productId = 1;
            var expectedResult = Result<string>.Ok("Producto eliminado correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);
            Assert.True(value.Success);
            Assert.Equal(expectedResult.Data, value.Data);
        }

        #endregion
    }

}
