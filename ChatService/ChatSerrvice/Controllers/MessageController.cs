using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceServices.IPostgresService;
using ClassLibrary1.Models.PostgreModels.Message;
using ClassLibrary1.Publishers;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Replication.PgOutput.Messages;

namespace ChatService.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController: ControllerBase
{
    private readonly IMessagePostgresService _messagePostgres;
    private readonly MessageDeletePublisher _messageDeletePublisher;
    private readonly MessageEditPublisher _messageEditPublisher;
    public MessageController(IMessagePostgresService messagePostgres, MessageDeletePublisher messageDeletePublisher,
        MessageEditPublisher messageEditPublisher)
    {
        _messagePostgres = messagePostgres;
        _messageDeletePublisher = messageDeletePublisher;
        _messageEditPublisher = messageEditPublisher;
    }


    //поменять этот метод, должен принимать какую-то дату 
    [HttpGet("GetMessagesForMonth")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetMessagesForMonthPostgres(int conversationId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        DateTime fromDateTime = DateTime.UtcNow;

        var res = await _messagePostgres.GetMessagesForMonthAsync(conversationId, fromDateTime);

        return Ok(res);
    }

    [HttpDelete("DeleteMessageForEveryone")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DeleteMessageForEveryoneAsync(DeleteMessageContract deleteMessage)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _messageDeletePublisher.PublishDeleteMessageAsync(deleteMessage);

        return Ok();
    }



    [HttpGet("GetMessagesByDate")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetMessagesByDate(int conversationId, DateTime date)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var ne = DateTime.UtcNow;

        var res = await _messagePostgres.GetMessagesByDateAsync(conversationId, date);

        return Ok(res);
    }


    [HttpPost("EditMessage")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> EditMessage(UpdateMessageContract updateMessage)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _messageEditPublisher.PublishEditMessageAsync(updateMessage);

        return Ok();
    }
}
