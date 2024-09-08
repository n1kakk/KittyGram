using AutoMapper;
using ClassLibrary1.Contracts;
using ClassLibrary1.Models;
using ClassLibrary1.Models.PostgreModels.Message;

namespace ClassLibrary1.MappingProfiles;

public class MessageProfile:Profile
{
    public MessageProfile()
    {
        CreateMap<MessageContract, Message>();
        CreateMap<Message, MessageContract>();

        CreateMap<Message, MessageJS>();
        CreateMap<MessageJS, Message>();

        CreateMap<MessageJS, MessageContract>();
        CreateMap<MessageContract, MessageJS>();

        CreateMap<Message, MessageInsertAckContract>();
        CreateMap<MessageInsertAckContract, Message>();

        CreateMap<UpdateDeleteMessage, DeleteMessageContract>();
        CreateMap<DeleteMessageContract, UpdateDeleteMessage>();

        CreateMap<UpdateDeleteMessage, UpdateMessageContract>();
        CreateMap<UpdateMessageContract, UpdateDeleteMessage>();
        //CreateMap<Message, MessageInsertAckContract>();
        //CreateMap<MessageInsertAckContract, Message>();
    }
}
