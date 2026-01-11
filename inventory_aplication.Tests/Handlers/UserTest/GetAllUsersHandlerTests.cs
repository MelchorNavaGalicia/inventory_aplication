using inventory_aplication.Application.Common.DTOs;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.User.Queries.GetAllUsers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.UserTest
{
    public class GetAllUsersHandlerTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly GetAllUsersHandler _handler;

        public GetAllUsersHandlerTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _handler = new GetAllUsersHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedUsers()
        {
            var pagedResult = new PagedResult<UserDto>(
                totalItems: 2,
                pageNumber: 1,
                pageSize: 10,
                items: new List<UserDto>
                {
                new UserDto { Id = 1, Name = "Alice" },
                new UserDto { Id = 2, Name = "Bob" }
                }
            );

            _repoMock.Setup(r => r.GetAllDtoAsync(1, 10, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(pagedResult);

            var query = new GetAllUsersQuery(1, 10);
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(pagedResult, result.Data);
        }
    }

}
