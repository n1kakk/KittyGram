﻿import React from 'react'
import Conversation from './Conversation';

const Conversations = () => {
  return (
    <div className='flex flex-col conversations-container'>
        <Conversation />
        <Conversation />
        <Conversation />
        <Conversation />
        <Conversation />
        <Conversation />
    </div>
  )
}

export default Conversations;