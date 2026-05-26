using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Features.Transactions.Commands.CreateDeposit;
using TransactionService.Application.Features.Transactions.Commands.CreateTransfer;
using TransactionService.Application.Features.Transactions.Commands.CreateWithdrawal;
using TransactionService.Application.Features.Transactions.Queries.GetTransaction;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/v1/transactions")]
public class TransactionsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var transaction = await mediator.Send(new GetTransactionQuery(id));

        return Ok(new { transaction });
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] CreateDepositCommand command)
    {
        var transactionId = await mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id = transactionId }, new { id = transactionId });
    }

    [HttpPost("withdrawal")]
    public async Task<IActionResult> Withdrawal([FromBody] CreateWithdrawalCommand command)
    {
        var withdrawalId = await mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id = withdrawalId }, new { id = withdrawalId });
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] CreateTransferCommand command)
    {
        var transferId = await mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id = transferId }, new { id = transferId });
    }
}