import React from 'react'
import { BiSolidCat } from "react-icons/bi";

const Conversation = () => {
  return <>
  <div className='gap-2 items-center rounded p-2 py-1 cursor-pointer flex conv-pointer '>
    <div className='avatar online'>
        <div className='w-12 rounded-full'>
        <BiSolidCat className='w-9 h-9 outline-none viewBox ="0 0 24 18"'/>
            {/* <img src="" alt="user avatar" /> */}
        </div>
    </div>
    <div className='flex flex-col flex-1'>
        <div className='flex gap-3 justify-between items-center'>
            <p className='font-normal'>Nika</p>
            <span></span>
        </div>
    </div>
  </div>

  <div className='divider my-0 py-0 h-0.2'/>
  </>
}

export default Conversation;