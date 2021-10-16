using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Tsukiy0.Extensions.Data.Models;
using Tsukiy0.Extensions.Data.Services;
using FluentAssertions;

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
                Type = "TEST",
                Version = 2,
                Updated = DateTimeOffset.MaxValue,
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
                Type = "TEST",
                Version = 2,
                Updated = DateTimeOffset.MaxValue,
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
                Type = "TEST",
                Version = 1,
                Updated = DateTimeOffset.MaxValue,
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
        public bool Latest { get; set; }
    }

    public class DaoV1 : IDao
    {
        public string Type { get; set; }
        public int Version { get; set; }
        public DateTimeOffset Updated { get; set; }
        public string Latest { get; set; }
    }

    public class DaoV2 : IDao
    {
        public string Type { get; set; }
        public int Version { get; set; }
        public DateTimeOffset Updated { get; set; }
        public bool Latest { get; set; }
    }

    public class V1DaoMapper : IDaoMapper<Dto, IDao>
    {
        public async Task<Dto> From(IDao source)
        {
            if (source is not DaoV1 v1)
            {
                throw new InvalidCastException();
            }

            return new Dto
            {
                Latest = v1.Latest == "true"
            };
        }

        public async Task<IDao> To(Dto destination)
        {
            return new DaoV1
            {
                Version = 1,
                Updated = DateTimeOffset.MaxValue,
                Type = "TEST",
                Latest = destination.Latest.ToString()
            };
        }
    }

    public class V2DaoMapper : IDaoMapper<Dto, IDao>
    {
        public async Task<Dto> From(IDao source)

        {
            if (source is not DaoV2 v1)
            {
                throw new InvalidCastException();
            }

            return new Dto
            {
                Latest = v1.Latest
            };
        }

        public async Task<IDao> To(Dto destination)
        {
            return new DaoV2
            {
                Version = 2,
                Updated = DateTimeOffset.MaxValue,
                Type = "TEST",
                Latest = destination.Latest
            };
        }
    }

    public class VersionDaoMapper : AbstractVersionDaoMapper<Dto, IDao>
    {
        public bool HadVersionMismatch { get; set; }

        public VersionDaoMapper() : base(
            new List<VersionMapper<Dto, IDao>>
            {
                new VersionMapper<Dto, IDao> (
                    Version: 1,
                    Mapper: new V1DaoMapper()
                ),
                new VersionMapper<Dto, IDao>(
                    Version: 2,
                    Mapper: new V2DaoMapper()
                )
            }
        )
        {
            HadVersionMismatch = false;
        }

        protected override async Task OnVersionMismatch(Dto dto, int currentVersion)
        {
            HadVersionMismatch = true;
        }

        protected override IDao ToDao(IDao u)
        {
            return u;
        }
    }
}
