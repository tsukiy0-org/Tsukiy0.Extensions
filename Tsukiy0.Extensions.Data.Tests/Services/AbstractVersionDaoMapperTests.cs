using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;

using Tsukiy0.Extensions.Data.Models;
using Tsukiy0.Extensions.Data.Services;

using Xunit;

namespace Tsukiy0.Extensions.Data.Tests.Services
{
    public class VersionDaoMapperTests
    {

        [Fact]
        public async void To__UsesLatestMapper()
        {
            // Arrange
            var sut = new VersionDaoMapper();
            var dto = new Dto
            {
                Latest = true
            };

            // Act
            var actual = await sut.To(dto) as DaoV2;

            // Assert
            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(new DaoV2
            {
                __TYPE = "TEST",
                __VERSION = 2,
                __UPDATED = DateTimeOffset.MaxValue,
                Latest = true
            });
        }

        [Fact]
        public async void From__WhenReadingLatestThenNoVersionMismatch()
        {
            // Arrange
            var sut = new VersionDaoMapper();
            var dao = new DaoV2
            {
                __TYPE = "TEST",
                __VERSION = 2,
                __UPDATED = DateTimeOffset.MaxValue,
                Latest = true
            };

            // Act
            var actual = await sut.From(dao);

            // Assert
            actual.Should().BeEquivalentTo(new Dto
            {
                Latest = true
            });
            sut.HadVersionMismatch.Should().BeFalse();
        }

        [Fact]
        public async void From__WhenReadingOlderVersionThenVersionMismatchIsCalled()
        {
            // Arrange
            var sut = new VersionDaoMapper();
            var dao = new DaoV1
            {
                __TYPE = "TEST",
                __VERSION = 1,
                __UPDATED = DateTimeOffset.MaxValue,
                Latest = "true"
            };

            // Act
            var actual = await sut.From(dao);

            // Assert
            actual.Should().BeEquivalentTo(new Dto
            {
                Latest = true
            });
            sut.HadVersionMismatch.Should().BeTrue();
        }
    }

    public class Dto
    {
        public bool Latest { get; init; }
    }

    public class DaoV1 : IDao
    {
        public string __TYPE { get; init; }
        public int __VERSION { get; init; }
        public DateTimeOffset __UPDATED { get; init; }
        public string Latest { get; init; }
    }

    public class DaoV2 : IDao
    {
        public string __TYPE { get; init; }
        public int __VERSION { get; init; }
        public DateTimeOffset __UPDATED { get; init; }
        public bool Latest { get; init; }
    }

    public class V1DaoMapper : IDaoMapper<Dto, IDao>
    {
        public Task<Dto> From(IDao source)
        {
            if (source is not DaoV1 v1)
            {
                throw new InvalidCastException();
            }

            return Task.FromResult(new Dto
            {
                Latest = v1.Latest == "true"
            });
        }

        public Task<IDao> To(Dto destination)
        {
            return Task.FromResult<IDao>(new DaoV1
            {
                __VERSION = 1,
                __UPDATED = DateTimeOffset.MaxValue,
                __TYPE = "TEST",
                Latest = destination.Latest.ToString()
            });
        }
    }

    public class V2DaoMapper : IDaoMapper<Dto, IDao>
    {
        public Task<Dto> From(IDao source)

        {
            if (source is not DaoV2 v1)
            {
                throw new InvalidCastException();
            }

            return Task.FromResult(new Dto
            {
                Latest = v1.Latest
            });
        }

        public Task<IDao> To(Dto destination)
        {
            return Task.FromResult<IDao>(new DaoV2
            {
                __VERSION = 2,
                __UPDATED = DateTimeOffset.MaxValue,
                __TYPE = "TEST",
                Latest = destination.Latest
            });
        }
    }

    public class VersionDaoMapper : AbstractVersionDaoMapper<Dto, IDao>
    {
        public bool HadVersionMismatch { get; set; }

        public VersionDaoMapper() : base(
            new List<VersionMapper<Dto, IDao>>
            {
                new VersionMapper<Dto, IDao> {
                    Version = 1,
                    Mapper = new V1DaoMapper()
                },
                new VersionMapper<Dto, IDao>{
                    Version = 2,
                    Mapper = new V2DaoMapper()
                }
            }
        )
        {
            HadVersionMismatch = false;
        }

        protected override Task OnVersionMismatch(Dto dto, int currentVersion)
        {
            HadVersionMismatch = true;
            return Task.CompletedTask;
        }

        protected override DaoVersion ToDaoVersion(IDao u)
        {
            return new DaoVersion
            {
                Type = u.__TYPE,
                Version = u.__VERSION
            };
        }

    }

    public interface IDao
    {
        public string __TYPE { get; }
        public int __VERSION { get; }
        public DateTimeOffset __UPDATED { get; }
    }
}