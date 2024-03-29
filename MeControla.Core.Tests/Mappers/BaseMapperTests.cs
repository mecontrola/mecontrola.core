﻿using AutoMapper;
using FluentAssertions;
using MeControla.Core.Mappers;
using MeControla.Core.Tests.Mocks.Datas.Dtos;
using MeControla.Core.Tests.Mocks.Datas.Entities;
using MeControla.Core.Tests.Mocks.Dtos;
using MeControla.Core.Tests.Mocks.Entities;
using System.Collections.Generic;
using Xunit;

namespace MeControla.Core.Tests.Mappers
{
    public class BaseMapperTests
    {
        private readonly IMapper<User, UserDto> mapper;

        public BaseMapperTests()
            => mapper = new UserEntityToDtoMapper();

        [Fact(DisplayName = "[BaseMapper.ToMap] Deve retornar null quando informado null.")]
        public void DeveRetornarNuloQuandoForNulo()
        {
            var actual = mapper.ToMap(null);

            Assert.Null(actual);
        }

        [Fact(DisplayName = "[BaseMapper.ToMap] Deve retornar dto quando informado entidade preenchido.")]
        public void DeveRetornarPreenchidoQuandoPreenchido()
        {
            var entity = UserMock.CreateUser1();
            var expected = UserDtoMock.CreateUser1();
            var actual = mapper.ToMap(entity);

            expected.Should().BeEquivalentTo(actual);
        }

        [Fact(DisplayName = "[BaseMapper.ToMap] Deve retornar dto quando informado entidade preenchido.")]
        public void DeveRetornarAlteradoQuandoPreenchidoComOutraObjeto()
        {
            var entity = UserMock.CreateUser1();
            var expected = UserDtoMock.CreateUser1();
            var actual = mapper.ToMap(entity, UserDtoMock.CreateUser2());

            expected.Should().BeEquivalentTo(actual);
        }

        [Fact(DisplayName = "[BaseMapper.ToMap] Deve retornar lista de dtos quando informado lista de entidades preenchidas.")]
        public void DeveRetornarListaPreenchidaQuandoPreenchida()
        {
            var entity = UserMock.CreateUser1();
            var expected = new List<UserDto> { UserDtoMock.CreateUser1() };
            var actual = mapper.ToMapList(new List<User> { entity });

            expected.Should().BeEquivalentTo(actual);
        }

        [Fact(DisplayName = "[BaseMapper.ToMap] Deve retornar um objeto utilizando o mapeamento padrão.")]
        public void DeveRetornarObjetoUtilizandoMapeamentoPadrao()
        {
            var entity = UserDtoMock.CreateUser1();
            var expected = UserDtoMock.CreateUser1();
            var actual = new UserDtoToDtoMapper().ToMap(entity);

            expected.Should().BeEquivalentTo(actual);
        }
    }

    class UserEntityToDtoMapper : BaseMapper<User, UserDto>
    {
        protected override void MapFields(IMappingExpression<User, UserDto> map)
            => map.ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Uuid))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name));
    }

    class UserDtoToDtoMapper : BaseMapper<UserDto, UserDto>
    { }
}