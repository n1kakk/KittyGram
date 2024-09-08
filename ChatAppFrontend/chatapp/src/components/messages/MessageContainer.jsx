import React from 'react'
import Messages from './Messages';
import MessageInput from './MessageInput';
import {TiMessages} from 'react-icons/ti';

const MessageContainer = () => {
    const noChatSelected = false;
  return (
    <div className='md:min-w-[450px] w-full flex flex-col'>
        {noChatSelected ? (
        <NoChatSelected/>):(        
        <>
        <div className='bg-[#fcdcf5] px-4 py-2 mb-2'>
            <span className='label-text'></span>{" "}
            <span className='text-[#918390] font-bold'>Nika</span>
        </div>
        <Messages />
        <MessageInput/>
        </>
    )}
    </div>
  )
}

export default MessageContainer;

const NoChatSelected = ()=>{
    return (
        <div className='flex items-center justify-center w-full h-full'>
            <div className='px-4 text-center sm:text-lg md:text-xl text-gray-200 font-semibold flex flex-col
            items-center gap-2'>
                <p>Welcome Nika</p>
                <p>Select a chat to start messaging</p>
                <TiMessages className ='text-3xl md:text-6xl text-center'/>
            </div>
        </div>
    );
}