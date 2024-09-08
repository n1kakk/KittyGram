import {  CloseButton, Input } from "@chakra-ui/react";
import { Message } from "./Message";
import React, { useEffect, useRef, useState } from "react";
import { PiPawPrintLight } from "react-icons/pi";
import {deleteMessageForEveryone} from "../services/message";
import {updateMessage} from "../services/message";

export const Chat = ({messages, conversationName, closeConversation, sendMessage, currentUser, deleteMessage, editMessageApp}) =>{
    const[message, setMessage]=useState("");
    const [chatMessages, setChatMessages] = useState([]);
    const messagesEndRef = useRef(null);

    const [editMessage, setEditMessage] = useState(null);


    useEffect(() => {
        setChatMessages(messages);
        setTimeout(() => {
            messagesEndRef.current.scrollIntoView({ behavior: 'smooth' });
        }, 0);
    }, [messages]);


    const onSendMessage =(e)=>{
        e.preventDefault();
        if (message.trim() === "") {return;}

        const tempId = `temp-${Date.now()}`; // Генерация временного ID
        const newMessage = {
            conversationName: conversationName,
            senderNickname: currentUser,
            //messageType: text,
            messageContent: message,
            created: new Date(),
            tempId: tempId
        };
        sendMessage(newMessage);
        setMessage("");

    };

    const handleDeleteMessage = async (messageInfo) => {

            const messageToDelete = {
                tempId: messageInfo.tempId,
                messageId: messageInfo.messageId,
                currentUser: currentUser,
                chatId: messageInfo.conversationName,
                messageContent: messageInfo.messageContent
            };

            const success = await deleteMessageForEveryone(messageToDelete);
            if (success) {
                deleteMessage(messageInfo.messageId); // Удаление сообщения через родительский компонент
            }
            const updatedMessages = chatMessages.filter((message) => message.messageId !== messageInfo.messageId);
            setChatMessages(updatedMessages);     
    };

    const handleSayMeow = ()=>{
        const tempId = `temp-${Date.now()}`; // Генерация временного ID
        const newMessage = {
            conversationName: conversationName,
            senderNickname: currentUser,
            messageContent: "meow",
            created: new Date(),
            tempId: tempId
        };
        sendMessage(newMessage);
    }

    const formatDate = (dateString) => {
        const currentDate = new Date();
        const date = new Date(dateString);
    
        const month = date.toLocaleString('en-US', { month: 'short' });
        const day = date.getDate();
    
        let formattedDate = `${month} ${day}`;
    
        if (date.getFullYear() !== currentDate.getFullYear()) {
            formattedDate += `, ${date.getFullYear()}`;
        }
    
        return formattedDate;
    };

    const handleEditMessage = async (messageInfo) => {

        // const messageToEdit = {
        //     tempId: messageInfo.tempId,
        //     messageId: messageInfo.messageId,
        //     currentUser: currentUser,
        //     chatId: messageInfo.conversationName,
        //     messageContent: messageInfo.messageContent
        // };

        const success = await updateMessage(messageInfo);
        if (success) {
            //editMessageApp(messageInfo); // Update сообщения через родительский компонент

            const updatedMessage = { ...editMessage };
            setMessage(prevMessages =>
                prevMessages.map(m => m.messageId === editMessage.messageId ? updatedMessage : m)
            );
            handleEditCancel();
        }  
    };

    const handleEditClick = (messageInfo) => {
        setMessage(messageInfo.messageContent);
        setEditMessage({ ...messageInfo });
        //setEditMessageContent(message.messageContent);
    };

    const handleEditCancel = () => {
        setEditMessage(null);
        setMessage('');
    };




    return (
        <div className='md:min-w-[450px] w-full flex flex-col' >
       
            <>
            <div className='bg-[#f6f1f6] px-4 py-2 mb-2 flex justify-between items-center'>
                <span className='label-text '></span>{" "}
                <span className='text-[#918390] font-bold mr-auto'>{conversationName}</span>
                <CloseButton onClick={closeConversation}/>
            </div>
            <div className="px-4 flex-1 overflow-auto chat-container">
                {chatMessages.map((messageInfo, index) => {
                    const currentDate = formatDate(messageInfo.created);
                    const prevMessage = index > 0 ? chatMessages[index - 1] : null;
                    const prevDate = prevMessage ?formatDate(prevMessage.created) : null;
                    const isFirstMessageOfDay = prevDate !== currentDate;
                    //const formatedTime = timeString(messageInfo.created);

                    return (
                        <React.Fragment key={index}>
                            {isFirstMessageOfDay && (
                                <div className="date-header text-center">{currentDate}</div>
                            )}
                            <Message
                                messageInfo={messageInfo} 
                                currentUser={currentUser} 
                                deleteMessage={handleDeleteMessage} 
                                sayMeow={handleSayMeow}
                                editMessage={handleEditMessage}
                            />
                        </React.Fragment>
                    );
                })}
                <span ref={messagesEndRef} />
            </div>
            
            {editMessage && (
            <div>
                <p>Editing: {editMessageContent}</p>
                <button onClick={handleEditCancel}>Cancel</button>
            </div>
        )}


            <form className='px-2 my-10 mb-4 mt-3' onSubmit={onSendMessage}>
                <div className='w-full relative'>
                    <Input
                        type='text'
                        value={message}
                        onChange={(e) => setMessage(e.target.value)}
                        className='border text-sm rounded-lg block w-full p-2.5 border-[#d5d2d5] caret-[#f27bd8] text-[#463f46]'
                        placeholder='Message'
                        focusBorderColor='#f8eaf5'
                    />
                    <button type='submit' className='absolute inset-y-0 end-0 flex items-center pe-3'>
                        <PiPawPrintLight />
                    </button>
                </div>
            </form>


            {/* <form className=' px-2 my-10 mb-4 mt-3'>
                <div className='w-full relative'>
                    <Input type='text'
                        value={message} 
                        onChange={(e)=>setMessage(e.target.value)} 
                        className=' border text-sm rounded-lg block w-full p-2.5  border-[#d5d2d5] caret-[#f27bd8] text-[#463f46]'
                        placeholder='Message'
                        focusBorderColor='#f8eaf5'
                    />
                    <button className='absolute inset-y-0 end-0 flex items-center pe-3'onClick={onSendMessage}>
                    <PiPawPrintLight />
                    </button>
                    
                </div>
            </form> */}

            </>

    </div>
    )
}

