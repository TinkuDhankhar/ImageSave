// ******************************************************************************
// ****    <copyright file="DapperDbContextExtensions.cs" company="nuuSoft Technology">     ****
// ****    Copyright (c) nuuSoft EnterPrises. All rights reserved.            ****
// ****    "</copyright>                                                     ****
// ******************************************************************************

using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;
using static Dapper.SqlMapper;

namespace ImageSave
{
	/// <summary>
	/// DapperDbContextExtensions
	/// </summary>
	public static class DapperDbContextExtensions
	{
		/// <summary>
		/// QueryAsync
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		/// <param name="timeout"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<T>> QueryAsync<T>(this DbContext context,
		string text,
		object? parameters = null,
		int? timeout = null,
		CommandType type = CommandType.StoredProcedure)
		{
			var cts = new CancellationTokenSource();
			var token = cts.Token;
			using var command = new DapperEFCoreCommand(
				context,
				text,
				parameters,
				timeout,
				type,
				token);
			try
			{
				var connection = context.Database.GetDbConnection();
				return await connection.QueryAsync<T>(command.Definition);
			}
			catch (Exception ex)
			{
				return new List<T>();
			}
		}
		/// <summary>
		/// QueryFirstAsync
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		/// <param name="timeout"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static async Task<T> QueryFirstAsync<T>(this DbContext context,
		string text,
		object? parameters = null,
		int? timeout = null,
		CommandType type = CommandType.StoredProcedure)
		{
			var cts = new CancellationTokenSource();
			var ct = cts.Token;
			using var command = new DapperEFCoreCommand(
				context,
				text,
				parameters,
				timeout,
				type,
				ct);
			try
			{
				var connection = context.Database.GetDbConnection();
				return await connection.QueryFirstAsync<T>(command.Definition);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		/// <summary>
		/// QueryFirstOrDefaultAsync
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		/// <param name="timeout"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static async Task<T> QueryFirstOrDefaultAsync<T>(this DbContext context,
		string text,
		object? parameters = null,
		int? timeout = null,
		CommandType type = CommandType.StoredProcedure)
		{
			var cts = new CancellationTokenSource();
			var ct = cts.Token;
			using var command = new DapperEFCoreCommand(
				context,
				text,
				parameters,
				timeout,
				type,
				ct);
			try
			{
				var connection = context.Database.GetDbConnection();
				return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		/// <summary>
		/// ExecuteAsync
		/// </summary>
		/// <param name="context"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		/// <param name="timeout"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static async Task<int> ExecuteAsync(this DbContext context,
			string text,
			object? parameters = null,
			int? timeout = null,
			CommandType type = CommandType.StoredProcedure)
		{
			var cts = new CancellationTokenSource();
			var ct = cts.Token;
			using var command = new DapperEFCoreCommand(
				context,
				text,
				parameters,
				timeout,
				type,
				ct);
			try
			{
				var connection = context.Database.GetDbConnection();
				return await connection.ExecuteAsync(command.Definition);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		/// <summary>
		/// ExecuteScalarAsync
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		/// <param name="timeout"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static async Task<T> ExecuteScalarAsync<T>(this DbContext context,
			string text,
			object? parameters = null,
			int? timeout = null,
			CommandType type = CommandType.StoredProcedure)
		{
			var cts = new CancellationTokenSource();
			var ct = cts.Token;
			using var command = new DapperEFCoreCommand(
				context,
				text,
				parameters,
				timeout,
				type,
				ct);
			try
			{
				var connection = context.Database.GetDbConnection();
				return await connection.ExecuteScalarAsync<T>(command.Definition);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		/// <summary>
		/// ExecuteScalarAsync
		/// </summary>
		/// <param name="context"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		/// <param name="timeout"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static async Task<object?> ExecuteScalarAsync(this DbContext context,
			string text,
			object? parameters = null,
			int? timeout = null,
			CommandType type = CommandType.StoredProcedure)
		{
			var cts = new CancellationTokenSource();
			var ct = cts.Token;
			using var command = new DapperEFCoreCommand(
				context,
				text,
				parameters,
				timeout,
				type,
				ct);
			try
			{
				var connection = context.Database.GetDbConnection();
				return await connection.ExecuteScalarAsync(command.Definition);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		/// <summary>
		/// QueryMultipleAsync
		/// </summary>
		/// <param name="context"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		/// <param name="timeout"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static async Task<GridReader> QueryMultipleAsync(this DbContext context,
			string text,
			object? parameters = null,
			int? timeout = null,
			CommandType type = CommandType.StoredProcedure)
		{
			var cts = new CancellationTokenSource();
			var ct = cts.Token;
			using var command = new DapperEFCoreCommand(
				context,
				text,
				parameters,
				timeout,
				type,
				ct);
			try
			{
				var connection = context.Database.GetDbConnection();
				return await connection.QueryMultipleAsync(command.Definition);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
	/// <summary>
	/// DapperEFCoreCommand
	/// </summary>
	public readonly struct DapperEFCoreCommand : IDisposable
	{
		private readonly ILogger<DapperEFCoreCommand> _logger;
		/// <summary>
		/// DapperEFCoreCommand
		/// </summary>
		/// <param name="context"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		/// <param name="timeout"></param>
		/// <param name="type"></param>
		/// <param name="ct"></param>
		public DapperEFCoreCommand(
			DbContext context,
			string text,
			object parameters,
			int? timeout,
			CommandType? type,
			CancellationToken ct)
		{
			_logger = context.GetService<ILogger<DapperEFCoreCommand>>();
			var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
			var commandType = type ?? CommandType.Text;
			var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;
			Definition = new CommandDefinition(
				text,
				parameters,
				transaction,
				commandTimeout,
				commandType,
				cancellationToken: ct);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug(
					@"Executing DbCommand [CommandType='{commandType}', CommandTimeout='{commandTimeout}']
{commandText}", Definition.CommandType, Definition.CommandTimeout, Definition.CommandText);
			}
		}
		public CommandDefinition Definition { get; }
		public void Dispose()
		{
			if (_logger.IsEnabled(LogLevel.Information))
			{
				_logger.LogInformation(
					@"Executed DbCommand [CommandType='{commandType}', CommandTimeout='{commandTimeout}']
{commandText}", Definition.CommandType, Definition.CommandTimeout, Definition.CommandText);
			}
		}
	}
}