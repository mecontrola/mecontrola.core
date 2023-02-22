﻿using FluentAssertions;
using MeControla.Core.Tests.Mocks;
using MeControla.Core.Tests.Mocks.Datas.Repositories;
using MeControla.Core.Tests.Mocks.Entities;
using MeControla.Core.Tests.Mocks.Repositories;
using MeControla.Core.Tests.TestingTools;
using Xunit;

namespace MeControla.Core.Tests.Repositories
{
    public class BaseAsyncRepositoryTests : BaseAsyncMethods
    {
        private const long TOTAL_USERS = 3;

        private readonly IUserRepository userRepository;

        public BaseAsyncRepositoryTests()
            => userRepository = UserRepositoryMock.Create();

        [Fact(DisplayName = "[BaseAsyncRepository.CountAsync] Deve retornar a quantidade total de registros na tabela do banco de dados.")]
        public async void DeveListarTodosUsuarios()
        {
            var actual = await userRepository.CountAsync(GetCancellationToken());
            actual.Should().Be(TOTAL_USERS);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.CountAsync] Deve retornar a quantidade total de registros na tabela do banco de dados que coincidem com o criterio.")]
        public async void DeveListarTodosUsuariosDaCondicao()
        {
            var actual = await userRepository.CountAsync(entity => entity.Id == DataMock.ID_DEV_1, GetCancellationToken());
            actual.Should().Be(1);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.CreateAsync] Deve criar um usuario na tabela do banco de dados.")]
        public async void DeveCriarUsuario()
        {
            var user = UserMock.CreateUser4();
            user.Id = 0;

            await userRepository.CreateAsync(user, GetCancellationToken());

            var total = await userRepository.CountAsync(entity => entity.Id == user.Id, GetCancellationToken());
            total.Should().Be(1);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.UpdateAsync] Deve atualizar um usuario na tabela do banco de dados.")]
        public async void DeveAtualizarUsuario()
        {
            var expected = UserMock.CreateUser3();
            expected.Name = DataMock.NAME_DEV_4;

            await userRepository.UpdateAsync(expected, GetCancellationToken());

            var actual = await userRepository.FindAsync(entity => entity.Id == expected.Id, GetCancellationToken());
            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.RemoveAsync] Deve remover um usuario na tabela do banco de dados.")]
        public async void DeveRemoverUsuario()
        {
            var user = UserMock.CreateUser3();

            await userRepository.RemoveAsync(user, GetCancellationToken());

            var total = await userRepository.CountAsync(entity => entity.Id == user.Id, GetCancellationToken());
            total.Should().Be(0);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAllPagedAsync] Deve retornar lista paginada dos registros que existir na tabela do banco de dados.")]
        public async void DeveRetornarListaPaginadaComRegistros()
        {
            var pagination = PaginationFilterMock.CreatePage1();

            var actual = await userRepository.FindAllPagedAsync(pagination, GetCancellationToken());

            actual.Should().NotBeEmpty();
            actual.Should().HaveCount((int)TOTAL_USERS);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAllPagedAsync] Deve retornar lista paginada de todos os registros que exitir na tabela do banco de dados que coincidem com o criterio informado.")]
        public async void DeveRetornarListaPaginadaComRegistrosQuandoCriterioAtendido()
        {
            var pagination = PaginationFilterMock.CreatePage1();

            var actual = await userRepository.FindAllPagedAsync(pagination, entity => entity.Id < DataMock.ID_DEV_4, GetCancellationToken());

            actual.Should().NotBeEmpty();
            actual.Should().HaveCount((int)TOTAL_USERS);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAllPagedAsync] Deve retornar lista paginada vazia quando existir registro que não coincidem com o criterio informado.")]
        public async void DeveRetornarListaPaginadaSemRegistrosQuandoCriterioNaoAtendido()
        {
            var pagination = PaginationFilterMock.CreatePage1();

            var actual = await userRepository.FindAllPagedAsync(pagination, entity => entity.Id >= DataMock.ID_DEV_4, GetCancellationToken());

            actual.Should().BeEmpty();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAllAsync] Deve retornar lista de todos os registro que existir na tabela do banco de dados.")]
        public async void DeveRetornarListaRegistros()
        {
            var actual = await userRepository.FindAllAsync(GetCancellationToken());

            actual.Should().NotBeEmpty();
            actual.Should().HaveCount((int)TOTAL_USERS);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAllAsync] Deve retornar lista de todos os registro que exitir na tabela do banco de dados que coincidem com o criterio informado.")]
        public async void DeveRetornarListaRegistrosQuandoCriterioAtendido()
        {
            var actual = await userRepository.FindAllAsync(entity => entity.Id < DataMock.ID_DEV_4, GetCancellationToken());

            actual.Should().NotBeEmpty();
            actual.Should().HaveCount((int)TOTAL_USERS);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAllAsync] Deve retornar lista vazia quando existir registro que não coincidem com o criterio informado.")]
        public async void DeveRetornarListaVaziaQuandoCriterioNaoAtendido()
        {
            var actual = await userRepository.FindAllAsync(entity => entity.Id >= DataMock.ID_DEV_4, GetCancellationToken());

            actual.Should().BeEmpty();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAsync] Deve retornar objeto se exitir na tabela do banco de dados algum registro que conincida com o Id.")]
        public async void DeveRetornarObjetoQuandoIdInformado()
        {
            var expected = UserMock.CreateUser3();
            var actual = await userRepository.FindAsync(expected.Id, GetCancellationToken());

            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAsync] Deve retornar null não se exitir na tabela do banco de dados algum registro que conincida com o Id.")]
        public async void DeveRetornarNullQuandoIdInformado()
        {
            var exist = await userRepository.FindAsync(DataMock.ID_DEV_4, GetCancellationToken());

            exist.Should().BeNull();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAsync] Deve retornar objeto se exitir na tabela do banco de dados algum registro que conincida com o Guid.")]
        public async void DeveRetornarObjetoQuandoGuidInformado()
        {
            var expected = UserMock.CreateUser3();
            var actual = await userRepository.FindAsync(expected.Uuid, GetCancellationToken());

            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAsync] Deve retornar null não se exitir na tabela do banco de dados algum registro que conincida com o Guid.")]
        public async void DeveRetornarNullQuandoGuidInformado()
        {
            var exist = await userRepository.FindAsync(DataMock.UUID_DEV_4, GetCancellationToken());

            exist.Should().BeNull();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAsync] Deve retornar objeto se exitir na tabela do banco de dados algum registro que conincida com o criterio.")]
        public async void DeveRetornarObjetoQuandoCriteriaComCoincidencia()
        {
            var expected = UserMock.CreateUser3();
            var actual = await userRepository.FindAsync(entity => entity.Uuid == expected.Uuid, GetCancellationToken());

            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "[BaseAsyncRepository.FindAsync] Deve retornar null não se exitir na tabela do banco de dados algum registro que conincida com o criterio.")]
        public async void DeveRetornarNullQuandoCriteriaSemCoincidencia()
        {
            var exist = await userRepository.FindAsync(entity => entity.Uuid == DataMock.UUID_DEV_4, GetCancellationToken());

            exist.Should().BeNull();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.ExistsAsync] Deve retornar true se exitir na tabela do banco de dados algum registro que conincida com o Id.")]
        public async void DeveRetornarTrueQuandoIdInformado()
        {
            var exist = await userRepository.ExistsAsync(DataMock.ID_DEV_3, GetCancellationToken());

            exist.Should().BeTrue();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.ExistsAsync] Deve retornar false não se exitir na tabela do banco de dados algum registro que conincida com o Id.")]
        public async void DeveRetornarFalseQuandoIdInformado()
        {
            var exist = await userRepository.ExistsAsync(DataMock.ID_DEV_4, GetCancellationToken());

            exist.Should().BeFalse();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.ExistsAsync] Deve retornar true se exitir na tabela do banco de dados algum registro que conincida com o Guid.")]
        public async void DeveRetornarTrueQuandoGuidInformado()
        {
            var exist = await userRepository.ExistsAsync(DataMock.UUID_DEV_3, GetCancellationToken());

            exist.Should().BeTrue();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.ExistsAsync] Deve retornar false não se exitir na tabela do banco de dados algum registro que conincida com o Guid.")]
        public async void DeveRetornarFalseQuandoGuidInformado()
        {
            var exist = await userRepository.ExistsAsync(DataMock.UUID_DEV_4, GetCancellationToken());

            exist.Should().BeFalse();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.ExistsAsync] Deve retornar true se exitir na tabela do banco de dados algum registro que conincida com o criterio.")]
        public async void DeveRetornarTrueQuandoCriteriaInformado()
        {
            var exist = await userRepository.ExistsAsync(entity => entity.Uuid == DataMock.UUID_DEV_3, GetCancellationToken());

            exist.Should().BeTrue();
        }

        [Fact(DisplayName = "[BaseAsyncRepository.ExistsAsync] Deve retornar false não se exitir na tabela do banco de dados algum registro que conincida com o criterio.")]
        public async void DeveRetornarFalseQuandoCriteriaInformado()
        {
            var exist = await userRepository.ExistsAsync(entity => entity.Uuid == DataMock.UUID_DEV_4, GetCancellationToken());

            exist.Should().BeFalse();
        }
    }
}