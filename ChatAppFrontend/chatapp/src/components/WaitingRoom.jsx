import {Button, Heading, Input, Text} from "@chakra-ui/react";
import {useState} from "react";

export const WaitingRoom =({joinChat})=>{
    const[nickname, setNickname] = useState();
    const[conversationName, setConversationName] = useState();

    const onSubmit =(e) =>{
        e.preventDefault();
        joinChat(nickname, conversationName);
    }
    
    return <form onSubmit={onSubmit} className="flex items-center justify-center w-full h-full">
    <div className='px-4 text-left sm:text-lg md:text-xl text-gray-700 font-semibold flex flex-col
items-center gap-2'>
        <Heading>Chat</Heading>
    <div className="mb-4">
        <Text fontSize={"sm"}>UserName</Text>
        <Input onChange={(e) => setNickname(e.target.value)} name="nickname" placeholder="Input your name"/>
    </div>
    <div className="mb-4">
        <Text fontSize={"sm"}>ConversationName</Text>
        <Input onChange={(e) => setConversationName(e.target.value)} name="ConversationName" placeholder="Input conv name"/>
    </div>
    <Button type="submit" backgroundColor="#f8eaf5" textColor={"gray-700"}>Join</Button>
    </div>
    </form>

}