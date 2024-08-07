﻿using FluentAssertions;
using MeControla.Core.Extensions;
using System.Collections.Generic;
using Xunit;

namespace MeControla.Core.Tests.Extensions
{
    public class ListExtensionTests
    {
        [Fact(DisplayName = "[ListExtension.IsNullOrEmpty] Deve retornar true quando a lista for nula.")]
        public void DeveRetornarVerdadeiroQuandoNulo()
        {
            var list = (List<string>)null;

            list.IsNullOrEmpty().Should().BeTrue();
        }

        [Fact(DisplayName = "[ListExtension.IsNullOrEmpty] Deve retornar true quando a lista estiver vazia.")]
        public void DeveRetornarVerdadeiroQuandoVazio()
        {
            var list = new List<string> { };

            list.IsNullOrEmpty().Should().BeTrue();
        }

        [Fact(DisplayName = "[ListExtension.IsNullOrEmpty] Deve retornar false quando a lista estiver preenchido.")]
        public void DeveRetornarFalsoQuandoPreenchido()
        {
            var list = new List<string> { "Teste" };

            list.IsNullOrEmpty().Should().BeFalse();
        }

        [Fact(DisplayName = "[ListExtension.IsNotNullAndAny] Deve retornar false quando a lista for nula.")]
        public void DeveRetornarFalsoQuandoNulo()
        {
            var list = (List<string>)null;

            list.IsNotNullAndAny().Should().BeFalse();
        }

        [Fact(DisplayName = "[ListExtension.IsNotNullAndAny] Deve retornar false quando a lista estiver vazia.")]
        public void DeveRetornarFalsoQuandoVazio()
        {
            var list = new List<string> { };

            list.IsNotNullAndAny().Should().BeFalse();
        }

        [Fact(DisplayName = "[ListExtension.IsNotNullAndAny] Deve retornar true quando a lista estiver preenchido.")]
        public void DeveRetornarVerdadeiroQuandoPreenchido()
        {
            var list = new List<string> { "Teste" };

            list.IsNotNullAndAny().Should().BeTrue();
        }

        [Fact(DisplayName = "[ListExtension.ToListOrEmpty] Deve retornar lista vazia quando a lista for nula.")]
        public void DeveRetornarListaVaziaQuandoNulo()
        {
            var list = (List<string>)null;
            var actual = list.ToListOrEmpty();

            actual.Should().NotBeNull();
            actual.Should().BeEmpty();
        }

        [Fact(DisplayName = "[ListExtension.ToListOrEmpty] Deve retornar lista vazia quando a lista estiver vazia.")]
        public void DeveRetornarListaVaziaQuandoVazio()
        {
            var list = new List<string> { };
            var actual = list.ToListOrEmpty();

            actual.Should().NotBeNull();
            actual.Should().BeEmpty();
        }

        [Fact(DisplayName = "[ListExtension.ToListOrEmpty] Deve retornar a lista quando a lista estiver preenchido.")]
        public void DeveRetornarPreenchidoQuandoPreenchido()
        {
            var list = new List<string> { "Teste" };
            var actual = list.ToListOrEmpty();

            actual.Should().NotBeNull();
            actual.Should().NotBeEmpty();
        }
    }
}